using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MECA.ConsoleApp.Models;
using MECA.ConsoleApp.Services;
using Xunit;

namespace MECA.ConsoleApp.Tests.Services
{
    public class MonthlyAggregatorServiceTests
    {
        [Fact]
        public async Task GivenNullDataThenThrowException()
        {
            var service = new MonthlyAggregatorService();
            
            Func<Task> act = async () => await service.Aggregate(null);

            await act.Should()
                .ThrowAsync<ArgumentNullException>()
                .WithMessage("*consumptionData*");
        }

        [Fact]
        public async Task GivenEmptyDataThenThrowException()
        {
            var service = new MonthlyAggregatorService();

            var data = new List<ConsumptionData>();

            Func<Task> act = async () => await service.Aggregate(data);

            await act.Should()
                .ThrowAsync<ArgumentNullException>()
                .WithMessage("*consumptionData*");
        }


        // BUG: Will break on the first of every month because the penultimate entry will fall in the month before instead of current. Can resolved by using hardcorded dates.
        [Fact]
        public async Task GivenValidDataThenReturnAggregated()
        {
            var service = new MonthlyAggregatorService();

            var data = new List<ConsumptionData>
            {
                new ConsumptionData() { Date = DateTime.Now.AddMonths(-5), Consumption = 123 },
                new ConsumptionData() { Date = DateTime.Now.AddMonths(-4), Consumption = 234 },
                new ConsumptionData() { Date = DateTime.Now.AddMonths(-3), Consumption = 345 },
                new ConsumptionData() { Date = DateTime.Now.AddMonths(-2), Consumption = 456 },
                new ConsumptionData() { Date = DateTime.Now.AddMonths(-1).AddDays(-1), Consumption = 557 },
                new ConsumptionData() { Date = DateTime.Now.AddMonths(-1), Consumption = 678 },
            };

            var expectedOutput = new Dictionary<string, int>()
            {
                {CustomDateFormat(DateTime.Now.AddMonths(-5)), 123},
                {CustomDateFormat(DateTime.Now.AddMonths(-4)), 111},
                {CustomDateFormat(DateTime.Now.AddMonths(-3)), 111},
                {CustomDateFormat(DateTime.Now.AddMonths(-2)), 111},
                {CustomDateFormat(DateTime.Now.AddMonths(-1)), 222}
            };

            var actualOutput = await service.Aggregate(data);

            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }

        private string CustomDateFormat(DateTime date) => date.ToString("MMM yyyy");
    }
}
