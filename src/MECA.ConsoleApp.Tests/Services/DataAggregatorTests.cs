using System;
using System.Collections.Generic;
using System.Linq;
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
            var monthlyAggregator = Mock.Of<IMonthlyAggregatorService>();
            var fileLoader = Mock.Of<IFileLoaderService>(x => x
                .LocateFile(Constants.IncomingFolder) == Task.FromResult(null as string));

            var aggregator = new DataAggregatorService(fileLoader, monthlyAggregator);

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

            var monthlyAggregator = Mock.Of<IMonthlyAggregatorService>();
            var fileLoader = Mock.Of<IFileLoaderService>(x =>
                x.LocateFile(Constants.IncomingFolder) == Task.FromResult(filePath)
                && x.ReadFile(filePath) == Task.FromResult<IList<ConsumptionData>>(null));

            var aggregator = new DataAggregatorService(fileLoader, monthlyAggregator);

            Func<Task> act = async () => await aggregator.Aggregate();

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage($"*{Constants.NoDataToProcessMessage}");
        }

        [Fact]
        public async Task WhenWeReadFileFromLocationWithDataThenWeAggregateAndWriteToFile()
        {
            var filename = "some_file.csv";
            var filePath = $"{Constants.IncomingFolder}/{filename}";
            var data = new List<ConsumptionData>
            {
                new ConsumptionData {Date = DateTime.Now.AddMonths(-3), Consumption = 0},
            };

            var formattedData = data.ToDictionary(k => k.Date.ToString("yyyy MMM"), v => v.Consumption);

            var monthlyAggregator = Mock.Of<IMonthlyAggregatorService>(x => x
                .Aggregate(data) == Task.FromResult(formattedData));
            
            var fileLoader = Mock.Of<IFileLoaderService>(x =>
                x.LocateFile(Constants.IncomingFolder) == Task.FromResult(filePath)
                && x.ReadFile(filePath) == Task.FromResult<IList<ConsumptionData>>(data)
                && x.WriteToFile(formattedData) == Task.CompletedTask);
            
            var aggregator = new DataAggregatorService(fileLoader, monthlyAggregator);

            await aggregator.Aggregate();
        }
    }
}
