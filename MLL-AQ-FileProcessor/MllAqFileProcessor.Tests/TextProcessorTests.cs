using Xunit;
using MllAqFileProcessor.App.Services;

namespace MllAqFileProcessor.Tests
{
    public class TextProcessorTests
    {
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