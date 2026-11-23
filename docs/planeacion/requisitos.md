# Programa 2 · Contador de Líneas de Código (LOC) — Requisitos

Fuente: *Assignment Kit for Program 2* (SEI, Carnegie Mellon University, 2012). Nivel PSP1 / PSP0.1.

## Enunciado

Utilizando el proceso PSP1 (o PSP0.1), desarrollar un programa que cuente las líneas de código (**LOC**) de un programa escrito en C#, determinando:
1. El **tamaño total** del programa (total LOC).
2. El **tamaño de cada una de sus partes** (clases o estructuras) en LOC.
3. El **número de ítems** (métodos o funciones) presentes en cada parte.

El programa debe imprimir una tabla formateada que liste el nombre de cada parte, su cantidad de ítems, el tamaño en LOC de la parte y el tamaño total acumulado. Debe probarse exhaustivamente, incluyendo como mínimo el conteo del código del **Programa 1** y del propio **Programa 2**.

---

## Preguntas resueltas antes de estimar

| # | Pregunta | Respuesta (fuente / decisión) |
|---|---|---|
| P1 | ¿Qué se define como una "Parte"? | En C#, una parte es una **clase** (`class`), **estructura** (`struct`) o **interfaz** (`interface`). |
| P2 | ¿Qué se define como un "Ítem"? | Cada método, constructor o función miembro dentro de la clase. |
| P3 | ¿Qué se cuenta exactamente como una línea de código (LOC)? | **Líneas lógicas ejecutables o de declaración**. Se excluyen líneas en blanco, comentarios (`//`, `/* */`), directivas `using` y llaves solas (`{`, `}`). |
| P4 | ¿Debe soportar un solo archivo o múltiples archivos / carpetas? | Debe permitir ingresar la ruta de un archivo `.cs` individual o un directorio con el código fuente para analizar todas las clases del proyecto. |
| P5 | ¿Cuál es el formato de salida requerido? | Formato tabular similar a la Tabla 1 del kit: Nombre de Parte, Cantidad de Métodos, Tamaño (LOC) y Total general. |
| P6 | ¿Cuáles son las pruebas obligatorias mínimas? | 1) Analizar y contar el código del **Programa 1** (media y desviación). 2) Analizarse a sí mismo (**Programa 2**). |

---

## Requisitos funcionales

| Id | Requisito | Criterio de aceptación |
|---|---|---|
| RF-01 | Leer código fuente en C# desde un archivo `.cs` o recursivamente desde un directorio. | Procesa correctamente archivos UTF-8 sin errores de lectura. |
| RF-02 | Filtrar líneas no contabilizables. | Ignora líneas vacías o con solo espacios, comentarios de una línea (`//`), bloques de comentarios (`/* ... */`), llaves aisladas (`{` / `}`) y directivas `using`. |
| RF-03 | Identificar partes (clases). | Detecta la declaración y límites de cada clase/struct en el código fuente. |
| RF-04 | Identificar ítems (métodos). | Cuenta con precisión las signaturas de métodos dentro de cada parte identificada. |
| RF-05 | Calcular tamaño por parte y total. | Determina las LOC efectivas de cada clase y la sumatoria total del programa analizado. |
| RF-06 | Presentar la salida en tabla estructurada. | Imprime en consola las columnas: `Nombre de la Parte`, `Número de Ítems`, `Tamaño de Parte (LOC)` y `Total Acumulado`. |
| RF-07 | Validar entradas y rutas de archivo. | Muestra mensajes claros en español si la ruta no existe, si el archivo no es `.cs` o si está vacío, sin provocar excepciones no controladas. |

---

## Requisitos no funcionales

| Id | Requisito | Criterio de aceptación |
|---|---|---|
| RNF-01 | Plataforma y lenguaje. | Aplicación de consola en C# (.NET 9), ejecutable en Windows y Linux. |
| RNF-02 | Arquitectura limpia y mantenible. | Separación por capas (Dominio, Infraestructura, Aplicación, Presentación). |
| RNF-03 | Estándar de codificación. | Clases y métodos documentados, métodos de ≤ 30 LOC, nombres descriptivos. |
| RNF-04 | Precisión de conteo. | Consistencia del 100% al volver a contar el mismo archivo bajo las mismas reglas. |
| RNF-05 | Usabilidad. | Mensajes de ayuda y error claros en consola con soporte para rutas relativas y absolutas. |
| RNF-06 | Pruebas automatizadas. | Suite de pruebas unitarias en xUnit que verifiquen el analizador de líneas, detección de clases y conteo de métodos. |
| RNF-07 | Trazabilidad PSP. | Commits en Git asociados al cierre de cada fase de PSP1 (planeación, diseño, código, compilación, pruebas, postmortem). |
