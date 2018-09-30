using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace LambdaFunctionNamespace
{
    public class DynamoDbService
    {
        internal readonly AmazonDynamoDBClient DynamoDbClient;

        public DynamoDbService()
        {
            DynamoDbClient = new AmazonDynamoDBClient();
        }

        public async Task<T> LoadAsync<T>(string key)
            where T : IEntity
        {
            using (var context = new DynamoDBContext(DynamoDbClient))
            {
                return await context.LoadAsync<T>(key);
            }
        }
    }
}