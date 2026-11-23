namespace Programa2.ContadorLOC.Dominio;

/// <summary>
/// Contiene la colección consolidada de partes analizadas y los totales acumulados del programa.
/// </summary>
public sealed class ResumenConteo
{
    private readonly List<ParteInfo> _partes = [];

    /// <summary>
    /// Lista de solo lectura con las partes identificadas.
    /// </summary>
    public IReadOnlyList<ParteInfo> Partes => _partes.AsReadOnly();

    /// <summary>
    /// Total de líneas de código lógicas (LOC) de todo el programa analizado.
    /// </summary>
    public int TotalLoc => _partes.Sum(p => p.TamanoLoc);

    /// <summary>
    /// Total de ítems (métodos) de todas las partes del programa.
    /// </summary>
    public int TotalItems => _partes.Sum(p => p.CantidadItems);

    /// <summary>
    /// Agrega una nueva parte al resumen o fusiona sus datos si ya existe una con el mismo nombre.
    /// </summary>
    /// <param name="parte">Información de la parte a registrar.</param>
    public void AgregarParte(ParteInfo parte)
    {
        ArgumentNullException.ThrowIfNull(parte);

        var existente = _partes.FirstOrDefault(p => string.Equals(p.Nombre, parte.Nombre, StringComparison.OrdinalIgnoreCase));
        if (existente is not null)
        {
            existente.AgregarLoc(parte.TamanoLoc);
            for (int i = 0; i < parte.CantidadItems; i++)
            {
                existente.IncrementarItems();
            }
        }
        else
        {
            _partes.Add(parte);
        }
    }
}
