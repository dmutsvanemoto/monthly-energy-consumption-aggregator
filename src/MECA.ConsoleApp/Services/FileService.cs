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

        public async Task WriteToFile(Dictionary<string, int> aggregatedData, string folderName)
        {
            if (aggregatedData == null || !aggregatedData.Keys.Any())
            {
                throw new ArgumentNullException(Constants.AggregatedDataIsRequired);
            }

            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            if (!Directory.Exists(folderName))
            {
                throw new InvalidOperationException(Constants.OutputFolderDoesNotExist);
            }

            // TODO: Create method to generate rows using aggregatedData
            var rows = new List<string> { "Month Year, Consumption" };
            foreach (var (key, value) in aggregatedData)
            {
                var row = $"{key}, {value}";
                rows.Add(row);
            }
            
            var path = @$"{folderName}\consumption-output.csv";

            // TODO: Create method for deleting file if it exists
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            await File.WriteAllLinesAsync(path, rows);
        }
    }
}