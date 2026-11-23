# Diseño de Arquitectura y Componentes · Programa 2

Entradas: `docs/planeacion/requisitos.md` (RF-01 a RF-07, RNF-01 a RNF-07).  
Salidas: este documento, `pseudocodigo.md` y `estandar_codificacion.md`.

---

## 1. Arquitectura en capas

Siguiendo el mismo patrón del Programa 1, el sistema se organiza en 4 capas desacopladas:

```
┌───────────────────────────────────────────────┐
│               PRESENTACIÓN                    │
│   (ConsolaUI: interfaz de usuario y tabla)    │
└───────────────────────┬───────────────────────┘
                        │ usa
┌───────────────────────▼───────────────────────┐
│                APLICACIÓN                     │
│    (Aplicacion.cs: orquestador del flujo)     │
└───────────────┬───────────────────────────────┘
                │ coordina
        ┌───────┴───────────────────────┐
        │ usa                           │ usa
┌───────▼──────────────────┐   ┌────────▼──────────────┐
│     INFRAESTRUCTURA      │   │        DOMINIO        │
│ (LectorCodigoFuente:     │   │ (ParteInfo, Resumen,  │
│  disco, rutas, archivos) │   │  AnalizadorCodigo)    │
└──────────────────────────┘   └───────────────────────┘
```

| Capa | Componente / Archivo | Responsabilidad | Requisitos |
|---|---|---|---|
| **Presentación** | `Presentacion/ConsolaUI.cs` | Solicitar ruta, validar parámetros e imprimir la tabla formateada (estilo Tabla 1 SEI). | RF-06, RF-07, RNF-05 |
| **Aplicación** | `Aplicacion.cs` | Orquesta la lectura de archivos, invoca el analizador de líneas y entrega el resumen. | RF-01, RF-05, RNF-02 |
| **Infraestructura** | `Infraestructura/LectorCodigoFuente.cs` | Búsqueda de archivos `.cs` (individual o recursivo en carpeta) y lectura de líneas. | RF-01, RF-07 |
| **Dominio** | `Dominio/ParteInfo.cs` | Modela una parte analizada (Nombre, Cantidad de Métodos, LOC). | RF-03, RF-04, RF-05 |
| **Dominio** | `Dominio/ResumenConteo.cs` | Almacena la colección de partes y calcula el total de LOC general. | RF-05 |
| **Dominio** | `Dominio/AnalizadorCodigoService.cs` | Algoritmo principal: elimina comentarios, detecta clases/métodos y cuenta LOC efectivas. | RF-02, RF-03, RF-04 |

---

## 2. Diagrama de Clases (Estructura lógica)

```text
+-------------------------------------------------------------+
|                         ParteInfo                           |
+-------------------------------------------------------------+
| + Nombre: string                                            |
| + CantidadItems: int                                        |
| + TamanoLoc: int                                            |
+-------------------------------------------------------------+
| + IncrementarItems()                                        |
| + IncrementarLoc()                                          |
+-------------------------------------------------------------+

+-------------------------------------------------------------+
|                       ResumenConteo                         |
+-------------------------------------------------------------+
| + Partes: IReadOnlyList<ParteInfo>                          |
| + TotalLoc: int                                             |
| + TotalItems: int                                           |
+-------------------------------------------------------------+
| + AgregarParte(parte: ParteInfo)                            |
+-------------------------------------------------------------+

+-------------------------------------------------------------+
|                  AnalizadorCodigoService                    |
+-------------------------------------------------------------+
| + Analizar(nombreArchivo: string, lineas: IEnumerable<string>): ResumenConteo |
| - EsLineaValida(linea: string): bool                        |
| - EsInicioClase(linea: string, out nombre: string): bool    |
| - EsDeclaracionMetodo(linea: string): bool                  |
| - LimpiarComentariosBloque(lineas): List<string>            |
+-------------------------------------------------------------+
```

---

## 3. Formato de salida de consola (Diseño UI)

Cumpliendo con la **Tabla 1** del Kit de Asignación del SEI:

```text
================================================================================
                    PROGRAMA 2 - CONTADOR DE LÍNEAS DE CÓDIGO
================================================================================
Ruta analizada: C:\Proyectos\Programa1

Part Name                       Number of Items    Part Size       Total Size
--------------------------------------------------------------------------------
CalculadoraEstadistica          3                  32
DatosInsuficientesException     1                  8
LectorArchivoNumeros            4                  45
ListaEnlazada                   3                  28
Nodo                            1                  10
Aplicacion                      1                  14
--------------------------------------------------------------------------------
TOTAL DEL PROGRAMA              13                 137             137
================================================================================
```

---

## 4. Trazabilidad Requisito → Diseño → Prueba

| Requisito | Componente de Diseño | Caso de Prueba (xUnit) |
|---|---|---|
| RF-01 | `LectorCodigoFuente.ObtenerArchivos` | `LectorCodigoFuenteTests.LeeArchivoYDirectorio` |
| RF-02 | `AnalizadorCodigoService.LimpiarYFiltrar` | `AnalizadorTests.IgnoraComentariosYLineasVacias` |
| RF-03 | `AnalizadorCodigoService.DetectarClases` | `AnalizadorTests.DetectaClasesCorrectamente` |
| RF-04 | `AnalizadorCodigoService.DetectarMetodos` | `AnalizadorTests.CuentaMetodosEItems` |
| RF-05 | `ResumenConteo.TotalLoc` | `ResumenConteoTests.SumaTotalesCorrectamente` |
| RF-06 | `ConsolaUI.MostrarTabla` | `ConsolaUITests.FormateaTablaSegunSEI` |
| RF-07 | `Aplicacion.Ejecutar` | `AplicacionTests.RutasInvalidasMuestranError` |
