using System.Text.RegularExpressions;
using MllAqFileProcessor.App.Interfaces;

namespace MllAqFileProcessor.App.Services
{
    /// <summary>
    /// Servicio que maneja el procesamiento de texto, incluyendo reemplazos de cadenas
    /// y conteo de ocurrencias en líneas de texto.
    /// </summary>
    public class TextProcessorService : ITextProcessorService
    {
        /// <summary>
        /// Procesa una línea de texto reemplazando todas las ocurrencias del texto de búsqueda
        /// con el texto de reemplazo, y cuenta el número de reemplazos realizados.
        /// </summary>
        /// <param name="line">La línea de texto a procesar.</param>
        /// <param name="searchText">El texto a buscar y reemplazar.</param>
        /// <param name="replaceText">El texto de reemplazo.</param>
        /// <returns>Un objeto ProcessResult con la línea procesada y el conteo de reemplazos.</returns>
        public ProcessResult ProcessLine(string line, string searchText, string replaceText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return new ProcessResult(line, 0);
            }

            // Escapamos el texto de búsqueda por si contiene caracteres especiales de Regex
            string pattern = Regex.Escape(searchText);
            
            // Contamos las ocurrencias antes de reemplazar
            int count = Regex.Matches(line, pattern).Count;
            
            // Realizamos el reemplazo
            string processedLine = line.Replace(searchText, replaceText);

            return new ProcessResult(processedLine, count);
        }
    }
}