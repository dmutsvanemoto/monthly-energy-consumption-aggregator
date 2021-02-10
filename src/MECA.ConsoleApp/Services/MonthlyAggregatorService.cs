using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MECA.ConsoleApp.Models;

namespace MECA.ConsoleApp.Services
{
    public class MonthlyAggregatorService : IMonthlyAggregatorService
    {
        public Task<Dictionary<string, int>> Aggregate(IList<ConsumptionData> consumptionData)
        {
            if (consumptionData == null || !consumptionData.Any())
            {
                throw new ArgumentNullException(nameof(consumptionData));
            }

            // TODO: Group By Month Year

            var grouped = consumptionData.GroupBy(x => x.Date.ToString("MMM yyyy")).ToList();

            // TODO: ForEach Month Year Grab recent consumption
            var items = new Dictionary<string, int>();
            foreach (var group in grouped)
            {
                var recent = group.OrderByDescending(x => x.Date).Select(x => x.Consumption).FirstOrDefault();
                items.Add(group.Key, recent);
            }

            var actualConsumptionData = new Dictionary<string, int>();

            for (var i = 0; i < items.Count; i++)
            {
                var current = items.ElementAt(i);

                if (i == 0)
                {
                    actualConsumptionData.Add(current.Key, current.Value);
                    continue;
                }

                var previous = items.ElementAt(i - 1);

                var actual = current.Value - previous.Value;

                actualConsumptionData.Add(current.Key, actual);
            }
            
            return Task.FromResult(actualConsumptionData);
        }
    }
}