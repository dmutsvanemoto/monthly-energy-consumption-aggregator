using System.Collections.Generic;
using System.Threading.Tasks;
using MECA.ConsoleApp.Models;

namespace MECA.ConsoleApp.Services
{
    public interface IFileService
    {
        ValueTask<string> LocateFile(string folderName);
        Task<IList<ConsumptionData>> ReadFile(string filePath);
        Task WriteToFile(Dictionary<string, int> aggregatedData);
    }
}