# Diseño · Pseudocódigo de Algoritmos Clave

Proyecto: **Programa 2 — Contador de LOC**  
Nivel: **PSP1 / PSP0.1**

Este documento define el algoritmo detallado del analizador sintáctico/léxico ligero **antes** de implementar el código.

---

## 1. Algoritmo principal de análisis (`AnalizadorCodigoService.Analizar`)

```text
FUNCIÓN Analizar(nombreArchivo : Cadena, lineasBrutas : Colección de Cadenas) : ResumenConteo
    resumen ← nuevo ResumenConteo()
    lineasLimpias ← FiltrarComentariosDeBloque(lineasBrutas)
    
    parteActual ← nulo
    nivelLlavesClase ← 0
    enClase ← FALSO
    tieneTopLevel ← FALSO
    
    PARA CADA lineaBruta EN lineasLimpias HACER
        // 1. Quitar comentarios inline y espacios extremos
        linea ← SanitizarLinea(lineaBruta)
        
        // 2. Filtrar líneas no contabilizables
        SI EsLineaNoContable(linea) ENTONCES
            CONTINUAR
        FIN SI
        
        // 3. Detección de inicio de clase/struct/interface/record
        SI NO enClase Y EsInicioDeTipo(linea, nombreTipo) ENTONCES
            parteActual ← nueva ParteInfo(nombreTipo)
            enClase ← VERDADERO
            nivelLlavesClase ← ContarLlavesAperturaFueraDeComillas(linea) - ContarLlavesCierreFueraDeComillas(linea)
            parteActual.IncrementarLoc()
            CONTINUAR
        FIN SI
        
        // 4. Procesamiento dentro de una clase
        SI enClase ENTONCES
            nivelLlavesClase ← nivelLlavesClase + ContarLlavesAperturaFueraDeComillas(linea) - ContarLlavesCierreFueraDeComillas(linea)
            
            // Un método SOLO puede declararse a nivel 1 de anidamiento dentro de la clase
            SI nivelLlavesClase == 1 Y EsFirmaMetodo(linea) ENTONCES
                parteActual.IncrementarItems()
            FIN SI
            
            parteActual.IncrementarLoc()
            
            // Si las llaves de la clase se cierran por completo
            SI nivelLlavesClase <= 0 ENTONCES
                resumen.AgregarParte(parteActual)
                parteActual ← nulo
                enClase ← FALSO
                nivelLlavesClase ← 0
            FIN SI
        SINO
            // 5. Caso especial: Top-Level Statements (ej. Program.cs de .NET 9 sin clase)
            SI parteActual == nulo ENTONCES
                parteActual ← nueva ParteInfo(ObtenerNombreBase(nombreArchivo))
                parteActual.IncrementarItems() // Se contabiliza el punto de entrada como 1 ítem (Main)
                tieneTopLevel ← VERDADERO
            FIN SI
            parteActual.IncrementarLoc()
        FIN SI
    FIN PARA
    
    // Si quedaron sentencias Top-Level o una clase sin cierre explícito en la última línea
    SI parteActual != nulo ENTONCES
        resumen.AgregarParte(parteActual)
    FIN SI
    
    RETORNAR resumen
FIN
```

---

## 2. Sanitización y detección de líneas no contabilizables

```text
FUNCIÓN SanitizarLinea(linea : Cadena) : Cadena
    // Si contiene "//", eliminar el comentario inline preservando lo que esté entre comillas
    lineaSinComentario ← TruncarComentarioInline(linea)
    RETORNAR QuitarEspaciosExtremos(lineaSinComentario)
FIN

FUNCIÓN EsLineaNoContable(linea : Cadena) : Booleano
    SI linea es Vacía ENTONCES
        RETORNAR VERDADERO
    FIN SI
    
    // Llaves aisladas
    SI linea == "{" O linea == "}" O linea == "{}" ENTONCES
        RETORNAR VERDADERO
    FIN SI
    
    // Directivas de espacio de nombres
    SI empiezaCon(linea, "using ") Y terminaCon(linea, ";") ENTONCES
        RETORNAR VERDADERO
    FIN SI
    
    // Directivas del preprocesador
    SI empiezaCon(linea, "#") ENTONCES
        RETORNAR VERDADERO
    FIN SI
    
    RETORNAR FALSO
FIN
```

