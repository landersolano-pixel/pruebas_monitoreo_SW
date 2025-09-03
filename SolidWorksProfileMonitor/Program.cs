// Program.cs
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // --- FASE 1: PREPARACIÓN (Se necesita el hash maestro) ---
        // Antes de ejecutar esto, asegúrate de que el archivo 'perfil_estandar.sldreg' exista.
        string standardProfilePath = @"C:\Users\lsr973\Documents\swSettings.sldreg";
        string masterHash;

        try
        {
            Console.WriteLine("Calculando el hash del perfil MAESTRO...");
            masterHash = HashingTool.CalculateFileHash(standardProfilePath);
            Console.WriteLine($"-> Hash Maestro: {masterHash}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: No se pudo procesar el archivo maestro en '{standardProfilePath}'. Asegúrate de que exista.");
            Console.WriteLine(ex.Message);
            Console.ReadKey();
            return;
        }


        // --- FASE 2: MONITOREO (Simulación en la máquina de un usuario) ---
        string solidworksVersion = "2025"; // Esto debería ser dinámico en una app real
        string userProfileTempPath = Path.Combine(Path.GetTempPath(), "sw_user_profile.sldreg");

        try
        {
            // 1. Exportar el perfil actual del usuario a un archivo temporal
            Console.WriteLine("Generando snapshot del perfil del usuario actual...");
            SwProfileExporter.GenerateSldregFromCurrentUser(solidworksVersion, userProfileTempPath);
            Console.WriteLine($"-> Snapshot guardado en: {userProfileTempPath}");

            // 2. Calcular el hash del perfil actual del usuario
            Console.WriteLine("Calculando el hash del perfil del usuario...");
            string userHash = HashingTool.CalculateFileHash(userProfileTempPath);
            Console.WriteLine($"-> Hash del Usuario: {userHash}\n");

            // 3. Comparar y reportar el resultado
            Console.WriteLine("--- RESULTADO DEL MONITOREO ---");
            if (masterHash.Equals(userHash, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Estado del Perfil: ESTÁNDAR");
            }
            else
            {
                Console.WriteLine("Estado del Perfil: PERSONALIZADO");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error durante el monitoreo: {ex.Message}");
        }
        finally
        {
            // 4. Limpiar el archivo temporal
            if (File.Exists(userProfileTempPath))
            {
                File.Delete(userProfileTempPath);
                Console.WriteLine("\nArchivo temporal eliminado.");
            }
            Console.ReadKey();
        }
    }
}