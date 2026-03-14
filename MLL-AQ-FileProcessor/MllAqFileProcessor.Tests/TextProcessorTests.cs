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
        [InlineData("Hola mundo", "mundo", "AuraQuantic", "Hola AuraQuantic", 1)]
        [InlineData("auraportal es auraportal", "auraportal", "ap", "ap es ap", 2)]
        [InlineData("No hay coincidencias", "C#", "Java", "No hay coincidencias", 0)]
        [InlineData("Caso sensible a MAYUSCULAS", "mayusculas", "X", "Caso sensible a MAYUSCULAS", 0)]
        public void ProcessLine_ShouldReplaceAndCountCorrectly(
            string input, string search, string replace, string expectedLine, int expectedCount)
        {
            // Preparar
            var service = new TextProcessorService();

            // Actuar
            var result = service.ProcessLine(input, search, replace);

            // Afirmar
            Assert.Equal(expectedLine, result.ProcessedLine);
            Assert.Equal(expectedCount, result.ReplacementCount);
        }

        /// <summary>
        /// Prueba que ProcessLine lance ArgumentNullException si la línea de entrada es null.
        /// </summary>
        [Fact]
        public void ProcessLine_ShouldThrowArgumentNullException_WhenLineIsNull()
        {
            // Preparar
            var service = new TextProcessorService();

            // Actuar y Afirmar
            Assert.Throws<ArgumentNullException>(() => service.ProcessLine(null, "buscar", "reemplazar"));
        }
    }
}