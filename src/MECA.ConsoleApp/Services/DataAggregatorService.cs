using System;
using System.Linq;
using System.Threading.Tasks;

namespace MECA.ConsoleApp.Services
{
    public class DataAggregatorService
    {
        private readonly IFileLoaderService _fileLoaderService;
        private readonly IMonthlyAggregatorService _monthlyAggregatorService;

        public DataAggregatorService(IFileLoaderService fileLoaderService, IMonthlyAggregatorService monthlyAggregatorService)
        {
            _fileLoaderService = fileLoaderService;
            _monthlyAggregatorService = monthlyAggregatorService;
        }

        public async Task Aggregate()
        {
            var fileLocation = await _fileLoaderService.LocateFile(Constants.IncomingFolder);

            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                throw new InvalidOperationException(Constants.MissingFileMessage);
            }

            var consumptionData = await _fileLoaderService.ReadFile(fileLocation);

            if (consumptionData == null)
            {
                throw new InvalidOperationException(Constants.NoDataToProcessMessage);
            }
            
            var aggregatedData = await _monthlyAggregatorService.Aggregate(consumptionData);

            await _fileLoaderService.WriteToFile(aggregatedData);
        }
    }
}