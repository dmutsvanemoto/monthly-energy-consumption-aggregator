using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MECA.ConsoleApp.Models;

namespace MECA.ConsoleApp.Services
{
    public class FileService : IFileService
    {
        public ValueTask<string> LocateFile(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            var files = Directory.GetFiles(folderName);

            var filePath = files.FirstOrDefault(x => x.EndsWith("consumption-data.csv"));

            return new ValueTask<string>(filePath);
        }

        public Task<IList<ConsumptionData>> ReadFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task WriteToFile(Dictionary<string, int> aggregatedData)
        {
            throw new NotImplementedException();
        }
    }
}