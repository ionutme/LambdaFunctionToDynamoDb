using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace LambdaFunctionNamespace.DataModel
{
    public class Conditions
    {
        [DynamoDBProperty("asset_type")]
        public List<string> AssetType { get; set; }

        [DynamoDBProperty("date")]
        public string Date { get; set; }
    }
}