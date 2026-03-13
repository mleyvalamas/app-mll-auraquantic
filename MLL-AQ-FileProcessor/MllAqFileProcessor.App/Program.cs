using System;
using System.IO;
using MllAqFileProcessor.App.Services;

namespace MllAqFileProcessor.App
{
    /// <summary>
    /// Clase principal del programa. Contiene el punto de entrada de la aplicación de procesamiento de archivos.
    /// Esta aplicación toma argumentos de línea de comandos para procesar un archivo, reemplazando texto especificado.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Método principal de la aplicación. Valida los argumentos de línea de comandos, configura las dependencias
        /// y ejecuta el procesamiento del archivo. Maneja excepciones comunes y devuelve códigos de salida apropiados.
        /// </summary>
        /// <param name="args">Argumentos de línea de comandos: ruta_origen, ruta_destino, texto_buscar, texto_reemplazo.</param>
        /// <returns>0 si el procesamiento es exitoso, 1 en caso de error.</returns>
        static int Main(string[] args)
        {
            // 1. Validación de número de parámetros
            if (args.Length != 4)
            {
                Console.WriteLine("Error: Número de parámetros insuficiente.");
                Console.WriteLine("Uso: MllAqFileProcessor.App <ruta_origen> <ruta_destino> <texto_buscar> <texto_reemplazo>");
                return 1;
            }

            string sourcePath = args[0];
            string targetPath = args[1];
            string searchText = args[2];
            string replaceText = args[3];

            try
            {
                // Inyección manual de dependencias (para un proyecto de esta escala es lo más limpio)
                var fileService = new FileService();
                var textProcessor = new TextProcessorService();
                var engine = new FileProcessorEngine(fileService, textProcessor);

                // 2. Ejecución del proceso
                int totalReplacements = engine.Run(sourcePath, targetPath, searchText, replaceText);

                // 3. Resultado satisfactorio
                Console.WriteLine($"Se han hecho {totalReplacements} reemplazos");
                return 0;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: No tiene permisos suficientes para acceder a los archivos o carpetas.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error de E/S: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }

            return 1;
        }
    }
}