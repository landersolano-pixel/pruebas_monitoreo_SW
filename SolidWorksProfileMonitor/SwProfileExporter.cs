// SwProfileExporter.cs
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

public static class SwProfileExporter
{
    // Importa la función nativa RegSaveKeyEx desde la librería advapi32.dll de Windows
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int RegSaveKeyEx(
        IntPtr hKey,
        string lpFile,
        IntPtr lpSecurityAttributes,
        uint Flags
    );

    /// <summary>
    /// Genera un archivo .sldreg a partir del perfil del usuario actual de SolidWorks.
    /// </summary>
    /// <param name="solidworksVersion">La versión de SolidWorks (ej. "2025").</param>
    /// <param name="destinationSldregPath">La ruta completa donde se guardará el archivo .sldreg.</param>
    public static void GenerateSldregFromCurrentUser(string solidworksVersion, string destinationSldregPath)
    {
        // Define la ruta de la clave del registro que se va a exportar
        string registryKeyPath = $@"Software\SOLIDWORKS\SOLIDWORKS {solidworksVersion}";

        try
        {
            // Abre la clave del registro de forma segura (el 'using' asegura que se cierre)
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyPath, false))
            {
                if (key == null)
                {
                    throw new InvalidOperationException($"La clave del registro de SolidWorks no fue encontrada para la versión {solidworksVersion}.");
                }

                // Obtiene el puntero (handle) nativo que la API de Windows necesita
                IntPtr hKey = key.Handle.DangerousGetHandle();

                const uint REG_LATEST_FORMAT = 0x00000010; // Usar el formato de archivo de registro más reciente

                // Llama a la función de la API de Windows para guardar la clave en un archivo
                int result = RegSaveKeyEx(hKey, destinationSldregPath, IntPtr.Zero, REG_LATEST_FORMAT);

                // Si el resultado no es 0, hubo un error. Lo convertimos en una excepción legible.
                if (result != 0)
                {
                    throw new Win32Exception(result);
                }
            }
        }
        catch (Exception ex)
        {
            // Propaga una excepción más descriptiva para facilitar el debugging
            throw new Exception($"Error al generar el archivo .sldreg en '{destinationSldregPath}'. Detalles: {ex.Message}", ex);
        }
    }
}