namespace MllAqFileProcessor.App.Interfaces
{
    /// <summary>
    /// Define el contrato para el procesamiento de texto.
    /// Cumple con el Principio de Responsabilidad Única (SRP).
    /// </summary>
    public interface ITextProcessorService
    {
        /// <summary>
        /// Procesa una línea de texto realizando los reemplazos.
        /// </summary>
        /// <param name="line">Línea de texto a procesar.</param>
        /// <param name="searchText">Cadena a buscar.</param>
        /// <param name="replaceText">Cadena de reemplazo.</param>
        /// <returns>Un record con la línea procesada y el total de reemplazos en ella.</returns>
        ProcessResult ProcessLine(string line, string searchText, string replaceText);
    }

    /// <summary>
    /// Estructura de datos inmutable para el resultado del procesamiento.
    /// </summary>
    public record ProcessResult(string ProcessedLine, int ReplacementCount);
}