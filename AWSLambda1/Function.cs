using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaFunctionNamespace
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Handler(string input, ILambdaContext context)
        {
            Task<string> response = GetResponse(input);
            response.Wait();

            return response.Result;
        }

        private static async Task<string> GetResponse(string input)
        {
            var dynamoDb = new AmazonDynamoDBClient();

            if (await IsTableAvailable(dynamoDb, "tbl_metadata"))
            {
                var context = new DynamoDBContext(dynamoDb);

                bool exists = await ExistsRecord(dynamoDb, "tbl_metadata", nameof(Metadata.ProcessName), "SimcorpDistributionProcess");
                if (!exists)
                {
                    await InsertProcess(context, "SimcorpDistributionProcess");
                    Console.WriteLine("Insert new record");

                    return "insert new record";
                }
                else
                {
                    return "record exists";
                }
            }

            return "Table not available!";
        }

        private static async Task<bool> ExistsRecord(IAmazonDynamoDB dynamoDb, string table, string attributeName, string attributeValue)
        {
            var item = await dynamoDb.GetItemAsync(
                tableName: table,
                key: new Dictionary<string, AttributeValue>
                {
                    {attributeName, new AttributeValue(attributeValue)}
                });

            return item?.Item[attributeName] != null;
        }

        private static async Task<string> InsertProcess(IDynamoDBContext context, string attributeValue)
        {
            await context.SaveAsync(new Metadata {ProcessName = attributeValue});

            return "Item added!";
        }

        private static async Task<bool> IsTableAvailable(AmazonDynamoDBClient dynamoDb, string table)
        {
            var tableResponse = await dynamoDb.ListTablesAsync();
            if (tableResponse.TableNames.Contains(table))
            {
                return true;
            }

            await WaitForTableToBecomeAvailable(dynamoDb, table);

            return false;
        }

        private static async Task WaitForTableToBecomeAvailable(IAmazonDynamoDB dynamoDb, string table)
        {
            bool isTableAvailable = false;
            while (!isTableAvailable)
            {
                Thread.Sleep(5000);

                var tableStatus = await dynamoDb.DescribeTableAsync(table);
                isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
            }
        }
    }
}
