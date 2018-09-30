using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace LambdaFunctionNamespace
{
    public class Function
    {
        private readonly MetadataRepository _metadataRepository;

        public Function() : this(new MetadataRepository())
        {
            
        }
        public Function(MetadataRepository metadataRepository)
        {
            _metadataRepository = metadataRepository;
        }

        // This attribute tells the Lambda runtime how to serialize & deserialize event and result types.
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<string> Handler(string input, ILambdaContext context)
        {
            return await GetResponse();
        }

        private async Task<string> GetResponse()
        {
            var metadata = await _metadataRepository.GetAsync("BestXFXDistribution");

            return GetFirstMapping(metadata);
        }

        private static string GetFirstMapping(Metadata metadata)
        {
            return metadata.Payload.Mappings.First().Source + " -> " + metadata.Payload.Mappings.First().Target;
        }
    }
}
