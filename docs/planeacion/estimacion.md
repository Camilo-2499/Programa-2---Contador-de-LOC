# Estimación de Tamaño y Recursos (PSP1 · Método PROBE)

Proyecto: **Programa 2 — Contador de LOC**  
Nivel: **PSP1 / PSP0.1**  
Fecha de estimación: **23/11/2026**

---

## 1. Datos históricos de referencia (Programa 1)

Los datos reales obtenidos del desarrollo del **Programa 1 (Media y Desviación Estándar)** sirven como base histórica para la estimación:

| Indicador | Valor Real Programa 1 |
|---|---|
| Tamaño real del programa base | 137 LOC |
| Tiempo total real de desarrollo | 342 minutos (~5.7 horas) |
| Productividad histórica real | 24.0 LOC / hora (0.40 LOC / min) |
| Defectos totales corregidos | 9 defectos |

---

## 2. Diseño conceptual y estimación de partes (PROBE)

Para el **Programa 2**, se identifican 4 componentes principales necesarios para cumplir con los requerimientos:

| Parte / Componente | Tipo de Parte | Ítems estimados | LOC estimada (Proxy) |
|---|---|---|---|
| `ModeloPartes` (ParteInfo, ResumenConteo) | Dominio / Estructura | 3 | 35 LOC |
| `ContadorLineasService` (Reglas de conteo, filtro comentarios) | Dominio / Lógica | 4 | 65 LOC |
| `LectorCodigoFuente` (Lectura de archivos y carpetas .cs) | Infraestructura | 3 | 45 LOC |
| `ConsolaUI` y `Program` (CLI y renderizado de tabla) | Presentación / App | 3 | 50 LOC |
| **Total Estimado de Tamaño** | — | **13 ítems** | **195 LOC** |

*Tamaño planificado total:* **195 LOC** (con un rango esperado de 180 a 220 LOC).

---

## 3. Estimación de tiempo de desarrollo

Aplicando la productividad histórica del Programa 1 (24 LOC/hora):
$$\text{Tiempo base} = \frac{195 \text{ LOC}}{24 \text{ LOC/hora}} \approx 8.1 \text{ horas}$$

Ajustando por familiaridad con el framework .NET 9, modularidad y reutilización conceptual de la arquitectura del Programa 1:
* **Tiempo total planificado:** **390 minutos (6.5 horas)**.

### Distribución planificada por fases PSP1:

| Fase PSP1 | Actividades clave | % Estimado | Minutos Planificados |
|---|---|---|---|
| **Planificación** | Lectura del kit, preguntas resueltas, requisitos RF/RNF, estimación PROBE | 10 % | 39 min |
| **Diseño** | Arquitectura, diagrama de clases, estándar de conteo, pseudocódigo | 18 % | 70 min |
| **Código** | Implementación de dominio, infraestructura, presentación y servicios | 32 % | 125 min |
| **Compilación** | Corrección de errores de sintaxis y tipado hasta 0 warnings / 0 errors | 5 % | 20 min |
| **Pruebas** | Diseño de casos de prueba xUnit, ejecución sobre Prog 1 y Prog 2 | 25 % | 98 min |
| **Postmortem** | Conteo real con la herramienta, registro de datos finales, formulario PIP | 10 % | 38 min |
| **Total** | — | **100 %** | **390 min (6.5 h)** |

---

## 4. Criterio de salida de la fase de Planificación

Para cerrar formalmente la fase de **Planificación**:
1. Los requisitos funcionales y no funcionales están documentados y consensuados (`requisitos.md`).
2. La estimación de tamaño y tiempo basada en PROBE está registrada (`estimacion.md`).
3. El tiempo invertido se registra en el cronómetro del **PSP Tracker**.
4. Se realiza el commit de Git para la fase: `planeacion: requisitos RF/RNF y estimacion PROBE`.
