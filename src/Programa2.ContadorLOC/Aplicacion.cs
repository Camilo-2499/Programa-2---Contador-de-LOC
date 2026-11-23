using Programa2.ContadorLOC.Dominio;
using Programa2.ContadorLOC.Infraestructura;
using Programa2.ContadorLOC.Presentacion;

namespace Programa2.ContadorLOC;

/// <summary>
/// Orquestador del flujo de la aplicación: lectura, análisis y presentación de métricas.
/// </summary>
public static class Aplicacion
{
    /// <summary>
    /// Ejecuta el flujo principal del contador de LOC.
    /// </summary>
    /// <param name="args">Argumentos de línea de comandos.</param>
    /// <param name="entrada">Lector de entrada.</param>
    /// <param name="salida">Escritor de salida.</param>
    /// <returns>0 si la ejecución fue exitosa; 1 si ocurrió un error controlado.</returns>
    public static int Ejecutar(string[] args, TextReader entrada, TextWriter salida)
    {
        var ui = new ConsolaUI(entrada, salida);
        ui.MostrarEncabezado();

        string ruta = args.Length > 0 ? args[0] : ui.PedirRuta();

        try
        {
            var archivos = LectorCodigoFuente.ObtenerArchivos(ruta);
            var resumenGlobal = new ResumenConteo();

            foreach (string archivo in archivos)
            {
                string[] lineas = LectorCodigoFuente.LeerLineas(archivo);
                var resumenArchivo = AnalizadorCodigoService.Analizar(Path.GetFileName(archivo), lineas);

                foreach (var parte in resumenArchivo.Partes)
                {
                    resumenGlobal.AgregarParte(parte);
                }
            }

            ui.MostrarResultados(ruta, resumenGlobal);
            return 0;
        }
        catch (ArchivoNoValidoException ex)
        {
            ui.MostrarError(ex.Message);
            return 1;
        }
    }
}
