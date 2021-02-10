using System;
using System.Threading.Tasks;

namespace MECA.ConsoleApp.Services
{
    public class DataAggregatorService
    {
        private readonly IFileService _fileService;
        private readonly IMonthlyAggregatorService _monthlyAggregatorService;

        public DataAggregatorService(IFileService fileService, IMonthlyAggregatorService monthlyAggregatorService)
        {
            _fileService = fileService;
            _monthlyAggregatorService = monthlyAggregatorService;
        }

        public async Task Aggregate()
        {
            var fileLocation = await _fileService.LocateFile(Constants.IncomingFolder);

            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                throw new InvalidOperationException(Constants.MissingFileMessage);
            }

            var consumptionData = await _fileService.ReadFile(fileLocation);

            if (consumptionData == null)
            {
                throw new InvalidOperationException(Constants.NoDataToProcessMessage);
            }
            
            var aggregatedData = await _monthlyAggregatorService.Aggregate(consumptionData);

            await _fileService.WriteToFile(aggregatedData);
        }
    }
}