using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MECA.ConsoleApp.Models;
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
                .LocateFile(Constants.IncomingFolder) == Task.FromResult(null as string));

            var aggregator = new DataAggregatorService(fileLoader);

            Func<Task> act = async () => await aggregator.Aggregate();

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{Constants.MissingFileMessage}");
        }
        
        [Fact]
        public async Task WhenWeReadFileFromLocationWithNoDataThenExceptionIsThrown()
        {
            var filename = "some_file.csv";
            var filePath = $"{Constants.IncomingFolder}/{filename}";

            var fileLoader = Mock.Of<IFileLoaderService>(x => 
                x.LocateFile(Constants.IncomingFolder) == Task.FromResult(filePath)
                && x.ReadFile(filePath) == Task.FromResult<IEnumerable<ConsumptionData>>(null));

            var aggregator = new DataAggregatorService(fileLoader);

            Func<Task> act = async () => await aggregator.Aggregate();

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{Constants.NoDataToProcessMessage}");
        }
    }
}
