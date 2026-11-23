# Postmortem · Resumen del Plan del Proyecto (PSP1 Project Plan Summary)

| Campo | Valor |
|---|---|
| Estudiante | Jhojan Camilo |
| Programa | 2 · Contador de Líneas de Código (LOC) |
| Nivel / lenguaje | PSP1 / PSP0.1 · C# (.NET 9) |
| Fecha | 23 de noviembre de 2026 |
| Entorno | Windows 11 · Visual Studio / .NET 9 SDK |

---

## 1. Tiempo en fase (minutos)

| Fase PSP1 | Plan (minutos) | Real (minutos) | % del Real | Diferencia (Real - Plan) |
|---|---:|---:|---:|---:|
| **Planificación** | 39 | 19 | 16.0 % | -20 |
| **Diseño** | 70 | 27 | 22.7 % | -43 |
| **Código** | 125 | 27 | 22.7 % | -98 |
| **Compilación** | 20 | 6 | 5.0 % | -14 |
| **Pruebas** | 98 | 25 | 21.0 % | -73 |
| **Post-Mortem** | 38 | 15 | 12.6 % | -23 |
| **Total** | **390** | **119** | **100 %** | **-271** |

> **Análisis del tiempo:** El tiempo real total fue de aproximadamente 2 horas (119 min). La reducción frente al plan se debió a que la fase de diseño blindó por completo el pseudocódigo y los casos límite, permitiendo una codificación directa y sin vacilaciones, con solo 2 defectos en todo el ciclo.

---

## 2. Tamaño del programa (medido con nuestro propio Contador de LOC)

| Componente | Partes | Ítems (Métodos) | LOC Plan | LOC Real | Variación |
|---|---:|---:|---:|---:|---:|
| **Programa principal (`src/`)** | 8 partes | 26 ítems | 195 | 259 | +32.8 % |
| **Suite de pruebas (`tests/`)** | 3 partes | 19 ítems | — | 265 | — |
| **Total nuevo desarrollado** | **11 partes** | **45 ítems** | **195** | **524** | — |

* **Productividad neta (código de producción):** 259 LOC / 1.98 h = **130.8 LOC / hora**.
* **Productividad total (incluyendo pruebas):** 524 LOC / 1.98 h = **264.6 LOC / hora**.

---

## 3. Resumen de Defectos

| # | Fase Inyección | Fase Remoción | Tipo Defecto | Min Corrección | Descripción |
|---|---|---|---|---:|---|
| **D1** | Código | Compilación | 20 (Sintaxis) | 2 min | `CS1503` en sobrecarga de `IndexOf` y `CA1822` para miembros estáticos. |
| **D2** | Código | Pruebas | 80 (Función/Lógica) | 5 min | Balance de llaves no se acumulaba en líneas que solo contenían llaves solas `{` o `}`. |
| **Total** | — | — | — | **7 min** | **2 defectos totales corregidos** |

* **Densidad de defectos:** 2 defectos / 0.259 KLOC = **7.7 defectos / KLOC** (una mejora radical frente a los 65.7 def/KLOC del Programa 1).

---

## 4. Verificación de consistencia y entrega (Checklist SEI)

- [x] El programa fue desarrollado siguiendo estrictamente las fases de PSP1.
- [x] Se cuenta el código del **Programa 1** (119 LOC, 10 partes, 22 ítems).
- [x] Se cuenta a sí mismo (**Programa 2**, 259 LOC, 8 partes, 26 ítems).
- [x] La suite de pruebas unitarias xUnit ejecuta 19 pruebas con 100% de éxito.
- [x] El estándar de conteo y codificación se documentó y respetó.
- [x] Los tiempos de corrección de defectos están dentro del tiempo de la fase correspondiente.
- [x] Hay un commit de Git al cierre de cada fase PSP.
