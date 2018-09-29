using System;
using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;

namespace LambdaFunctionNamespace
{
    [DynamoDBTable("tbl_metadata")]
    public class Metadata
    {
        [DynamoDBHashKey("ProcessName")]
        public string ProcessName { get; set; }

        [DynamoDBProperty("jsonld")]
        public Jsonld Jsonld { get; set; }
    }

    public class Jsonld
    {
        [DynamoDBProperty("@context")]
        public Uri Context { get; set; }

        [DynamoDBProperty("@type")]
        public string Type { get; set; }

        [DynamoDBProperty("conditions")]
        public Conditions Conditions { get; set; }

        [DynamoDBProperty("mappings")]
        public Mapping[] Mappings { get; set; }
    }

    public class Conditions
    {
        [DynamoDBProperty("asset_type")]
        public string[] AssetType { get; set; }

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