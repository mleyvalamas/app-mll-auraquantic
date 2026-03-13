using System.IO;

namespace MllAqFileProcessor.App.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones básicas para el manejo de archivos,
    /// incluyendo verificación de existencia, apertura de streams y creación de directorios.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Verifica si un archivo existe en la ruta especificada.
        /// </summary>
        /// <param name="path">Ruta del archivo a verificar.</param>
        /// <returns>True si el archivo existe, false en caso contrario.</returns>
        bool Exists(string path);

        /// <summary>
        /// Abre un lector de stream para el archivo en la ruta especificada.
        /// </summary>
        /// <param name="path">Ruta del archivo a leer.</param>
        /// <returns>Un StreamReader para el archivo.</returns>
        StreamReader OpenReader(string path);

        /// <summary>
        /// Abre un escritor de stream para el archivo en la ruta especificada.
        /// </summary>
        /// <param name="path">Ruta del archivo a escribir.</param>
        /// <returns>Un StreamWriter para el archivo.</returns>
        StreamWriter OpenWriter(string path);

        /// <summary>
        /// Asegura que el directorio del archivo especificado exista, creándolo si es necesario.
        /// </summary>
        /// <param name="path">Ruta del archivo cuyo directorio se debe verificar o crear.</param>
        void EnsureDirectoryExists(string path);
    }
}