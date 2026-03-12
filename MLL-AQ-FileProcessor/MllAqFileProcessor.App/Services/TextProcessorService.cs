using System.Text.RegularExpressions;
using MllAqFileProcessor.App.Interfaces;

namespace MllAqFileProcessor.App.Services
{
    public class TextProcessorService : ITextProcessorService
    {
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