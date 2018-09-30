using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.TestUtilities;
using LambdaFunctionNamespace;
using LambdaFunctionNamespace.DataModel;
using LambdaFunctionNamespace.Repository;
using Moq;
using Xunit;

namespace LambdaFunctionTestsNamespace
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestFunction()
        {
            var mockMetadataRepository = new Mock<IMetadataRepository>();
            mockMetadataRepository.Setup(x => x.GetAsync("BestXFXDistribution"))
                .Returns(Task.FromResult(new Metadata
                {
                    Payload = new Payload
                    {
                        Mappings = new List<Mapping> {new Mapping {Source = "test_src", Target = "test_target"}}
                    }
                }));

            var function = new Function(mockMetadataRepository.Object);

            var result = await function.Handler("whatever", new TestLambdaContext());

            Assert.Equal("test_src -> test_target", result);
        }
    }
}
