# Estándar de Codificación y Conteo de Tamaño (LOC) — C#

Proyecto: **Programa 2 — Contador de LOC**  
Nivel: **PSP1 / PSP0.1**  
Basado en: Convenciones de Microsoft para C# y Guía SEI PSP (Capítulo 3).

---

## 1. Estándar de conteo de tamaño (Size Counting Standard)

En PSP1 es fundamental contar las **líneas de código físicas y lógicas** con una regla uniforme y reproducible.

### Reglas de contabilización (¿Qué se cuenta y qué NO?):

| Elemento | ¿Se cuenta como LOC? | Justificación / Regla |
|---|---|---|
| **Sentencias ejecutables** (asignaciones, llamadas, retornos) | **SÍ** | Representa esfuerzo lógico de desarrollo. |
| **Declaraciones** (variables, propiedades, campos, firmas de clase y método) | **SÍ** | Define estructuras y contratos de datos. |
| **Estructuras de control** (`if`, `for`, `while`, `switch`) | **SÍ** | Lógica de flujo del programa. |
| **Líneas en blanco** o con solo espacios / tabulaciones | **NO** | No contienen información lógica. |
| **Comentarios de una línea** (`// ...`) | **NO** | Documentación, no código ejecutable. |
| **Bloques de comentarios** (`/* ... */`) | **NO** | Documentación de bloque. |
| **Directivas de importación** (`using ...;`) | **NO** | Dependencias de espacio de nombres, no lógica del módulo. |
| **Líneas que contienen únicamente llaves** (`{` o `}`) | **NO** | Delimitadores sintácticos de bloque en estilo Allman. |

---

## 2. Definición de Partes e Ítems en C#

* **Parte (Part):** Cada tipo definido por el usuario: `class`, `struct`, `record` o `interface`.
  * Una parte inicia en la línea donde se declara (ej. `public class CalculadoraEstadistica`).
  * Termina cuando se cierra la llave `}` correspondiente a ese bloque de clase (control de llaves balanceadas).
* **Ítem (Item):** Cada función miembro de la parte:
  * Métodos (`public void Metodo()`, `private int Sumar(int a, int b)`).
  * Constructores (`public MiClase()`).
  * No se cuentan como ítems independientes las propiedades automáticas de una sola línea (`public int Id { get; set; }`), aunque sus líneas sí se suman al tamaño de la parte si tienen lógica.

---

## 3. Estándar de codificación C#

| # | Regla | Ejemplo correcto | Incorrecto |
|---|---|---|---|
| E-01 | Tipos, métodos y propiedades en **PascalCase**. | `ContadorLineasService.Analizar()` | `contador_lineas()` |
| E-02 | Variables locales y parámetros en **camelCase**. | `totalLineas`, `rutaArchivo` | `TotalLineas`, `tl` |
| E-03 | Campos privados con guion bajo (`_`). | `_partes`, `_lineas` | `partes`, `m_partes` |
| E-04 | Sin números mágicos; usar constantes descriptivas. | `Constantes.ExtensionCs = ".cs"` | `if (ext == ".cs")` |
| E-05 | Un tipo principal por archivo, con el mismo nombre. | `ParteInfo.cs` | `TiposVarios.cs` |
| E-06 | Estilo de llaves Allman (llave en línea propia). | `class MiClase\n{` | `class MiClase {` |
| E-07 | Métodos de **30 líneas o menos** y una sola responsabilidad. | Descomponer análisis en submétodos | Un método `Analizar` de 150 líneas |
| E-08 | Excepciones personalizadas para errores de dominio/infraestructura. | `ArchivoNoValidoException` | `throw new Exception()` |
| E-09 | Tipos anulables activados (`<Nullable>enable</Nullable>`). | Validar referencias nulas explícitamente | Asumir que nada es nulo |
| E-10 | El dominio nunca depende de `System.Console` ni de la UI. | El dominio retorna objetos `ResumenConteo` | El dominio imprime en pantalla |
