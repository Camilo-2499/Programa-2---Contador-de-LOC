namespace Programa2.ContadorLOC.Dominio;

/// <summary>
/// Servicio de dominio que analiza líneas de código fuente en C#, filtrando comentarios
/// y contabilizando partes (clases), ítems (métodos) y líneas efectivas (LOC).
/// </summary>
public sealed class AnalizadorCodigoService
{
    private static readonly string[] PalabrasControl =
    [
        "if", "for", "foreach", "while", "switch", "catch", "using", "lock", "fixed"
    ];

    private static readonly string[] ModificadoresTipo =
    [
        "class", "struct", "interface", "record", "enum"
    ];

    /// <summary>
    /// Analiza una colección de líneas de un archivo y devuelve el resumen con las partes encontradas.
    /// </summary>
    /// <param name="nombreArchivo">Nombre base del archivo analizado.</param>
    /// <param name="lineasBrutas">Colección de líneas sin procesar.</param>
    /// <returns>Objeto <see cref="ResumenConteo"/> con los resultados.</returns>
    public static ResumenConteo Analizar(string nombreArchivo, IEnumerable<string> lineasBrutas)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nombreArchivo);
        ArgumentNullException.ThrowIfNull(lineasBrutas);

        var resumen = new ResumenConteo();
        var lineasLimpias = FiltrarComentariosDeBloque(lineasBrutas);

        ParteInfo? parteActual = null;
        int nivelLlavesClase = 0;
        bool enClase = false;

        foreach (var lineaBruta in lineasLimpias)
        {
            string linea = SanitizarLinea(lineaBruta);
            if (EsLineaNoContable(linea))
            {
                continue;
            }

            if (!enClase && IntentarDetectarTipo(linea, out string? nombreTipo))
            {
                parteActual = new ParteInfo(nombreTipo!);
                enClase = true;
                nivelLlavesClase = ContarDiferenciaLlaves(linea);
                parteActual.IncrementarLoc();
                continue;
            }

            if (enClase)
            {
                ProcesarLineaEnClase(linea, ref nivelLlavesClase, ref enClase, ref parteActual, resumen);
            }
            else
            {
                ProcesarLineaTopLevel(linea, nombreArchivo, ref parteActual);
            }
        }

        if (parteActual is not null)
        {
            resumen.AgregarParte(parteActual);
        }

        return resumen;
    }

    private static void ProcesarLineaEnClase(
        string linea,
        ref int nivelLlavesClase,
        ref bool enClase,
        ref ParteInfo? parteActual,
        ResumenConteo resumen)
    {
        nivelLlavesClase += ContarDiferenciaLlaves(linea);

        if (nivelLlavesClase == 1 && EsFirmaMetodo(linea))
        {
            parteActual!.IncrementarItems();
        }

        parteActual!.IncrementarLoc();

        if (nivelLlavesClase <= 0)
        {
            resumen.AgregarParte(parteActual);
            parteActual = null;
            enClase = false;
            nivelLlavesClase = 0;
        }
    }

    private static void ProcesarLineaTopLevel(string linea, string nombreArchivo, ref ParteInfo? parteActual)
    {
        if (parteActual is null)
        {
            string nombreBase = Path.GetFileNameWithoutExtension(nombreArchivo);
            parteActual = new ParteInfo(string.IsNullOrWhiteSpace(nombreBase) ? "Program" : nombreBase);
            parteActual.IncrementarItems();
        }
        parteActual.IncrementarLoc();
    }

    /// <summary>
    /// Remueve comentarios inline (//) fuera de cadenas de texto y elimina espacios sobrantes.
    /// </summary>
    public static string SanitizarLinea(string linea)
    {
        if (string.IsNullOrWhiteSpace(linea))
        {
            return string.Empty;
        }

        bool dentroDeCadena = false;
        for (int i = 0; i < linea.Length; i++)
        {
            char c = linea[i];
            if (c == '"' && (i == 0 || linea[i - 1] != '\\'))
            {
                dentroDeCadena = !dentroDeCadena;
            }
            else if (!dentroDeCadena && c == '/' && i + 1 < linea.Length && linea[i + 1] == '/')
            {
                return linea[..i].Trim();
            }
        }

        return linea.Trim();
    }

    /// <summary>
    /// Determina si una línea no debe contabilizarse en LOC (en blanco, llaves solas, usings o directivas).
    /// </summary>
    public static bool EsLineaNoContable(string linea)
    {
        if (string.IsNullOrWhiteSpace(linea))
        {
            return true;
        }

        if (linea is "{" or "}" or "{}" or "};")
        {
            return true;
        }

        if (linea.StartsWith("using ", StringComparison.Ordinal) && linea.EndsWith(';'))
        {
            return true;
        }

        if (linea.StartsWith('#'))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Filtra y elimina los bloques multilínea de comentarios (/* ... */).
    /// </summary>
    public static List<string> FiltrarComentariosDeBloque(IEnumerable<string> lineas)
    {
        var resultado = new List<string>();
        bool enBloque = false;

        foreach (var linea in lineas)
        {
            string lineaLimpia = LimpiarBloqueEnLinea(linea, ref enBloque);
            resultado.Add(lineaLimpia);
        }

        return resultado;
    }

    private static string LimpiarBloqueEnLinea(string linea, ref bool enBloque)
    {
        var chars = new System.Text.StringBuilder();
        int i = 0;

        while (i < linea.Length)
        {
            if (enBloque)
            {
                if (i + 1 < linea.Length && linea[i] == '*' && linea[i + 1] == '/')
                {
                    enBloque = false;
                    i += 2;
                }
                else
                {
                    i++;
                }
            }
            else
            {
                if (i + 1 < linea.Length && linea[i] == '/' && linea[i + 1] == '*')
                {
                    enBloque = true;
                    i += 2;
                }
                else
                {
                    chars.Append(linea[i]);
                    i++;
                }
            }
        }

        return chars.ToString();
    }

    private static bool IntentarDetectarTipo(string linea, out string? nombreTipo)
    {
        nombreTipo = null;
        var tokens = linea.Split([' ', '\t', ':', '{'], StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < tokens.Length; i++)
        {
            if (ModificadoresTipo.Contains(tokens[i], StringComparer.Ordinal) && i + 1 < tokens.Length)
            {
                string candidato = tokens[i + 1];
                if ((candidato is "class" or "struct") && i + 2 < tokens.Length)
                {
                    candidato = tokens[i + 2];
                }

                int idxGenerico = candidato.IndexOf('<', StringComparison.Ordinal);
                nombreTipo = idxGenerico >= 0 ? candidato[..idxGenerico] : candidato;
                return true;
            }
        }

        return false;
    }

    private static bool EsFirmaMetodo(string linea)
    {
        int apertura = linea.IndexOf('(');
        int cierre = linea.LastIndexOf(')');

        if (apertura <= 0 || cierre <= apertura)
        {
            return false;
        }

        string prefijo = linea[..apertura].Trim();

        foreach (var control in PalabrasControl)
        {
            if (prefijo.Equals(control, StringComparison.Ordinal) ||
                prefijo.EndsWith(" " + control, StringComparison.Ordinal))
            {
                return false;
            }
        }

        if (prefijo.StartsWith("return ", StringComparison.Ordinal) ||
            prefijo.StartsWith("throw ", StringComparison.Ordinal) ||
            prefijo.Contains(" = ", StringComparison.Ordinal))
        {
            return false;
        }

        // Si termina con punto y coma y no tiene modificador típico ni =>, suele ser una llamada
        if (linea.EndsWith(';') && !linea.Contains("=>", StringComparison.Ordinal))
        {
            // Para ser declaración abstracta o interfaz debe tener modificador o ser firma
            bool esDeclaracion = prefijo.Contains("public ") || prefijo.Contains("protected ") ||
                                 prefijo.Contains("abstract ") || prefijo.Contains("void ") ||
                                 prefijo.Contains("int ") || prefijo.Contains("string ");
            if (!esDeclaracion)
            {
                return false;
            }
        }

        return true;
    }

    private static int ContarDiferenciaLlaves(string linea)
    {
        bool enCadena = false;
        int delta = 0;

        for (int i = 0; i < linea.Length; i++)
        {
            char c = linea[i];
            if (c == '"' && (i == 0 || linea[i - 1] != '\\'))
            {
                enCadena = !enCadena;
            }
            else if (!enCadena)
            {
                if (c == '{')
                {
                    delta++;
                }
                else if (c == '}')
                {
                    delta--;
                }
            }
        }

        return delta;
    }
}
