using System.IO;
using Xunit;
using MllAqFileProcessor.App.Services;

namespace MllAqFileProcessor.Tests
{
    /// <summary>
    /// Clase de pruebas unitarias para el servicio FileService.
    /// Verifica el correcto funcionamiento de las operaciones básicas de archivos.
    /// </summary>
    public class FileServiceTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _tempFile;

        public FileServiceTests()
        {
            // Preparar: Crear directorio temporal y archivo para pruebas
            _tempDir = Path.Combine(Path.GetTempPath(), "FileServiceTests");
            _tempFile = Path.Combine(_tempDir, "test.txt");
            Directory.CreateDirectory(_tempDir);
            File.WriteAllText(_tempFile, "contenido de prueba");
        }

        public void Dispose()
        {
            // Limpiar: Eliminar archivos y directorios temporales
            if (File.Exists(_tempFile)) File.Delete(_tempFile);
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
        }

        /// <summary>
        /// Prueba que Exists devuelva true para un archivo que existe.
        /// </summary>
        [Fact]
        public void Exists_ShouldReturnTrue_WhenFileExists()
        {
            // Preparar
            var service = new FileService();

            // Actuar
            var result = service.Exists(_tempFile);

            // Afirmar
            Assert.True(result);
        }

        /// <summary>
        /// Prueba que Exists devuelva false para un archivo que no existe.
        /// </summary>
        [Fact]
        public void Exists_ShouldReturnFalse_WhenFileDoesNotExist()
        {
            // Preparar
            var service = new FileService();
            var nonExistentFile = Path.Combine(_tempDir, "noexiste.txt");

            // Actuar
            var result = service.Exists(nonExistentFile);

            // Afirmar
            Assert.False(result);
        }

        /// <summary>
        /// Prueba que OpenReader abra correctamente un archivo existente.
        /// </summary>
        [Fact]
        public void OpenReader_ShouldOpenFile_WhenFileExists()
        {
            // Preparar
            var service = new FileService();

            // Actuar
            using var reader = service.OpenReader(_tempFile);

            // Afirmar
            Assert.NotNull(reader);
            var content = reader.ReadToEnd();
            Assert.Equal("contenido de prueba", content);
        }

        /// <summary>
        /// Prueba que OpenReader lance FileNotFoundException para un archivo inexistente.
        /// </summary>
        [Fact]
        public void OpenReader_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Preparar
            var service = new FileService();
            var nonExistentFile = Path.Combine(_tempDir, "noexiste.txt");

            // Actuar y Afirmar
            Assert.Throws<FileNotFoundException>(() => service.OpenReader(nonExistentFile));
        }

        /// <summary>
        /// Prueba que OpenWriter cree y abra un archivo para escritura.
        /// </summary>
        [Fact]
        public void OpenWriter_ShouldCreateAndOpenFile_ForWriting()
        {
            // Preparar
            var service = new FileService();
            var newFile = Path.Combine(_tempDir, "nuevo.txt");

            // Actuar
            using var writer = service.OpenWriter(newFile);
            writer.Write("nuevo contenido");
            writer.Flush();

            // Afirmar
            Assert.True(File.Exists(newFile));
            var content = File.ReadAllText(newFile);
            Assert.Equal("nuevo contenido", content);
        }

        /// <summary>
        /// Prueba que EnsureDirectoryExists cree el directorio si no existe.
        /// </summary>
        [Fact]
        public void EnsureDirectoryExists_ShouldCreateDirectory_WhenDoesNotExist()
        {
            // Preparar
            var service = new FileService();
            var newDir = Path.Combine(_tempDir, "subdir");
            var fileInNewDir = Path.Combine(newDir, "archivo.txt");

            // Actuar
            service.EnsureDirectoryExists(fileInNewDir);

            // Afirmar
            Assert.True(Directory.Exists(newDir));
        }

        /// <summary>
        /// Prueba que EnsureDirectoryExists no haga nada si el directorio ya existe.
        /// </summary>
        [Fact]
        public void EnsureDirectoryExists_ShouldDoNothing_WhenDirectoryExists()
        {
            // Preparar
            var service = new FileService();
            var existingDir = Path.Combine(_tempDir, "existing");
            Directory.CreateDirectory(existingDir);
            var fileInDir = Path.Combine(existingDir, "archivo.txt");

            // Actuar
            service.EnsureDirectoryExists(fileInDir);

            // Afirmar
            Assert.True(Directory.Exists(existingDir));
        }
    }
}