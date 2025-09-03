// HashingTool.cs
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class HashingTool
{
    /// <summary>
    /// Calcula el hash SHA-256 de un archivo y lo devuelve como un string hexadecimal.
    /// </summary>
    /// <param name="filePath">La ruta al archivo del cual se calculará el hash.</param>
    /// <returns>Una cadena de 64 caracteres que es la huella digital del archivo.</returns>
    public static string CalculateFileHash(string filePath)
    {
        using (var sha256 = SHA256.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hashBytes = sha256.ComputeHash(stream);

                // Convertir el array de bytes a un string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}