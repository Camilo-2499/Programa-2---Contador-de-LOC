using System.Text;

namespace Programa2.ContadorLOC.Infraestructura;

/// <summary>
/// Provee operaciones para buscar y leer archivos de código fuente (.cs) en disco.
/// </summary>
public sealed class LectorCodigoFuente
{
    private const string ExtensionCs = ".cs";

    /// <summary>
    /// Obtiene la lista de rutas completas a analizar a partir de un archivo o directorio.
    /// </summary>
    /// <param name="ruta">Ruta de archivo .cs o carpeta.</param>
    /// <returns>Lista de rutas de archivos encontrados.</returns>
    /// <exception cref="ArchivoNoValidoException">Se lanza si la ruta no existe o no tiene archivos .cs.</exception>
    public IReadOnlyList<string> ObtenerArchivos(string ruta)
    {
        if (string.IsNullOrWhiteSpace(ruta))
        {
            throw new ArchivoNoValidoException("Debe proporcionar una ruta válida de archivo o carpeta.");
        }

        string rutaLimpia = ruta.Trim('\"', '\'');

        if (File.Exists(rutaLimpia))
        {
            if (!rutaLimpia.EndsWith(ExtensionCs, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArchivoNoValidoException($"El archivo «{Path.GetFileName(rutaLimpia)}» no es un archivo de código C# (.cs).");
            }
            return [Path.GetFullPath(rutaLimpia)];
        }

        if (Directory.Exists(rutaLimpia))
        {
            var archivos = Directory.GetFiles(rutaLimpia, $"*{ExtensionCs}", SearchOption.AllDirectories)
                .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                            !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}") &&
                            !f.Contains($"{Path.DirectorySeparatorChar}.vs{Path.DirectorySeparatorChar}"))
                .OrderBy(f => f)
                .Select(Path.GetFullPath)
                .ToList();

            if (archivos.Count == 0)
            {
                throw new ArchivoNoValidoException($"El directorio «{rutaLimpia}» no contiene archivos .cs para analizar.");
            }

            return archivos;
        }

        throw new ArchivoNoValidoException($"No se encontró el archivo o carpeta «{rutaLimpia}». Verifique la ruta.");
    }

    /// <summary>
    /// Lee todas las líneas de un archivo de texto con codificación UTF-8.
    /// </summary>
    /// <param name="rutaArchivo">Ruta completa del archivo.</param>
    /// <returns>Arreglo con las líneas de texto.</returns>
    public string[] LeerLineas(string rutaArchivo)
    {
        try
        {
            return File.ReadAllLines(rutaArchivo, Encoding.UTF8);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new ArchivoNoValidoException($"Error al leer «{Path.GetFileName(rutaArchivo)}»: {ex.Message}", ex);
        }
    }
}
