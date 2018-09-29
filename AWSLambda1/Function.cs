using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

                //return await GetRecord(dynamoDb, context);
                //return await SearchForRecord(context);
                return await LoadRecord(context);
            }

            return "Table not available!";
        }

        private static async Task<string> LoadRecord(IDynamoDBContext context)
        {
            var metadata = await context.LoadAsync<Metadata>("BestXFXDistribution");

            return metadata.Payload.Conditions.AssetType.First();
        }

        private static Task<string> SearchForRecord(IDynamoDBContext context)
        {
            AsyncSearch<Metadata> x = context.QueryAsync<Metadata>("BestXFXDistribution");
            Task<List<Metadata>> searchTask = x.GetRemainingAsync();
            searchTask.Wait();

            List<Metadata> results = searchTask.Result;

            return Task.FromResult(results?.Single().ProcessName);
        }

        private static async Task<string> GetRecord(AmazonDynamoDBClient dynamoDb, DynamoDBContext context)
        {
            Dictionary<string, AttributeValue> metadata = await GetMetadata(dynamoDb, "SimcorpDistributionProcess");
            if (metadata == null)
            {
                await InsertProcess(context, "SimcorpDistributionProcess");
                Console.WriteLine("Insert new record");

                return "insert new record";
            }
            else
            {
                return metadata["Description"].S;
            }
        }

        private static async Task<Dictionary<string, AttributeValue>> GetMetadata(IAmazonDynamoDB dynamoDb, string attributeValue)
        {
            var item = await dynamoDb.GetItemAsync(
                tableName: "tbl_metadata",
                key: new Dictionary<string, AttributeValue>
                {
                    {nameof(Metadata.ProcessName), new AttributeValue(attributeValue)}
                });

            return item?.Item;
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
