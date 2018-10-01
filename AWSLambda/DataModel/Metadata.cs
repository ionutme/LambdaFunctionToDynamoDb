using Amazon.DynamoDBv2.DataModel;
using LambdaFunctionNamespace.Services;

namespace LambdaFunctionNamespace.DataModel
{
    [DynamoDBTable(DynamoDbConfig.TableName)]
    public class Metadata : IEntity
    {
        [DynamoDBHashKey("ProcessName")]
        public string ProcessName { get; set; }

        [DynamoDBProperty("jsonld")]
        public Payload Payload { get; set; }
    }
}