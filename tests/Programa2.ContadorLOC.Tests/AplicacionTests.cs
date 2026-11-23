namespace Programa2.ContadorLOC.Tests;

public sealed class AplicacionTests
{
    [Fact]
    public void CP18_RetornaUnoSiRutaEsInvalida()
    {
        using var salida = new StringWriter();
        using var entrada = new StringReader(string.Empty);

        int exitCode = Aplicacion.Ejecutar(["ruta_que_no_existe_xyz.cs"], entrada, salida);

        Assert.Equal(1, exitCode);
        string output = salida.ToString();
        Assert.Contains("ERROR:", output);
    }

    [Fact]
    public void CP19_RetornaCeroYMuestraTablaSiRutaEsValida()
    {
        string tempFile = Path.GetTempFileName() + ".cs";
        try
        {
            File.WriteAllText(tempFile, "public class Demo { public void Run() {} }");

            using var salida = new StringWriter();
            using var entrada = new StringReader(string.Empty);

            int exitCode = Aplicacion.Ejecutar([tempFile], entrada, salida);

            Assert.Equal(0, exitCode);
            string output = salida.ToString();
            Assert.Contains("Part Name", output);
            Assert.Contains("Demo", output);
            Assert.Contains("TOTAL DEL PROGRAMA", output);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }
}
