using Programa2.ContadorLOC.Dominio;

namespace Programa2.ContadorLOC.Tests;

public sealed class AnalizadorCodigoServiceTests
{
    [Fact]
    public void CP01_IgnoraComentariosDeUnaLinea()
    {
        string[] lineas =
        [
            "// Comentario 1",
            "   // Comentario con espacios",
            "/// <summary>XML doc</summary>",
            "public class MiClase",
            "// otro comentario",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("MiClase.cs", lineas);

        Assert.Single(resumen.Partes);
        Assert.Equal("MiClase", resumen.Partes[0].Nombre);
        // Solo la línea de declaración de clase es contable
        Assert.Equal(1, resumen.Partes[0].TamanoLoc);
    }

    [Fact]
    public void CP02_IgnoraBloquesComentariosMultilinea()
    {
        string[] lineas =
        [
            "/* Inicio bloque",
            "   linea 2",
            "   fin bloque */",
            "public class A",
            "{",
            "   /* comentario */ int x = 1;",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("A.cs", lineas);

        Assert.Single(resumen.Partes);
        // public class A (1) + int x = 1; (1) = 2 LOC
        Assert.Equal(2, resumen.Partes[0].TamanoLoc);
    }

    [Fact]
    public void CP03_IgnoraLineasEnBlancoYEspacios()
    {
        string[] lineas =
        [
            "",
            "    ",
            "\t",
            "public class Vacia",
            "{",
            "",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("Vacia.cs", lineas);

        Assert.Single(resumen.Partes);
        Assert.Equal(1, resumen.Partes[0].TamanoLoc);
    }

    [Fact]
    public void CP04_IgnoraLlavesSolasUsingsYDirectivas()
    {
        string[] lineas =
        [
            "using System;",
            "using System.Collections.Generic;",
            "#nullable enable",
            "#region Directivas",
            "public class Config",
            "{",
            "}",
            "#endregion"
        ];

        var resumen = AnalizadorCodigoService.Analizar("Config.cs", lineas);

        Assert.Single(resumen.Partes);
        Assert.Equal(1, resumen.Partes[0].TamanoLoc);
    }

    [Fact]
    public void CP05_DetectaClaseSimpleConMetodosEItems()
    {
        string[] lineas =
        [
            "public class Calculadora",
            "{",
            "    public int Sumar(int a, int b)",
            "    {",
            "        return a + b;",
            "    }",
            "    public int Restar(int a, int b)",
            "    {",
            "        return a - b;",
            "    }",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("Calculadora.cs", lineas);

        Assert.Single(resumen.Partes);
        var parte = resumen.Partes[0];
        Assert.Equal("Calculadora", parte.Nombre);
        Assert.Equal(2, parte.CantidadItems);
        // class (1) + Sumar (1) + return (1) + Restar (1) + return (1) = 5 LOC
        Assert.Equal(5, parte.TamanoLoc);
    }

    [Fact]
    public void CP06_NoCuentaLlamadasAFuncionComoNuevosMetodos()
    {
        string[] lineas =
        [
            "public class Servicio",
            "{",
            "    public void Procesar()",
            "    {",
            "        Console.WriteLine(\"hola\");",
            "        Math.Sqrt(16);",
            "        var lista = new List<int>();",
            "        lista.Add(1);",
            "    }",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("Servicio.cs", lineas);

        Assert.Single(resumen.Partes);
        var parte = resumen.Partes[0];
        // Solo Procesar() es un método a nivel 1
        Assert.Equal(1, parte.CantidadItems);
        Assert.Equal(6, parte.TamanoLoc);
    }

    [Fact]
    public void CP07_DetectaMultiplesClasesEnMismoArchivo()
    {
        string[] lineas =
        [
            "public class Primera",
            "{",
            "    public void MetodoUno() {}",
            "}",
            "public class Segunda",
            "{",
            "    public void MetodoDos() {}",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("Varias.cs", lineas);

        Assert.Equal(2, resumen.Partes.Count);
        Assert.Equal("Primera", resumen.Partes[0].Nombre);
        Assert.Equal("Segunda", resumen.Partes[1].Nombre);
        Assert.Equal(2, resumen.TotalItems);
    }

    [Fact]
    public void CP08_DetectaTopLevelStatements()
    {
        string[] lineas =
        [
            "// Archivo Program.cs sin clase",
            "using System;",
            "",
            "Console.WriteLine(\"Inicio\");",
            "int resultado = 42;",
            "return resultado;"
        ];

        var resumen = AnalizadorCodigoService.Analizar("Program.cs", lineas);

        Assert.Single(resumen.Partes);
        var parte = resumen.Partes[0];
        Assert.Equal("Program", parte.Nombre);
        Assert.Equal(1, parte.CantidadItems);
        Assert.Equal(3, parte.TamanoLoc);
    }

    [Fact]
    public void CP09_SoportaComentariosInlineContandoLocEfectiva()
    {
        string[] lineas =
        [
            "public class InlineTest",
            "{",
            "    int x = 10; // asignacion de variable",
            "    string url = \"http://test.com\"; // comentario",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("InlineTest.cs", lineas);

        Assert.Single(resumen.Partes);
        Assert.Equal(3, resumen.Partes[0].TamanoLoc);
    }

    [Fact]
    public void CP10_NoConfundeLlavesDentroDeCadenasDeTexto()
    {
        string[] lineas =
        [
            "public class Formato",
            "{",
            "    public string Obtener()",
            "    {",
            "        return \"{hola}\";",
            "    }",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("Formato.cs", lineas);

        Assert.Single(resumen.Partes);
        Assert.Equal(1, resumen.Partes[0].CantidadItems);
        Assert.Equal(3, resumen.Partes[0].TamanoLoc);
    }

    [Fact]
    public void CP11_DetectaEstructurasInterfacesRecordsYEnums()
    {
        string[] lineas =
        [
            "public interface IContrato { void Ejecutar(); }",
            "public struct Punto { public int X; }",
            "public enum Estado { Activo, Inactivo }"
        ];

        var resumen = AnalizadorCodigoService.Analizar("Tipos.cs", lineas);

        Assert.Equal(3, resumen.Partes.Count);
        Assert.Contains(resumen.Partes, p => p.Nombre == "IContrato");
        Assert.Contains(resumen.Partes, p => p.Nombre == "Punto");
        Assert.Contains(resumen.Partes, p => p.Nombre == "Estado");
    }

    [Fact]
    public void CP12_DetectaMetodosConCuerpoDeExpresion()
    {
        string[] lineas =
        [
            "public class MathHelper",
            "{",
            "    public int Duplicar(int n) => n * 2;",
            "    public int Triplicar(int n) => n * 3;",
            "}"
        ];

        var resumen = AnalizadorCodigoService.Analizar("MathHelper.cs", lineas);

        Assert.Single(resumen.Partes);
        Assert.Equal(2, resumen.Partes[0].CantidadItems);
        Assert.Equal(3, resumen.Partes[0].TamanoLoc);
    }
}