---

## 3. Filtrado de comentarios multilínea (`/* ... */`)

```text
FUNCIÓN FiltrarComentariosDeBloque(lineas : Lista de Cadenas) : Lista de Cadenas
    resultado ← nueva Lista()
    enComentarioBloque ← FALSO
    
    PARA CADA linea EN lineas HACER
        lineaProcesada ← ""
        i ← 0
        MIENTRAS i < longitud(linea) HACER
            SI enComentarioBloque ENTONCES
                SI subcadena(linea, i, 2) == "*/" ENTONCES
                    enComentarioBloque ← FALSO
                    i ← i + 2
                SINO
                    i ← i + 1
                FIN SI
            SINO
                SI subcadena(linea, i, 2) == "/*" ENTONCES
                    enComentarioBloque ← VERDADERO
                    i ← i + 2
                SINO
                    lineaProcesada ← lineaProcesada + linea[i]
                    i ← i + 1
                FIN SI
            FIN SI
        FIN MIENTRAS
        
        resultado.Agregar(lineaProcesada)
    FIN PARA
    
    RETORNAR resultado
FIN
```

---

## 4. Detección de firmas de métodos e ítems

```text
FUNCIÓN EsFirmaMetodo(linea : Cadena) : Booleano
    // Debe tener paréntesis para argumentos
    SI NO contiene(linea, "(") O NO contiene(linea, ")") ENTONCES
        RETORNAR FALSO
    FIN SI
    
    // Descartar estructuras de control de flujo
    palabrasControl ← ["if", "for", "foreach", "while", "switch", "catch", "using", "lock"]
    PARA CADA palabra EN palabrasControl HACER
        SI empiezaCon(linea, palabra + "(") O empiezaCon(linea, palabra + " ") ENTONCES
            RETORNAR FALSO
        FIN SI
    FIN PARA
    
    // Descartar retornos, asignaciones típicas y llamadas
    SI empiezaCon(linea, "return ") O contiene(linea, " = ") ENTONCES
        RETORNAR FALSO
    FIN SI
    
    // Un método o constructor contiene modificadores de acceso o tipo de retorno
    RETORNAR VERDADERO
FIN
```

---

## 5. Prueba de escritorio manual con Top-Level Statements y comentarios inline

```csharp
// PSP0 · Programa 1
using Programa1.Estadistica;

Console.OutputEncoding = Encoding.UTF8; // comentario inline

return Aplicacion.Ejecutar(args, Console.In, Console.Out);
```

| Línea | Contenido | Sanitizado | ¿Se cuenta? | LOC acumulada | Parte / Ítem |
|---|---|---|---|---|---|
| 1 | `// PSP0 · Programa 1` | `""` | No (vacía) | 0 | - |
| 2 | `using Programa1...;` | `using Programa1...;` | No (using) | 0 | - |
| 3 | *(vacía)* | `""` | No (vacía) | 0 | - |
| 4 | `Console.Output...; // com` | `Console.OutputEncoding = ...;` | Sí | 1 | Parte `Program` (Ítem 1) |
| 5 | *(vacía)* | `""` | No (vacía) | 1 | - |
| 6 | `return Aplicacion...;` | `return Aplicacion...;` | Sí | 2 | Parte `Program` |

**Resultado esperado:**
* Parte: `Program`
* Cantidad de Ítems: 1
* LOC de la Parte: 2 LOC
* Total LOC: 2 LOC

El algoritmo procesa tanto clases tradicionales como sentencias de nivel superior de manera consistente.
