# Pruebas · Diseño de Casos de Prueba (PSP1)

Proyecto: **Programa 2 — Contador de LOC**  
Nivel: **PSP1 / PSP0.1**

Cada caso de prueba se deriva de los requisitos (RF-01 a RF-07, RNF-01 a RNF-07) y del estándar de codificación y conteo. Se ejecutan automáticamente mediante el proyecto xUnit `tests/Programa2.ContadorLOC.Tests`.

Ejecutar pruebas: `dotnet test` desde la raíz de la solución.

---

## Matriz de Casos de Prueba

| Id | Requisito | Descripción / Entrada | Resultado Esperado | Técnica |
|---|---|---|---|---|
| **CP-01** | RF-02 | Líneas con comentarios simples `//`, con sangría y XML `///` | No se suman a LOC; ignorados | Partición: comentarios de línea |
| **CP-02** | RF-02 | Bloques de comentarios multilínea `/* ... */` | No se suman a LOC; código posterior sí se cuenta | Partición: bloques multilínea |
| **CP-03** | RF-02 | Líneas vacías, solo espacios o tabulaciones | No se suman a LOC | Límite: líneas en blanco |
| **CP-04** | RF-02 | Líneas que contienen `{`, `}`, `using ...;` y directivas `#` | Descartadas de LOC | Sintaxis C# |
| **CP-05** | RF-03, RF-04, RF-05 | Clase con 2 métodos y cuerpo de sentencias | 1 parte (`Calculadora`), 2 ítems, 5 LOC | Valor conocido |
| **CP-06** | RF-04 | Clase con métodos que llaman a `Console.WriteLine`, `Math.Sqrt`, etc. | 1 ítem (solo la firma a nivel 1; llamadas internas no son ítems) | Límite: anidamiento |
| **CP-07** | RF-03 | Dos clases declaradas en el mismo archivo fuente | 2 partes identificadas de forma independiente | Partición: múltiples tipos |
| **CP-08** | RF-03, RF-04 | Archivo sin clase explícita (Top-Level Statements en `Program.cs`) | 1 parte (`Program`), 1 ítem (`Main`), LOC efectivas contadas | Caso especial C# |
| **CP-09** | RF-02 | Sentencia con comentario al final (`int x = 1; // com`) | Comentario recortado; 1 LOC contada | Partición: inline |
| **CP-10** | RF-03 | Cadena de texto con llaves literales (`"{hola}"`) | Llaves ignoradas en el balance de bloques | Partición: cadenas |
| **CP-11** | RF-03 | Declaración de `interface`, `struct`, `record` y `enum` | Reconocidos como partes válidas | Cobertura de tipos |
| **CP-12** | RF-04 | Métodos con cuerpo de expresión (`=>`) | Reconocidos como ítems válidos | Sintaxis moderna C# |
| **CP-13** | RF-01 | Ruta a un archivo `.cs` único existente | Lista con 1 archivo | Integración I/O |
| **CP-14** | RF-01 | Ruta a un directorio con subcarpetas | Lista recursiva de `.cs`, excluye `bin/` y `obj/` | Integración I/O |
| **CP-15** | RF-07 | Ruta inexistente | `ArchivoNoValidoException` con mensaje claro | Manejo de errores |
| **CP-16** | RF-07 | Archivo con extensión distinta a `.cs` (`.txt`) | `ArchivoNoValidoException` indicando el tipo | Validación de entrada |
| **CP-17** | RF-07 | Directorio sin archivos `.cs` | `ArchivoNoValidoException` indicando carpeta vacía | Validación de entrada |
| **CP-18** | RF-06, RF-07 | Argumento de ruta inválido a `Aplicacion.Ejecutar` | Retorno código 1 y mensaje `ERROR:` en consola | Sistema / CLI |
| **CP-19** | RF-06 | Argumento válido a `Aplicacion.Ejecutar` | Retorno código 0 y tabla formateada SEI | Sistema / CLI |
| **CP-20** | Requisito SEI | **Prueba obligatoria 1:** Conteo del código del **Programa 1** | Tabla generada con 10 partes, 22 ítems, 119 LOC | Integración del curso |
| **CP-21** | Requisito SEI | **Prueba obligatoria 2:** Conteo del código del **Programa 2** (autocirugía) | Tabla generada con 8 partes, 26 ítems, 259 LOC | Integración del curso |

---

## Cobertura de Requisitos

* **RF-01 (Lectura de archivos/carpetas):** CP-13, CP-14
* **RF-02 (Filtrado de no contabilizables):** CP-01, CP-02, CP-03, CP-04, CP-09
* **RF-03 (Identificación de clases/partes):** CP-05, CP-07, CP-08, CP-10, CP-11
* **RF-04 (Identificación de ítems/métodos):** CP-05, CP-06, CP-08, CP-12
* **RF-05 (Cálculo de tamaño por parte y total):** CP-05, CP-20, CP-21
* **RF-06 (Salida tabular SEI):** CP-19, CP-20, CP-21
* **RF-07 (Validación de errores):** CP-15, CP-16, CP-17, CP-18
