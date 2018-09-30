using System.Threading.Tasks;
using LambdaFunctionNamespace.DataModel;
using LambdaFunctionNamespace.Services;

namespace LambdaFunctionNamespace.Repository
{
    public class MetadataRepository : IMetadataRepository
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

    public interface IMetadataRepository
    {
        Task<Metadata> GetAsync(string processName);
    }
}
