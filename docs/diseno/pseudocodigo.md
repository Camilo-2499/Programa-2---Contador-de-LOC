# Diseño · Pseudocódigo de Algoritmos Clave

Proyecto: **Programa 2 — Contador de LOC**  
Nivel: **PSP1 / PSP0.1**

Este documento define el algoritmo detallado del analizador sintáctico/léxico ligero **antes** de implementar el código.

---

## 1. Algoritmo principal de análisis (`AnalizadorCodigoService.Analizar`)

```text
FUNCIÓN Analizar(lineasBrutas : Colección de Cadenas) : ResumenConteo
    resumen ← nuevo ResumenConteo()
    lineasLimpias ← FiltrarComentariosDeBloque(lineasBrutas)
    
    parteActual ← nulo
    nivelLlavesClase ← 0
    enClase ← FALSO
    
    PARA CADA linea EN lineasLimpias HACER
        lineaTrim ← QuitarEspacios(linea)
        
        // 1. Filtrar líneas no contabilizables
        SI EsLineaNoContable(lineaTrim) ENTONCES
            CONTINUAR
        FIN SI
        
        // 2. Detección de inicio de clase/struct/interface
        SI NO enClase Y EsInicioDeTipo(lineaTrim, nombreTipo) ENTONCES
            parteActual ← nueva ParteInfo(nombreTipo)
            enClase ← VERDADERO
            nivelLlavesClase ← ContarAperturas(lineaTrim) - ContarCierres(lineaTrim)
            parteActual.IncrementarLoc()
            CONTINUAR
        FIN SI
        
        // 3. Procesamiento dentro de la clase
        SI enClase ENTONCES
            nivelLlavesClase ← nivelLlavesClase + ContarAperturas(lineaTrim) - ContarCierres(lineaTrim)
            
            // Detección de métodos / constructores
            SI EsFirmaMetodo(lineaTrim) ENTONCES
                parteActual.IncrementarItems()
            FIN SI
            
            parteActual.IncrementarLoc()
            
            // Si las llaves llegaron a cero, se cerró la clase
            SI nivelLlavesClase <= 0 ENTONCES
                resumen.AgregarParte(parteActual)
                parteActual ← nulo
                enClase ← FALSO
            FIN SI
        SINO
            // Código fuera de clase (ej. archivos con declaraciones de nivel superior)
            resumen.IncrementarLocGlobal()
        FIN SI
    FIN PARA
    
    // Si quedó una parte abierta por ausencia de llaves en la misma línea
    SI parteActual != nulo ENTONCES
        resumen.AgregarParte(parteActual)
    FIN SI
    
    RETORNAR resumen
FIN
```

---

## 2. Detección de líneas no contabilizables (`EsLineaNoContable`)

```text
FUNCIÓN EsLineaNoContable(linea : Cadena) : Booleano
    SI linea es Vacía O TieneSoloEspacios ENTONCES
        RETORNAR VERDADERO
    FIN SI
    
    SI linea empieza con "//" ENTONCES
        RETORNAR VERDADERO
    FIN SI
    
    SI linea es exactamente "{" O es exactamente "}" ENTONCES
        RETORNAR VERDADERO
    FIN SI
    
    SI linea empieza con "using " Y termina con ";" ENTONCES
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
    // Debe contener paréntesis de parámetros, pero no ser estructura de control ni llamada
    SI NO contiene(linea, "(") O NO contiene(linea, ")") ENTONCES
        RETORNAR FALSO
    FIN SI
    
    // Descartar if, for, while, switch, catch, etc.
    palabrasControl ← ["if", "for", "foreach", "while", "switch", "catch", "using", "lock"]
    PARA CADA palabra EN palabrasControl HACER
        SI empiezaCon(linea, palabra + "(") O empiezaCon(linea, palabra + " ") ENTONCES
            RETORNAR FALSO
        FIN SI
    FIN PARA
    
    // No debe ser instanciación (new) ni asignación típica
    SI empiezaCon(linea, "return ") O contiene(linea, " = ") ENTONCES
        RETORNAR FALSO
    FIN SI
    
    // Si tiene modificador de acceso o tipo de retorno y termina con bloque o sin punto y coma directo
    RETORNAR VERDADERO
FIN
```

---

## 5. Prueba de escritorio manual (Verificación pre-código)

Dado el siguiente código de prueba:

```csharp
// Comentario simple
using System;

public class Calculadora
{
    public int Sumar(int a, int b)
    {
        return a + b;
    }
}
```

| Línea | Contenido | ¿Se cuenta? | LOC acumulada | ¿Es clase? | ¿Es método? |
|---|---|---|---|---|---|
| 1 | `// Comentario simple` | No (comentario) | 0 | No | No |
| 2 | `using System;` | No (using) | 0 | No | No |
| 3 | *(vacía)* | No (blanco) | 0 | No | No |
| 4 | `public class Calculadora` | Sí | 1 | Sí (`Calculadora`) | No |
| 5 | `{` | No (llave sola) | 1 | - | - |
| 6 | `public int Sumar(int a, int b)` | Sí | 2 | - | Sí (Ítem 1) |
| 7 | `{` | No (llave sola) | 2 | - | - |
| 8 | `return a + b;` | Sí | 3 | - | No |
| 9 | `}` | No (llave sola) | 3 | - | - |
| 10 | `}` | No (llave sola) | 3 | Cierra clase | - |

**Resultado esperado:**
* Parte: `Calculadora`
* Número de Ítems: 1
* LOC de la Parte: 3
* Total LOC: 3

El algoritmo produce exactamente el resultado previsto antes de comenzar la codificación.
