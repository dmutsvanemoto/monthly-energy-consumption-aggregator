using System.Collections.Generic;
using System.Threading.Tasks;
using MECA.ConsoleApp.Models;

namespace MECA.ConsoleApp.Services
{
    public interface IFileLoaderService
    {
        Task<string> LocateFile(string folderName);
        Task<IEnumerable<ConsumptionData>> ReadFile(string filePath);
    }
}