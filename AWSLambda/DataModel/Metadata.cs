using Amazon.DynamoDBv2.DataModel;

namespace LambdaFunctionNamespace.DataModel
{
    [DynamoDBTable("tbl_metadata")]
    public class Metadata : IEntity
    {
        [DynamoDBHashKey("ProcessName")]
        public string ProcessName { get; set; }

        [DynamoDBProperty("jsonld")]
        public Payload Payload { get; set; }
    }
}