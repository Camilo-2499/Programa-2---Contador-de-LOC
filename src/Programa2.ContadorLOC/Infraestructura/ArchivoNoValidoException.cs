namespace Programa2.ContadorLOC.Infraestructura;

/// <summary>
/// Excepción lanzada cuando una ruta de archivo o directorio no es válida, no existe o no contiene código válido.
/// </summary>
public sealed class ArchivoNoValidoException : Exception
{
    public ArchivoNoValidoException(string mensaje) : base(mensaje)
    {
    }

    public ArchivoNoValidoException(string mensaje, Exception innerException) : base(mensaje, innerException)
    {
    }
}
