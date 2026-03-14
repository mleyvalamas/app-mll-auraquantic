using System;
using System.IO;
using NSubstitute;
using Xunit;
using MllAqFileProcessor.App;
using MllAqFileProcessor.App.Interfaces;

namespace MllAqFileProcessor.Tests
{
    /// <summary>
    /// Clase de pruebas unitarias para el motor FileProcessorEngine.
    /// Verifica la coordinación entre servicios de archivo y procesamiento de texto.
    /// </summary>
    public class FileProcessorEngineTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _sourceFile;
        private readonly string _targetFile;
        private readonly IFileService _fileService;
        private readonly ITextProcessorService _textProcessor;
        private readonly FileProcessorEngine _engine;

        public FileProcessorEngineTests()
        {
            // Preparar: Crear mocks y archivos temporales
            _tempDir = Path.Combine(Path.GetTempPath(), "FileProcessorEngineTests");
            _sourceFile = Path.Combine(_tempDir, "origen.txt");
            _targetFile = Path.Combine(_tempDir, "destino.txt");
            Directory.CreateDirectory(_tempDir);

            _fileService = Substitute.For<IFileService>();
            _textProcessor = Substitute.For<ITextProcessorService>();
            _engine = new FileProcessorEngine(_fileService, _textProcessor);
        }

        public void Dispose()
        {
            // Limpiar: Eliminar archivos temporales
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
        }

        /// <summary>
        /// Prueba que Run procese correctamente un archivo y devuelva el conteo total de reemplazos.
        /// </summary>
        [Fact]
        public void Run_ShouldProcessFileAndReturnTotalReplacements_WhenValidInputs()
        {
            // Preparar
            File.WriteAllText(_sourceFile, "línea 1 buscar\nlínea 2");
            var searchText = "buscar";
            var replaceText = "reemplazar";

            _fileService.Exists(_sourceFile).Returns(true);
            _fileService.OpenReader(_sourceFile).Returns(new StreamReader(_sourceFile));
            _fileService.OpenWriter(_targetFile).Returns(new StreamWriter(_targetFile));

            var result1 = new ProcessResult("línea 1 reemplazar", 1);
            var result2 = new ProcessResult("línea 2", 0);
            _textProcessor.ProcessLine("línea 1 buscar", searchText, replaceText).Returns(result1);
            _textProcessor.ProcessLine("línea 2", searchText, replaceText).Returns(result2);

            // Actuar
            var totalReplacements = _engine.Run(_sourceFile, _targetFile, searchText, replaceText);

            // Afirmar
            Assert.Equal(1, totalReplacements);
            _fileService.Received().Exists(_sourceFile);
            _fileService.Received().EnsureDirectoryExists(_targetFile);
            _textProcessor.Received(1).ProcessLine("línea 1 buscar", searchText, replaceText);
            _textProcessor.Received(1).ProcessLine("línea 2", searchText, replaceText);
        }

        /// <summary>
        /// Prueba que Run lance FileNotFoundException si el archivo origen no existe.
        /// </summary>
        [Fact]
        public void Run_ShouldThrowFileNotFoundException_WhenSourceFileDoesNotExist()
        {
            // Preparar
            var nonExistentFile = Path.Combine(_tempDir, "noexiste.txt");
            var searchText = "buscar";
            var replaceText = "reemplazar";

            _fileService.Exists(nonExistentFile).Returns(false);

            // Actuar y Afirmar
            var exception = Assert.Throws<FileNotFoundException>(() =>
                _engine.Run(nonExistentFile, _targetFile, searchText, replaceText));
            Assert.Contains("El fichero origen no existe", exception.Message);
        }

        /// <summary>
        /// Prueba que Run lance InvalidOperationException si origen y destino son el mismo archivo.
        /// </summary>
        [Fact]
        public void Run_ShouldThrowInvalidOperationException_WhenSourceAndTargetAreSame()
        {
            // Preparar
            var searchText = "buscar";
            var replaceText = "reemplazar";

            _fileService.Exists(_sourceFile).Returns(true);

            // Actuar y Afirmar
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _engine.Run(_sourceFile, _sourceFile, searchText, replaceText));
            Assert.Contains("El fichero de destino no puede ser igual al de origen", exception.Message);
        }

        /// <summary>
        /// Prueba que Run llame a EnsureDirectoryExists para crear el directorio del destino si es necesario.
        /// </summary>
        [Fact]
        public void Run_ShouldCallEnsureDirectoryExists_ForTargetPath()
        {
            // Preparar
            File.WriteAllText(_sourceFile, "línea");
            var searchText = "buscar";
            var replaceText = "reemplazar";

            _fileService.Exists(_sourceFile).Returns(true);
            _fileService.OpenReader(_sourceFile).Returns(new StreamReader(_sourceFile));
            _fileService.OpenWriter(_targetFile).Returns(new StreamWriter(_targetFile));
            _textProcessor.ProcessLine("línea", searchText, replaceText)
                .Returns(new ProcessResult("línea", 0));

            // Actuar
            _engine.Run(_sourceFile, _targetFile, searchText, replaceText);

            // Afirmar
            _fileService.Received().EnsureDirectoryExists(_targetFile);
        }
    }
}