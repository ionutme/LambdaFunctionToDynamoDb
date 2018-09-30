using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace LambdaFunctionNamespace.DataModel
{
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
}