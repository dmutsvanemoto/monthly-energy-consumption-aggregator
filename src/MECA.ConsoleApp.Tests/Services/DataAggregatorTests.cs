using System;
using System.Threading.Tasks;
using MECA.ConsoleApp.Services;
using Moq;
using Xunit;

namespace MECA.ConsoleApp.Tests.Services
{
    public class DataAggregatorTests
    {
        [Fact]
        public async Task WhenWeCantLocateFileThenErrorIsThrown()
        {
            var fileLoader = Mock.Of<IFileLoaderService>(x => x
                .LocateFile(Constants.IncomingFolder) == null);

            var aggregator = new DataAggregatorService(fileLoader);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await aggregator.Aggregate());
        }
    }
}
