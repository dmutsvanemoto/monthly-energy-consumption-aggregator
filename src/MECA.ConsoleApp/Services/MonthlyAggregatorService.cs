using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MECA.ConsoleApp.Models;

namespace MECA.ConsoleApp.Services
{
    public class MonthlyAggregatorService : IMonthlyAggregatorService
    {
        public Task<Dictionary<string, int>> Aggregate(IEnumerable<ConsumptionData> consumptionData)
        {
            throw new NotImplementedException();
        }
    }
}