// PSP1 · Programa 2 — Contador de Líneas de Código (LOC), Partes e Ítems.
// Uso: dotnet run --project src/Programa2.ContadorLOC -- <ruta_archivo_o_carpeta>
using System.Text;
using Programa2.ContadorLOC;

Console.OutputEncoding = Encoding.UTF8;

return Aplicacion.Ejecutar(args, Console.In, Console.Out);
