using Xunit;
using MllAqFileProcessor.App.Services;

namespace MllAqFileProcessor.Tests
{
    /// <summary>
    /// Clase de pruebas unitarias para el servicio TextProcessorService.
    /// Verifica el correcto funcionamiento del procesamiento de texto y conteo de reemplazos.
    /// </summary>
    public class TextProcessorTests
    {
        /// <summary>
        /// Prueba que el método ProcessLine realice reemplazos correctos y cuente las ocurrencias.
        /// Utiliza datos de prueba para diferentes escenarios: reemplazos simples, múltiples, sin coincidencias y sensibles a mayúsculas.
        /// </summary>
        /// <param name="input">Línea de entrada a procesar.</param>
        /// <param name="search">Texto a buscar.</param>
        /// <param name="replace">Texto de reemplazo.</param>
        /// <param name="expectedLine">Línea esperada después del procesamiento.</param>
        /// <param name="expectedCount">Número esperado de reemplazos.</param>
        [Theory]
        [InlineData("Hola mundo", "mundo", "C#", "Hola C#", 1)]
        [InlineData("auraportal es auraportal", "auraportal", "ap", "ap es ap", 2)]
        [InlineData("No hay coincidencias", "test", "reemplazo", "No hay coincidencias", 0)]
        [InlineData("Caso sensible a MAYUSCULAS", "mayusculas", "X", "Caso sensible a MAYUSCULAS", 0)]
        public void ProcessLine_ShouldReplaceAndCountCorrectly(
            string input, string search, string replace, string expectedLine, int expectedCount)
        {
            // Arrange
            var service = new TextProcessorService();

            // Act
            var result = service.ProcessLine(input, search, replace);

            // Assert
            Assert.Equal(expectedLine, result.ProcessedLine);
            Assert.Equal(expectedCount, result.ReplacementCount);
        }
    }
}