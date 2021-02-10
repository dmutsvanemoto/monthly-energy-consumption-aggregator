using System;
using System.Threading.Tasks;
using MECA.ConsoleApp.Constants;

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
            var fileLocation = await _fileService.LocateFile(FolderConstants.InputFolder);

            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                throw new InvalidOperationException(ErrorMessageConstants.MissingFileMessage);
            }

            var consumptionData = await _fileService.ReadFile(fileLocation);

            if (consumptionData == null)
            {
                throw new InvalidOperationException(ErrorMessageConstants.NoDataToProcessMessage);
            }
            
            var aggregatedData = await _monthlyAggregatorService.Aggregate(consumptionData);

            await _fileService.WriteToFile(aggregatedData, FolderConstants.OutputFolder);
        }
    }
}