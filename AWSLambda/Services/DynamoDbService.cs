using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using LambdaFunctionNamespace.DataModel;

namespace LambdaFunctionNamespace.Services
{
    public class DynamoDbService
    {
        internal readonly AmazonDynamoDBClient DynamoDbClient;

        public DynamoDbService()
        {
            DynamoDbClient = new AmazonDynamoDBClient();
        }

        public async Task<bool> IsTableAvailable(string tableName)
        {
            var tableInfo = await DynamoDbClient.DescribeTableAsync(tableName);

            return IsTableStatusActive(tableInfo);
        }

        public async Task<T> LoadAsync<T>(string key)
            where T : IEntity
        {
            using (var context = new DynamoDBContext(DynamoDbClient))
            {
                return await context.LoadAsync<T>(key);
            }
        }

        public async Task SaveAsync<T>(T entity)
            where T : IEntity
        {
            using (var context = new DynamoDBContext(DynamoDbClient))
            {
                await context.SaveAsync(entity);
            }
        }

        private static bool IsTableStatusActive(DescribeTableResponse tableInfo)
        {
            return tableInfo.Table.TableStatus.Equals(TableStatus.ACTIVE);
        }
    }
}