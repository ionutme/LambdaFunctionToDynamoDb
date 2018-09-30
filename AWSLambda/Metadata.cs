using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace LambdaFunctionNamespace
{
    public interface IEntity
    {
    }

    [DynamoDBTable("tbl_metadata")]
    public class Metadata : IEntity
    {
        [DynamoDBHashKey("ProcessName")]
        public string ProcessName { get; set; }

        [DynamoDBProperty("jsonld")]
        public Payload Payload { get; set; }
    }

    public class Payload
    {
        [DynamoDBProperty("@context")]
        public string Context { get; set; }

        [DynamoDBProperty("@type")]
        public string Type { get; set; }

        [DynamoDBProperty("conditions")]
        public Conditions Conditions { get; set; }

        [DynamoDBProperty("mappings")]
        public List<Mapping> Mappings { get; set; }
    }

    public class Conditions
    {
        [DynamoDBProperty("asset_type")]
        public List<string> AssetType { get; set; }

        [DynamoDBProperty("date")]
        public string Date { get; set; }
    }

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