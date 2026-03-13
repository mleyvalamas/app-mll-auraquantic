using System.IO;
using MllAqFileProcessor.App.Interfaces;

namespace MllAqFileProcessor.App.Services
{
    /// <summary>
    /// Servicio que proporciona operaciones básicas de archivo, como verificación de existencia,
    /// apertura de lectores y escritores de stream, y creación de directorios.
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// Verifica si un archivo existe en la ruta especificada.
        /// </summary>
        /// <param name="path">Ruta del archivo a verificar.</param>
        /// <returns>True si el archivo existe, false en caso contrario.</returns>
        public bool Exists(string path) => File.Exists(path);

        /// <summary>
        /// Abre un lector de stream para el archivo en la ruta especificada.
        /// </summary>
        /// <param name="path">Ruta del archivo a leer.</param>
        /// <returns>Un StreamReader para el archivo.</returns>
        public StreamReader OpenReader(string path) => new StreamReader(path);

        /// <summary>
        /// Abre un escritor de stream para el archivo en la ruta especificada.
        /// </summary>
        /// <param name="path">Ruta del archivo a escribir.</param>
        /// <returns>Un StreamWriter para el archivo.</returns>
        public StreamWriter OpenWriter(string path) => new StreamWriter(path);

        /// <summary>
        /// Asegura que el directorio del archivo especificado exista, creándolo si es necesario.
        /// </summary>
        /// <param name="path">Ruta del archivo cuyo directorio se debe verificar o crear.</param>
        public void EnsureDirectoryExists(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}