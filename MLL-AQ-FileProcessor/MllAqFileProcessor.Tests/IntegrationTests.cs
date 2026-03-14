using System;
using System.IO;
using Xunit;
using MllAqFileProcessor.App;

namespace MllAqFileProcessor.Tests
{
    /// <summary>
    /// Clase de pruebas de integración para verificar el flujo completo de la aplicación.
    /// Prueba la interacción entre todos los componentes desde la entrada de línea de comandos hasta la salida.
    /// </summary>
    public class IntegrationTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _sourceFile;
        private readonly string _targetFile;

        public IntegrationTests()
        {
            // Preparar: Crear directorio y archivos temporales
            _tempDir = Path.Combine(Path.GetTempPath(), "IntegrationTests");
            _sourceFile = Path.Combine(_tempDir, "origen.txt");
            _targetFile = Path.Combine(_tempDir, "destino.txt");
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            // Limpiar: Eliminar archivos temporales
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
        }

        /// <summary>
        /// Prueba de integración completa: procesa un archivo con reemplazos y verifica el resultado.
        /// </summary>
        [Fact]
        public void FullIntegration_ShouldProcessFileAndOutputCorrectly()
        {
            // Preparar: Crear archivo origen con contenido
            File.WriteAllText(_sourceFile, "Hola mundo\nEste es un test\nmundo mundo");

            // Capturar output de consola
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Actuar: Ejecutar Main con argumentos válidos
            var args = new[] { _sourceFile, _targetFile, "mundo", "universo" };
            var exitCode = Program.Main(args);

            // Afirmar: Verificar código de salida
            Assert.Equal(0, exitCode);

            // Verificar output de consola
            var output = sw.ToString();
            Assert.Contains("Se han hecho 3 reemplazos", output);

            // Verificar contenido del archivo destino
            Assert.True(File.Exists(_targetFile));
            var targetContent = File.ReadAllText(_targetFile);
            var expectedContent = "Hola universo\nEste es un test\nuniverso universo\n";
            Assert.Equal(expectedContent, targetContent);
        }

        /// <summary>
        /// Prueba de integración con error: archivo origen inexistente.
        /// </summary>
        [Fact]
        public void FullIntegration_ShouldHandleFileNotFoundError()
        {
            // Preparar: Archivo origen no existe
            var nonExistentFile = Path.Combine(_tempDir, "noexiste.txt");

            // Capturar output de consola
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Actuar: Ejecutar Main con archivo inexistente
            var args = new[] { nonExistentFile, _targetFile, "test", "reemplazo" };
            var exitCode = Program.Main(args);

            // Afirmar: Verificar código de salida de error
            Assert.Equal(1, exitCode);

            // Verificar output de error
            var output = sw.ToString();
            Assert.Contains("Error:", output);
        }

        /// <summary>
        /// Prueba de integración con error: número insuficiente de argumentos.
        /// </summary>
        [Fact]
        public void FullIntegration_ShouldHandleInsufficientArguments()
        {
            // Capturar output de consola
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Actuar: Ejecutar Main con pocos argumentos
            var args = new[] { _sourceFile, _targetFile }; // Solo 2, necesita 4
            var exitCode = Program.Main(args);

            // Afirmar: Verificar código de salida de error
            Assert.Equal(1, exitCode);

            // Verificar output de error
            var output = sw.ToString();
            Assert.Contains("Número de parámetros insuficiente", output);
            Assert.Contains("Uso:", output);
        }

        /// <summary>
        /// Prueba de integración: origen y destino son el mismo archivo (debe fallar).
        /// </summary>
        [Fact]
        public void FullIntegration_ShouldHandleSameSourceAndTarget()
        {
            // Preparar: Crear archivo
            File.WriteAllText(_sourceFile, "contenido");

            // Capturar output de consola
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Actuar: Ejecutar Main con mismo origen y destino
            var args = new[] { _sourceFile, _sourceFile, "test", "reemplazo" };
            var exitCode = Program.Main(args);

            // Afirmar: Verificar código de salida de error
            Assert.Equal(1, exitCode);

            // Verificar output de error
            var output = sw.ToString();
            Assert.Contains("Error inesperado:", output);
        }
    }
}