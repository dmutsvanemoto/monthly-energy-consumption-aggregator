using System.Threading.Tasks;

namespace MECA.ConsoleApp.Services
{
    public interface IFileLoaderService
    {
        Task<string> LocateFile(string folderName);
    }
}