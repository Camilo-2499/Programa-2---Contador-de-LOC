namespace Programa2.ContadorLOC.Dominio;

/// <summary>
/// Representa una parte lógica del programa (clase, estructura, interfaz o módulo)
/// y acumula su tamaño en LOC y la cantidad de ítems (métodos/constructores).
/// </summary>
public sealed class ParteInfo
{
    /// <summary>
    /// Nombre de la parte o clase.
    /// </summary>
    public string Nombre { get; }

    /// <summary>
    /// Cantidad de métodos o funciones miembro (ítems).
    /// </summary>
    public int CantidadItems { get; private set; }

    /// <summary>
    /// Tamaño efectivo en líneas de código (LOC).
    /// </summary>
    public int TamanoLoc { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ParteInfo"/>.
    /// </summary>
    /// <param name="nombre">Nombre de la clase o parte.</param>
    public ParteInfo(string nombre)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nombre);
        Nombre = nombre;
        CantidadItems = 0;
        TamanoLoc = 0;
    }

    /// <summary>
    /// Incrementa en uno el conteo de ítems (métodos).
    /// </summary>
    public void IncrementarItems()
    {
        CantidadItems++;
    }

    /// <summary>
    /// Incrementa en uno el conteo de líneas lógicas de código (LOC).
    /// </summary>
    public void IncrementarLoc()
    {
        TamanoLoc++;
    }

    /// <summary>
    /// Agrega una cantidad específica de líneas de código a la parte.
    /// </summary>
    /// <param name="lineas">Cantidad de líneas a sumar.</param>
    public void AgregarLoc(int lineas)
    {
        if (lineas > 0)
        {
            TamanoLoc += lineas;
        }
    }
}
