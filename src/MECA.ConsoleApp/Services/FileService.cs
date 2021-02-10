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

        public async Task<IList<ConsumptionData>> ReadFile(string filePath)
        {
            var results = new List<ConsumptionData>();
            
            using TextReader file = new StreamReader(Path.GetFullPath(filePath));

            var row = await file.ReadLineAsync();

            while (row != null)
            {
                if (row.StartsWith("date"))
                {
                    row = await file.ReadLineAsync();
                    continue;
                }

                var columns = row.Split(",");

                var consumption = new ConsumptionData
                {
                    Date = DateTime.Parse(columns[0]),
                    Consumption = int.Parse(columns[1])
                };

                results.Add(consumption);

                row = await file.ReadLineAsync();
            }
            
            return results;
        }

        public Task WriteToFile(Dictionary<string, int> aggregatedData)
        {
            throw new NotImplementedException();
        }
    }
}