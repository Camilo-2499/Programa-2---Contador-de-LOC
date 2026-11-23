# Postmortem · Propuesta de Mejora del Proceso (PIP #2)

| Campo | Valor |
|---|---|
| Estudiante | Jhojan Camilo |
| Programa | 2 · Contador de Líneas de Código (PSP1 / PSP0.1) |
| Fecha | 23 de noviembre de 2026 |

---

## 1. Evaluación de las propuestas del PIP #1 (del Programa 1)

| Propuesta previa (PIP #1) | Resultado en Programa 2 | Estado |
|---|---|---|
| *1. Codificar con el pseudocódigo abierto al lado para evitar defectos de lógica.* | Se redujeron los defectos lógicos drásticamente: solo 1 defecto lógico (D2) frente a 7 en el Programa 1. | **Cumplida con éxito** |
| *2. Escribir pruebas antes o inmediatamente después de compilar.* | Se diseñaron 19 casos xUnit antes de la primera corrida global. Se atraparon las discrepancias en minutos. | **Cumplida con éxito** |
| *3. Disminuir el tiempo de corrección en Pruebas a < 15 min.* | Se dedicaron solo 5 minutos a la corrección del defecto D2. | **Cumplida con éxito** |
| *4. Uso del cronómetro de PSP Tracker.* | El 100% de las fases se registraron en vivo con el cronómetro de la aplicación. | **Cumplida con éxito** |

---

## 2. Nuevas propuestas de mejora para el Programa 3 (Regresión Lineal)

| # | Problema detectado (con datos) | Propuesta de mejora | Prioridad | Medición para el Programa 3 |
|---|---|---|---|---|
| **1** | **Subestimación de tamaño LOC (+32.8 %):** Se estimaron 195 LOC y el código real fue de 259 LOC debido al soporte de casos límite de C# (records, enums, inline comments). | Al realizar la estimación PROBE para el Programa 3, considerar un 20% adicional en los componentes de parsing y manejo de expresiones matemáticas complejas. | Alta | Error de estimación de tamaño en Programa 3 $\le 15\%$. |
| **2** | **Sobrestimación de tiempo planificado:** Se planearon 390 min y se requirieron ~120 min gracias a la reutilización conceptual de la arquitectura en capas. | Recalcular la productividad base para el Programa 3 usando el promedio ponderado entre Programa 1 (24 LOC/h) y Programa 2 (130 LOC/h), proyectando una velocidad realista de ~50-60 LOC/h. | Media | Desviación de tiempo total en Programa 3 $\le 20\%$. |
| **3** | **Defecto D2 en Pruebas (5 min):** La acumulación de llaves se condicionó erróneamente al filtro de descarte de LOC. | En algoritmos de análisis sintáctico o cálculo acumulativo (como sumatorias $\sum x, \sum y, \sum x^2$ del Programa 3), separar explícitamente la etapa de captura de datos de la etapa de evaluación de reglas. | Alta | 0 defectos de acumulación en Programa 3. |

---

## 3. Conclusión del Ingeniero

El Programa 2 demostró la efectividad de invertir tiempo de calidad en la fase de **Diseño**. A pesar de construir un programa más complejo que el Programa 1 (259 LOC vs 137 LOC), el tiempo total de desarrollo disminuyó de 342 minutos a ~120 minutos, y los defectos cayeron de 9 a solo 2.
