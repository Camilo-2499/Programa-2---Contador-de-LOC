using Programa2.ContadorLOC.Dominio;

namespace Programa2.ContadorLOC.Presentacion;

/// <summary>
/// Gestiona la interacción con el usuario en la terminal y el formateo de tablas según el estándar SEI.
/// </summary>
public sealed class ConsolaUI
{
    private readonly TextReader _entrada;
    private readonly TextWriter _salida;

    public ConsolaUI(TextReader entrada, TextWriter salida)
    {
        _entrada = entrada ?? throw new ArgumentNullException(nameof(entrada));
        _salida = salida ?? throw new ArgumentNullException(nameof(salida));
    }

    /// <summary>
    /// Imprime el encabezado principal del programa.
    /// </summary>
    public void MostrarEncabezado()
    {
        _salida.WriteLine("================================================================================");
        _salida.WriteLine("         PSP1 · PROGRAMA 2 — CONTADOR DE LÍNEAS DE CÓDIGO (LOC)");
        _salida.WriteLine("================================================================================");
    }

    /// <summary>
    /// Solicita al usuario la ruta a analizar si no se especificó como argumento.
    /// </summary>
    public string PedirRuta()
    {
        _salida.Write("Ingrese la ruta del archivo .cs o carpeta a analizar: ");
        string? linea = _entrada.ReadLine();
        return linea?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Muestra la tabla de resultados estructurada según la Tabla 1 del SEI Assignment Kit.
    /// </summary>
    public void MostrarResultados(string ruta, ResumenConteo resumen)
    {
        ArgumentNullException.ThrowIfNull(resumen);

        _salida.WriteLine();
        _salida.WriteLine($"Ruta analizada: {ruta}");
        _salida.WriteLine();
        _salida.WriteLine("{0,-32} {1,-18} {2,-15} {3,-12}", "Part Name", "Number of Items", "Part Size", "Total Size");
        _salida.WriteLine(new string('-', 80));

        foreach (var parte in resumen.Partes)
        {
            _salida.WriteLine("{0,-32} {1,-18} {2,-15}", parte.Nombre, parte.CantidadItems, parte.TamanoLoc);
        }

        _salida.WriteLine(new string('-', 80));
        _salida.WriteLine("{0,-32} {1,-18} {2,-15} {3,-12}", "TOTAL DEL PROGRAMA", resumen.TotalItems, resumen.TotalLoc, resumen.TotalLoc);
        _salida.WriteLine("================================================================================");
    }

    /// <summary>
    /// Muestra un mensaje de error legible.
    /// </summary>
    public void MostrarError(string mensaje)
    {
        _salida.WriteLine();
        _salida.WriteLine($"ERROR: {mensaje}");
    }
}
