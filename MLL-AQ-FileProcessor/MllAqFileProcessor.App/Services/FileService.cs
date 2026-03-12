using System.IO;
using MllAqFileProcessor.App.Interfaces;

namespace MllAqFileProcessor.App.Services
{
    public class FileService : IFileService
    {
        public bool Exists(string path) => File.Exists(path);

        public StreamReader OpenReader(string path) => new StreamReader(path);

        public StreamWriter OpenWriter(string path) => new StreamWriter(path);

        public void EnsureDirectoryExists(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}