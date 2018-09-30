using Amazon.DynamoDBv2.DataModel;

namespace LambdaFunctionNamespace.DataModel
{
    public class Mapping
    {
        [DynamoDBProperty("operation")]
        public string Operation { get; set; }

        [DynamoDBProperty("src_data")]
        public string Source { get; set; }

        [DynamoDBProperty("tgt_data")]
        public string Target { get; set; }
    }
}