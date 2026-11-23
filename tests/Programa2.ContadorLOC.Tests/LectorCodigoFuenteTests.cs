using Programa2.ContadorLOC.Infraestructura;

namespace Programa2.ContadorLOC.Tests;

public sealed class LectorCodigoFuenteTests
{
    [Fact]
    public void CP13_LeeArchivoUnicoExistente()
    {
        string tempFile = Path.GetTempFileName() + ".cs";
        try
        {
            File.WriteAllText(tempFile, "public class Temp {}");
            var archivos = LectorCodigoFuente.ObtenerArchivos(tempFile);

            Assert.Single(archivos);
            Assert.Equal(Path.GetFullPath(tempFile), archivos[0]);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void CP14_BuscaArchivosRecursivamenteEnCarpeta()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        string subDir = Path.Combine(tempDir, "Sub");
        Directory.CreateDirectory(subDir);

        try
        {
            File.WriteAllText(Path.Combine(tempDir, "A.cs"), "class A {}");
            File.WriteAllText(Path.Combine(subDir, "B.cs"), "class B {}");
            File.WriteAllText(Path.Combine(tempDir, "Ignorado.txt"), "texto");

            var archivos = LectorCodigoFuente.ObtenerArchivos(tempDir);

            Assert.Equal(2, archivos.Count);
            Assert.Contains(archivos, f => f.EndsWith("A.cs", StringComparison.Ordinal));
            Assert.Contains(archivos, f => f.EndsWith("B.cs", StringComparison.Ordinal));
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public void CP15_LanzaExcepcionSiRutaNoExiste()
    {
        string rutaInexistente = Path.Combine(Path.GetTempPath(), "inexistente_12345.cs");

        Assert.Throws<ArchivoNoValidoException>(() =>
            LectorCodigoFuente.ObtenerArchivos(rutaInexistente));
    }

    [Fact]
    public void CP16_LanzaExcepcionSiArchivoNoEsCs()
    {
        string tempTxt = Path.GetTempFileName() + ".txt";
        try
        {
            File.WriteAllText(tempTxt, "texto plano");

            Assert.Throws<ArchivoNoValidoException>(() =>
                LectorCodigoFuente.ObtenerArchivos(tempTxt));
        }
        finally
        {
            if (File.Exists(tempTxt)) File.Delete(tempTxt);
        }
    }

    [Fact]
    public void CP17_LanzaExcepcionSiCarpetaNoTieneArchivosCs()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            File.WriteAllText(Path.Combine(tempDir, "datos.csv"), "1,2,3");

            Assert.Throws<ArchivoNoValidoException>(() =>
                LectorCodigoFuente.ObtenerArchivos(tempDir));
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }
}
