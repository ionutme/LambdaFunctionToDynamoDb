using System.Threading.Tasks;

namespace LambdaFunctionNamespace
{
    public class MetadataRepository
    {
        private readonly DynamoDbService _dynamoDbService;

        public MetadataRepository()
        {
            _dynamoDbService = new DynamoDbService();
        }

        public async Task<Metadata> GetAsync(string processName)
        {
            return await _dynamoDbService.LoadAsync<Metadata>(processName);
        }
    }
}
