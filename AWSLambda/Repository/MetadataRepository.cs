using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
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
            return await _dynamoDbService.GetObjectAsync<Metadata>(processName);
        }

        public async Task<Document> GetDocumentAsync(string processName)
        {
            return await _dynamoDbService.GetDocumentAsync("tbl_metadata", processName);
        }
    }

    public interface IMetadataRepository
    {
        Task<Metadata> GetAsync(string processName);
    }
}
