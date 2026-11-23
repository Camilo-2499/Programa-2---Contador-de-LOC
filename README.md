# PSP1 · Programa 2 — Contador de Líneas de Código (LOC)

Implementación del segundo programa bajo el proceso **PSP1** (Personal Software Process, SEI - Carnegie Mellon University).  
Desarrolla un contador de líneas de código lógicas (LOC) para proyectos C# (.NET 9), identificando **partes** (clases, interfaces, structs, records), **ítems** (métodos y constructores) y **tamaño total acumulado**.

---

## Ejecutar

Requisito: [.NET 9 / 10 / 11 SDK](https://dotnet.microsoft.com/download).

```bash
# Analizar el Programa 1 (prueba obligatoria 1 del SEI)
dotnet run --project src/Programa2.ContadorLOC -- "C:/ruta/al/Programa1.Estadistica"

# Analizarse a sí mismo (prueba obligatoria 2 del SEI)
dotnet run --project src/Programa2.ContadorLOC -- "src/Programa2.ContadorLOC"

# Si se ejecuta sin argumentos, solicita la ruta interactivamente por consola
dotnet run --project src/Programa2.ContadorLOC

# Ejecutar las 19 pruebas unitarias automatizadas (xUnit)
dotnet test
```

---

## Estructura del Proyecto

| Ruta | Fase PSP1 | Contenido |
|---|---|---|
| `docs/planeacion/` | **Planificación** | `requisitos.md` (RF-01 a RF-07, RNF), `estimacion.md` (PROBE, 195 LOC / 390 min). |
| `docs/diseno/` | **Diseño** | `diseno.md` (arquitectura 4 capas), `estandar_codificacion.md` (reglas de conteo LOC), `pseudocodigo.md`. |
| `src/Programa2.ContadorLOC/` | **Código** | Dominio (`ParteInfo`, `ResumenConteo`, `AnalizadorCodigoService`), Infraestructura (`LectorCodigoFuente`), Presentación (`ConsolaUI`) y `Aplicacion`. |
| Historial Git `compilacion:` | **Compilación** | Corrección de errores CS1503 y reglas CA1822 (0 advertencias, 0 errores). |
| `tests/`, `docs/pruebas/` | **Pruebas** | Suite xUnit (19 pruebas), `casos_prueba.md`, `informe_pruebas.md` y evidencias de salida. |
| `docs/postmortem/` | **Post-Mortem** | `resumen_pps.md` (Project Plan Summary), `pip.md` (PIP #2 con lecciones para Programa 3). |

---

## Resultados Clave

| Indicador | Planificado (PROBE) | Real Obtenido |
|---|---|---|
| **Tiempo de desarrollo** | 390 min (6.5 h) | ~119 min (~2.0 h) |
| **Tamaño Programa (`src/`)** | 195 LOC | **259 LOC** (8 partes, 26 ítems) |
| **Tamaño Pruebas (`tests/`)** | — | **265 LOC** (3 partes, 19 ítems) |
| **Defectos totales** | — | **2 defectos** (D1 en Compilación, D2 en Pruebas; 7 min de corrección) |
| **Densidad de defectos** | — | **7.7 def / KLOC** (reducción del 88 % frente al Programa 1) |
| **Pruebas automatizadas** | 19 casos | **19 / 19 aprobadas (100 %)** |

---

## Historial de Commits por Fase PSP

```text
planeacion:   requisitos RF/RNF y estimacion PROBE
diseno:       arquitectura, estandar de codificacion y conteo, pseudocodigo
codigo:       implementa capas Dominio, Infraestructura, Presentacion y Aplicacion
compilacion:  corrige CS1503 y CA1822; 0 errores y 0 advertencias
pruebas:      implementa suite xunit, corrige D2 y ejecuta pruebas sobre Prog 1 y Prog 2
postmortem:   resumen del plan (PPS), PIP #2 y metricas finales de proceso
```
