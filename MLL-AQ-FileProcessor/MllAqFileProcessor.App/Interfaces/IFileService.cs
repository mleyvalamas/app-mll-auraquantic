using System.IO;

namespace MllAqFileProcessor.App.Interfaces
{
    public interface IFileService
    {
        bool Exists(string path);
        StreamReader OpenReader(string path);
        StreamWriter OpenWriter(string path);
        void EnsureDirectoryExists(string path);
    }
}