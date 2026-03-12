using System;
using MllAqFileProcessor.App.Interfaces;

namespace MllAqFileProcessor.App
{
    public class FileProcessorEngine
    {
        private readonly IFileService _fileService;
        private readonly ITextProcessorService _textProcessor;

        public FileProcessorEngine(IFileService fileService, ITextProcessorService textProcessor)
        {
            _fileService = fileService;
            _textProcessor = textProcessor;
        }

        public int Run(string sourcePath, string targetPath, string searchText, string replaceText)
        {
            // Validaciones de negocio
            if (!_fileService.Exists(sourcePath))
                throw new FileNotFoundException($"El fichero origen no existe: {sourcePath}");

            if (string.Equals(Path.GetFullPath(sourcePath), Path.GetFullPath(targetPath), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("El fichero de destino no puede ser igual al de origen.");

            _fileService.EnsureDirectoryExists(targetPath);

            int totalReplacements = 0;

            // Procesamiento por flujo (Stream) para eficiencia de memoria
            using (var reader = _fileService.OpenReader(sourcePath))
            using (var writer = _fileService.OpenWriter(targetPath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var result = _textProcessor.ProcessLine(line, searchText, replaceText);
                    writer.WriteLine(result.ProcessedLine);
                    totalReplacements += result.ReplacementCount;
                }
            }

            return totalReplacements;
        }
    }
}