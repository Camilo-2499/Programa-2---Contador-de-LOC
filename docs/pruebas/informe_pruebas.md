# Pruebas · Informe de Pruebas (Test Report Template)

Proyecto: **Programa 2 — Contador de LOC**  
Nivel: **PSP1 / PSP0.1**  
Fecha de ejecución: **23/11/2026**

---

## 1. Resumen de ejecución

* **Total de casos de prueba automatizados (xUnit):** 19
* **Pruebas aprobadas:** 19 (100 %)
* **Pruebas fallidas:** 0
* **Tiempo total de ejecución xUnit:** 84 ms

---

## 2. Defectos encontrados en la fase de pruebas

Durante la primera corrida de la suite de pruebas unitarias, se detectó 1 defecto lógico que afectó 4 casos de prueba antes de ser corregido:

* **Defecto D2 (Lógica / Función - Tipo 80):**  
  * *Descripción:* Las líneas que contenían exclusivamente llaves (`{` o `}`) eran filtradas por `EsLineaNoContable` antes de actualizar el acumulador `nivelLlavesClase`. Esto impedía que el balance de llaves alcanzara 1 o regresara a 0 para cerrar clases y detectar métodos.
  * *Fase inyectado:* Código
  * *Fase removido:* Pruebas
  * *Tiempo de corrección:* 5 minutos
  * *Acción correctiva:* Se reestructuró el flujo para que `diffLlaves` se calcule en cada línea antes del descarte de LOC, permitiendo rastrear el bloque de clase de manera exacta.

---

## 3. Pruebas obligatorias del Kit SEI (Resultados Reales)

### Prueba Obligatoria 1: Conteo del Programa 1 (Media y Desviación)
* **Ruta:** `PSP\Programa 1 Media y Desciasion estandar\psp0-programa1-media-desviacion\src\Programa1.Estadistica`

```text
Part Name                        Number of Items    Part Size       Total Size  
--------------------------------------------------------------------------------
Aplicacion                       1                  15             
CalculadoraEstadistica           3                  24             
DatosInsuficientesException      2                  3              
ListaEnlazada                    3                  17             
Nodo                             1                  4              
ResultadoEstadistico             0                  1              
DatosInvalidosException          2                  4              
LectorArchivoNumeros             3                  25             
ConsolaUI                        6                  24             
Program                          1                  2              
--------------------------------------------------------------------------------
TOTAL DEL PROGRAMA               22                 119             119         
================================================================================
```

### Prueba Obligatoria 2: Conteo del Programa 2 (Autocirugía)
* **Ruta:** `Programa 2 - Contador de LOC\src\Programa2.ContadorLOC`

```text
Part Name                        Number of Items    Part Size       Total Size  
--------------------------------------------------------------------------------
Aplicacion                       1                  18             
AnalizadorCodigoService          7                  150            
ParteInfo                        4                  16             
ResumenConteo                    4                  14             
ArchivoNoValidoException         2                  3              
LectorCodigoFuente               2                  27             
ConsolaUI                        5                  29             
Program                          1                  2              
--------------------------------------------------------------------------------
TOTAL DEL PROGRAMA               26                 259             259         
================================================================================
```

---

## 4. Conclusión

El programa cumple con el 100% de los Requisitos Funcionales y No Funcionales, y genera la salida tabular exacta especificada en la Tabla 1 del SEI Assignment Kit.
