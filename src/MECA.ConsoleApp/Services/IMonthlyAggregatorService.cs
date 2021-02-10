using System.Collections.Generic;
using System.Threading.Tasks;
using MECA.ConsoleApp.Models;

namespace MECA.ConsoleApp.Services
{
    public interface IMonthlyAggregatorService
    {
        Task<Dictionary<string, int>> Aggregate(IEnumerable<ConsumptionData> consumptionData);
    }
}