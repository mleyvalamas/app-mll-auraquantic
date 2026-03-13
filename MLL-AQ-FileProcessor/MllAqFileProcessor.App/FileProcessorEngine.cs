using System;
using MllAqFileProcessor.App.Interfaces;

namespace MllAqFileProcessor.App
{
    /// <summary>
    /// Motor principal para el procesamiento de archivos. Esta clase coordina la lectura, procesamiento y escritura de archivos,
    /// permitiendo reemplazar texto en un archivo origen y guardar el resultado en un archivo destino.
    /// </summary>
    public class FileProcessorEngine
    {
        private readonly IFileService _fileService;
        private readonly ITextProcessorService _textProcessor;

        /// <summary>
        /// Constructor de la clase FileProcessorEngine. Inyecta las dependencias necesarias para el procesamiento de archivos.
        /// </summary>
        /// <param name="fileService">Servicio para operaciones de archivos (lectura, escritura, verificación de existencia).</param>
        /// <param name="textProcessor">Servicio para procesamiento de texto (reemplazo de cadenas).</param>
        public FileProcessorEngine(IFileService fileService, ITextProcessorService textProcessor)
        {
            _fileService = fileService;
            _textProcessor = textProcessor;
        }

        /// <summary>
        /// Ejecuta el procesamiento del archivo. Lee el archivo origen línea por línea, reemplaza el texto especificado
        /// y escribe el resultado en el archivo destino. Devuelve el número total de reemplazos realizados.
        /// </summary>
        /// <param name="sourcePath">Ruta del archivo origen.</param>
        /// <param name="targetPath">Ruta del archivo destino.</param>
        /// <param name="searchText">Texto a buscar para reemplazar.</param>
        /// <param name="replaceText">Texto con el que reemplazar.</param>
        /// <returns>Número total de reemplazos realizados en el archivo.</returns>
        /// <exception cref="FileNotFoundException">Se lanza si el archivo origen no existe.</exception>
        /// <exception cref="InvalidOperationException">Se lanza si el archivo destino es el mismo que el origen.</exception>
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