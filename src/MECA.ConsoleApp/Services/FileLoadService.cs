using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MECA.ConsoleApp.Models;

namespace MECA.ConsoleApp.Services
{
    public class FileLoadService : IFileLoaderService
    {
        public Task<string> LocateFile(string folderName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConsumptionData>> ReadFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}