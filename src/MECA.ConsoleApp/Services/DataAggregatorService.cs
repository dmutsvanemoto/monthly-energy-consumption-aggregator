using System;
using System.Threading.Tasks;

namespace MECA.ConsoleApp.Services
{
    public class DataAggregatorService
    {
        private readonly IFileLoaderService _fileLoaderService;

        public DataAggregatorService(IFileLoaderService fileLoaderService)
        {
            _fileLoaderService = fileLoaderService;
        }

        public async Task Aggregate()
        {
            var fileLocation = await _fileLoaderService.LocateFile(Constants.IncomingFolder);

            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                throw new InvalidOperationException(Constants.MissingFileMessage);
            }
        }
    }
}