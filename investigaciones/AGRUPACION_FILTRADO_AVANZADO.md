# 📊 Agrupación y Filtrado Avanzado en SQL

---

## 🎯 Resumen de Conceptos

Este documento cubre:
1. **Funciones de Agregación** - Operaciones que resumen datos
2. **GROUP BY** - Cómo agrupar y procesar internamente
3. **HAVING vs WHERE** - El orden de ejecución de consultas

---

## 📈 PARTE 1: Funciones de Agregación

### ¿Qué son las funciones de agregación?

Son funciones que toman **múltiples filas** y retornan **un solo valor** (un resumen).

### Las 5 funciones principales:

#### 1️⃣ **COUNT()** - Contar filas
Cuenta cuántas filas cumplen una condición.

```sql
-- Contar TODOS los pedidos
SELECT COUNT(*) AS TotalPedidos
FROM Sales.SalesOrderHeader;
-- Resultado: 31465

-- Contar solo pedidos con estado específico
SELECT COUNT(SalesOrderID) AS PedidosValidos
FROM Sales.SalesOrderHeader
WHERE OrderDate >= '2012-01-01';
-- Resultado: 28614

-- ⚠️ IMPORTANTE: COUNT(columna) ignora NULLs
SELECT COUNT(SalesPersonID) AS PedidosConVendedor
FROM Sales.SalesOrderHeader;
-- Resultado: 28014 (algunos pedidos no tienen vendedor asignado)
```

---

#### 2️⃣ **SUM()** - Sumar valores
Suma todos los valores de una columna.

```sql
-- Sumar ventas totales de TODOS los pedidos
SELECT SUM(TotalDue) AS VentasTotales
FROM Sales.SalesOrderHeader;
-- Resultado: $109,846,381.39

-- Sumar solo del 2012
SELECT SUM(TotalDue) AS VentasDelAño2012
FROM Sales.SalesOrderHeader
WHERE YEAR(OrderDate) = 2012;
-- Resultado: $32,202,278.93

-- ⚠️ IMPORTANTE: SUM(columna) ignora NULLs
SELECT SUM(CAST(TotalDue AS DECIMAL(15,2))) AS Total
FROM Sales.SalesOrderHeader;
```

---

#### 3️⃣ **AVG()** - Promedio
Calcula el promedio de los valores.

```sql
-- Promedio de ventas por pedido
SELECT AVG(TotalDue) AS PromedioPorPedido
FROM Sales.SalesOrderHeader;
-- Resultado: $3,482.29

-- Promedio del precio unitario de los productos vendidos
SELECT AVG(UnitPrice) AS PrecioPromedio
FROM Sales.SalesOrderDetail;
-- Resultado: $432.66

-- ⚠️ AVG(columna) ignora NULLs, solo cuenta filas NO nulas
```

---

#### 4️⃣ **MAX()** - Valor máximo
Encuentra el mayor valor de una columna.

```sql
-- Pedido más grande en dinero
SELECT MAX(TotalDue) AS PedidoMasGrande
FROM Sales.SalesOrderHeader;
-- Resultado: $135,862.89

-- Producto más caro que se vendió
SELECT MAX(UnitPrice) AS PrecioMaximo
FROM Sales.SalesOrderDetail;
-- Resultado: $3,578.27

-- Fecha del último pedido
SELECT MAX(OrderDate) AS UltimoPedido
FROM Sales.SalesOrderHeader;
-- Resultado: 2014-06-30
```

---

#### 5️⃣ **MIN()** - Valor mínimo
Encuentra el menor valor de una columna.

```sql
-- Pedido más pequeño en dinero
SELECT MIN(TotalDue) AS PedidoMasChico
FROM Sales.SalesOrderHeader;
-- Resultado: $0.00 (¡algunos pedidos sin pago!)

-- Producto más barato que se vendió
SELECT MIN(UnitPrice) AS PrecioMinimo
FROM Sales.SalesOrderDetail;
-- Resultado: $0.00

-- Fecha del primer pedido
SELECT MIN(OrderDate) AS PrimerPedido
FROM Sales.SalesOrderHeader;
-- Resultado: 2011-05-31
```

---

### 📌 Usando múltiples agregaciones juntas

```sql
SELECT 
    COUNT(*) AS TotalPedidos,
    SUM(TotalDue) AS VentasTotales,
    AVG(TotalDue) AS VentaPromedio,
    MIN(TotalDue) AS VentaMinima,
    MAX(TotalDue) AS VentaMaxima
FROM Sales.SalesOrderHeader;
```

**Resultado:**
| TotalPedidos | VentasTotales | VentaPromedio | VentaMinima | VentaMaxima |
|---|---|---|---|---|
| 31465 | $109,846,381.39 | $3,482.29 | $0.00 | $135,862.89 |

---

---

## 🔗 PARTE 2: GROUP BY - Agrupación

### ¿Qué es GROUP BY?

**GROUP BY** divide las filas en **grupos** basados en los valores de una o más columnas, y luego aplica funciones de agregación a cada grupo.

### 🧠 Analogía del Mundo Real

Imagina que tienes un **registro de ventas** con muchas filas:

```
Vendedor      | Venta
Luis          | $100
María         | $150
Luis          | $200
Carlos        | $300
María         | $50
```

Sin GROUP BY, si haces SUM(Venta), obtienes: **$800** (todo sumado)

Con **GROUP BY Vendedor**, obtienes:
```
Vendedor | Total
Luis     | $300
María    | $200
Carlos   | $300
```

Ahora tienes un resultado **POR VENDEDOR**.

---

### Ejemplo 1: Ventas por vendedor

```sql
SELECT 
    sp.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS NombreVendedor,
    SUM(soh.TotalDue) AS VentasTotales,
    COUNT(soh.SalesOrderID) AS CantidadPedidos,
    AVG(soh.TotalDue) AS VentaPromedio
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
GROUP BY sp.BusinessEntityID, p.FirstName, p.LastName
ORDER BY VentasTotales DESC;
```

**Resultado:**
| BusinessEntityID | NombreVendedor | VentasTotales | CantidadPedidos | VentaPromedio |
|---|---|---|---|---|
| 276 | Linda Mitchell | $4,251,368.55 | 104 | $40,877.19 |
| 289 | Jae Pak | $4,116,871.23 | 102 | $40,360.30 |
| 275 | Michael Blythe | $3,763,178.18 | 99 | $38,062.81 |

---

### Ejemplo 2: Ventas por año

```sql
SELECT 
    YEAR(soh.OrderDate) AS Año,
    SUM(soh.TotalDue) AS VentasTotales,
    COUNT(DISTINCT soh.CustomerID) AS ClientesUnicos,
    AVG(soh.TotalDue) AS PromedioPorPedido
FROM Sales.SalesOrderHeader soh
GROUP BY YEAR(soh.OrderDate)
ORDER BY Año;
```

**Resultado:**
| Año | VentasTotales | ClientesUnicos | PromedioPorPedido |
|---|---|---|---|
| 2011 | $8,999,433.51 | 6 | $2,898.34 |
| 2012 | $32,202,278.93 | 450 | $3,751.45 |
| 2013 | $37,164,142.28 | 1120 | $3,945.87 |
| 2014 | $31,480,526.67 | 893 | $3,198.23 |

---

### Ejemplo 3: Agrupación por múltiples columnas

```sql
SELECT 
    YEAR(soh.OrderDate) AS Año,
    MONTH(soh.OrderDate) AS Mes,
    sp.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS Vendedor,
    COUNT(soh.SalesOrderID) AS PedidosDelMes,
    SUM(soh.TotalDue) AS VentasDelMes
FROM Sales.SalesOrderHeader soh
LEFT JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
LEFT JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
GROUP BY YEAR(soh.OrderDate), MONTH(soh.OrderDate), sp.BusinessEntityID, p.FirstName, p.LastName
ORDER BY Año, Mes, VentasDelMes DESC;
```

Esto te muestra **vendedor por mes por año**.

---

### 🧠 ¿Cómo procesa SQL GROUP BY internamente?

**Paso 1: FROM** - Lee todas las filas de la tabla
```
SalesOrderID | CustomerID | TotalDue | SalesPersonID
43793        | 11000      | $3,756.99| 276
43794        | 11001      | $2,499.50| 289
43795        | 11000      | $5,123.45| 276
...
```

**Paso 2: WHERE** - Filtra filas ANTES de agrupar
```
(si tuviéramos WHERE OrderDate >= '2012-01-01')
Solo 28,614 de las 31,465 filas continúan
```

**Paso 3: GROUP BY** - Agrupa filas por la columna especificada
```
Grupo 276 (Linda Mitchell):
  Pedido 43793 → $3,756.99
  Pedido 43795 → $5,123.45
  Pedido 43797 → $2,999.99
  ... (104 pedidos totales)

Grupo 289 (Jae Pak):
  Pedido 43794 → $2,499.50
  Pedido 43796 → $4,555.00
  ... (102 pedidos totales)
```

**Paso 4: SELECT con AGREGACIONES** - Aplica funciones a cada grupo
```
SELECT SUM(TotalDue) → Para grupo 276: $4,251,368.55
SELECT SUM(TotalDue) → Para grupo 289: $4,116,871.23
```

**Paso 5: ORDER BY** - Ordena los resultados finales

---

### ⚠️ REGLA IMPORTANTE de GROUP BY

**Regla: Todas las columnas en SELECT deben estar en GROUP BY o dentro de una función de agregación**

❌ INCORRECTO:
```sql
SELECT 
    sp.BusinessEntityID,
    p.FirstName,          -- ❌ No está en GROUP BY
    SUM(soh.TotalDue)
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
GROUP BY sp.BusinessEntityID;  -- ❌ FirstName no está aquí
```

✅ CORRECTO:
```sql
SELECT 
    sp.BusinessEntityID,
    p.FirstName,          -- ✅ Está en GROUP BY
    SUM(soh.TotalDue)
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
GROUP BY sp.BusinessEntityID, p.FirstName;  -- ✅ Ambas aquí
```

---

---

## 🎯 PARTE 3: HAVING vs WHERE - Orden de Ejecución

### El Problema: ¿Dónde filtramos?

Tienes estos datos de vendedores con 3 pedidos cada uno:

```
Vendedor  | Pedido1 | Pedido2 | Pedido3 | Total
Luis      | $100    | $200    | $300    | $600
María     | $50     | $100    | $75     | $225
Carlos    | $400    | $500    | $600    | $1,500
Ana       | $25     | $30     | $20     | $75
```

Quieres responder: **"¿Cuáles vendedores tienen ventas totales mayores a $500?"**

**¡PERO HAY DOS OPCIONES!**

---

### 🚀 El Orden de Ejecución de una Consulta SQL

SQL ejecuta las cláusulas en este orden (NO el orden en que las escribes):

```
1️⃣  FROM         → Lee las tablas
2️⃣  JOIN         → Une las tablas
3️⃣  WHERE        → Filtra FILAS INDIVIDUALES (ANTES de agrupar)
4️⃣  GROUP BY     → Agrupa las filas
5️⃣  HAVING       → Filtra GRUPOS (DESPUÉS de agrupar)
6️⃣  SELECT       → Selecciona las columnas finales
7️⃣  ORDER BY     → Ordena los resultados
8️⃣  LIMIT        → Limita el número de filas
```

---

### 📍 WHERE - Filtra ANTES de agrupar

**WHERE** actúa sobre **filas individuales** antes de que se creen los grupos.

**Ejemplo: "Dame la suma de ventas de pedidos mayores a $150 por vendedor"**

```sql
SELECT 
    Vendedor,
    SUM(TotalPedido) AS TotalVentas
FROM MisVentas
WHERE TotalPedido > 150    -- ⚡ Filtra FILAS INDIVIDUALES
GROUP BY Vendedor
ORDER BY TotalVentas DESC;
```

**Paso a paso:**
```
1. FROM MisVentas → Leo todas las filas
   Luis: $100, $200, $300
   María: $50, $100, $75
   Carlos: $400, $500, $600
   Ana: $25, $30, $20

2. WHERE TotalPedido > 150 → Filtro filas individuales
   Luis: $200, $300           (de $100, $200, $300)
   María: (ninguna)           (de $50, $100, $75)
   Carlos: $400, $500, $600   (de $400, $500, $600)
   Ana: (ninguna)             (de $25, $30, $20)

3. GROUP BY Vendedor → Agrupo lo que quedó
   Luis: [$200, $300]
   Carlos: [$400, $500, $600]

4. SELECT SUM() → Sumo cada grupo
   Luis:   $500
   Carlos: $1,500
```

---

### 📍 HAVING - Filtra DESPUÉS de agrupar

**HAVING** actúa sobre **grupos** después de que se han creado.

**Ejemplo: "Dame los vendedores cuya suma de ventas es mayor a $500"**

```sql
SELECT 
    Vendedor,
    SUM(TotalPedido) AS TotalVentas
FROM MisVentas
GROUP BY Vendedor
HAVING SUM(TotalPedido) > 500   -- ⚡ Filtra GRUPOS
ORDER BY TotalVentas DESC;
```

**Paso a paso:**
```
1. FROM MisVentas → Leo todas las filas
   Luis: $100, $200, $300
   María: $50, $100, $75
   Carlos: $400, $500, $600
   Ana: $25, $30, $20

2. GROUP BY Vendedor → Agrupo TODAS las filas
   Luis: [$100, $200, $300]
   María: [$50, $100, $75]
   Carlos: [$400, $500, $600]
   Ana: [$25, $30, $20]

3. HAVING SUM(TotalPedido) > 500 → Filtro grupos
   Luis: SUM = $600 ✅ (mayor que 500)
   María: SUM = $225 ❌ (menor que 500)
   Carlos: SUM = $1,500 ✅ (mayor que 500)
   Ana: SUM = $75 ❌ (menor que 500)

4. SELECT SUM() → Resultado
   Luis:   $600
   Carlos: $1,500
```

---

### 📊 Ejemplo Real con Adventure Works

#### WHERE - Filtra pedidos individuales, LUEGO agrupa

```sql
SELECT 
    sp.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS Vendedor,
    COUNT(soh.SalesOrderID) AS TotalPedidos,
    SUM(soh.TotalDue) AS TotalVentas,
    AVG(soh.TotalDue) AS VentaPromedio
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
WHERE soh.TotalDue > 5000  -- ⚡ Filtra ANTES: solo pedidos > $5000
GROUP BY sp.BusinessEntityID, p.FirstName, p.LastName
ORDER BY TotalVentas DESC;
```

**Resultado (ejemplo):**
```
Solo se cuentan pedidos > $5000 de cada vendedor
Linda Mitchell: 42 pedidos (de 104 totales)
Jae Pak: 38 pedidos (de 102 totales)
```

---

#### HAVING - Filtra grupos ya calculados

```sql
SELECT 
    sp.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS Vendedor,
    COUNT(soh.SalesOrderID) AS TotalPedidos,
    SUM(soh.TotalDue) AS TotalVentas,
    AVG(soh.TotalDue) AS VentaPromedio
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
GROUP BY sp.BusinessEntityID, p.FirstName, p.LastName
HAVING SUM(soh.TotalDue) > 3000000  -- ⚡ Filtra DESPUÉS: grupos con total > $3M
ORDER BY TotalVentas DESC;
```

**Resultado:**
```
Solo vendedores cuyas ventas TOTALES > $3,000,000
Linda Mitchell: 104 pedidos, $4,251,368.55
Jae Pak: 102 pedidos, $4,116,871.23
Michael Blythe: 99 pedidos, $3,763,178.18
(Otros vendedores con menos de $3M se filtran)
```

---

### 🎯 Usando WHERE y HAVING juntos

```sql
SELECT 
    YEAR(soh.OrderDate) AS Año,
    sp.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS Vendedor,
    COUNT(soh.SalesOrderID) AS TotalPedidos,
    SUM(soh.TotalDue) AS TotalVentas
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
WHERE soh.OrderDate >= '2012-01-01'   -- ⚡ Filtra FILAS: solo del 2012 en adelante
GROUP BY YEAR(soh.OrderDate), sp.BusinessEntityID, p.FirstName, p.LastName
HAVING SUM(soh.TotalDue) > 500000     -- ⚡ Filtra GRUPOS: vendedores > $500K por año
ORDER BY Año, TotalVentas DESC;
```

**¿Qué hace?**
1. **WHERE** → Usa solo pedidos del 2012 en adelante (filtra filas)
2. **GROUP BY** → Agrupa por año y vendedor
3. **HAVING** → Solo muestra vendedores que sumaron más de $500K por año (filtra grupos)

---

### 📊 Tabla Comparativa: WHERE vs HAVING

| Aspecto | WHERE | HAVING |
|--------|-------|--------|
| **Filtra** | Filas individuales | Grupos |
| **Momento** | ANTES de GROUP BY | DESPUÉS de GROUP BY |
| **Puede usar** | Nombres de columnas | Funciones de agregación (SUM, COUNT, etc.) |
| **Rápido** | ✅ Muy rápido (menos datos) | ⚠️ Más lento (calcula primero) |
| **Ejemplo** | `WHERE age > 30` | `HAVING COUNT(*) > 5` |

---

### ❌ ERRORES COMUNES

#### Error 1: Usar agregación en WHERE

```sql
❌ INCORRECTO:
SELECT Vendedor, SUM(Ventas) AS Total
FROM Ventas
WHERE SUM(Ventas) > 1000  -- ❌ ERROR: No puedes hacer agregación en WHERE
GROUP BY Vendedor;

✅ CORRECTO:
SELECT Vendedor, SUM(Ventas) AS Total
FROM Ventas
GROUP BY Vendedor
HAVING SUM(Ventas) > 1000;
```

---

#### Error 2: Confundir el orden de ejecución

```sql
❌ Consultarás con la lógica incorrecta:
SELECT 
    Vendedor,
    COUNT(*) AS Pedidos,
    SUM(Ventas) AS Total
FROM Ventas
WHERE SUM(Ventas) > 5000      -- ❌ Incorrecto lugar
GROUP BY Vendedor;

✅ Lógica correcta:
SELECT 
    Vendedor,
    COUNT(*) AS Pedidos,
    SUM(Ventas) AS Total
FROM Ventas
GROUP BY Vendedor
HAVING SUM(Ventas) > 5000;    -- ✅ Después de agrupar
```

---

---

## 🎓 RESUMEN VISUAL DEL FLUJO COMPLETO

```
┌─────────────────────────────────────────────────────────────┐
│                    CONSULTA SQL COMPLETA                    │
│                                                             │
│  SELECT col1, SUM(col2)                                    │
│  FROM tabla                                                │
│  WHERE col3 > 100              ← 3️⃣ Filtra FILAS antes   │
│  GROUP BY col1                 ← 4️⃣ Agrupa              │
│  HAVING SUM(col2) > 1000       ← 5️⃣ Filtra GRUPOS      │
│  ORDER BY SUM(col2) DESC       ← 7️⃣ Ordena              │
│  LIMIT 10;                     ← 8️⃣ Limita              │
│                                                             │
└─────────────────────────────────────────────────────────────┘

ORDEN DE EJECUCIÓN INTERNO:

1. FROM tabla
   ↓
2. (JOIN si existen)
   ↓
3. WHERE col3 > 100  ⚡ AQUÍ: Reduce de N filas a M filas
   ↓
4. GROUP BY col1     ⚡ AQUÍ: Agrupa las M filas en K grupos
   ↓
5. HAVING SUM(col2) > 1000  ⚡ AQUÍ: Reduce de K grupos a J grupos
   ↓
6. SELECT col1, SUM(col2)  ⚡ AQUÍ: Formatea y calcula resultado
   ↓
7. ORDER BY SUM(col2) DESC
   ↓
8. LIMIT 10  ⚡ AQUÍ: Retorna máximo 10 filas
```

---

## 🧪 Ejercicios Prácticos

### Ejercicio 1: Contador de productos por categoría
**Pregunta:** ¿Cuántos productos hay en cada categoría?

```sql
SELECT 
    pc.ProductCategoryID,
    pc.Name AS Categoria,
    COUNT(p.ProductID) AS TotalProductos
FROM Production.ProductCategory pc
LEFT JOIN Production.ProductSubcategory ps ON pc.ProductCategoryID = ps.ProductCategoryID
LEFT JOIN Production.Product p ON ps.ProductSubcategoryID = p.ProductSubcategoryID
GROUP BY pc.ProductCategoryID, pc.Name
ORDER BY TotalProductos DESC;
```

---

### Ejercicio 2: Vendedores con más de 50 pedidos
**Pregunta:** ¿Qué vendedores han hecho más de 50 pedidos?

```sql
SELECT 
    sp.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS Vendedor,
    COUNT(soh.SalesOrderID) AS TotalPedidos,
    SUM(soh.TotalDue) AS TotalVentas
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
GROUP BY sp.BusinessEntityID, p.FirstName, p.LastName
HAVING COUNT(soh.SalesOrderID) > 50
ORDER BY TotalPedidos DESC;
```

---

### Ejercicio 3: Clientes VIP (con pedidos > $10K)
**Pregunta:** ¿Cuáles clientes han gastado más de $10,000?

```sql
SELECT 
    soh.CustomerID,
    c.AccountNumber,
    COUNT(soh.SalesOrderID) AS TotalPedidos,
    SUM(soh.TotalDue) AS TotalGastado,
    AVG(soh.TotalDue) AS PromedioPorPedido
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.Customer c ON soh.CustomerID = c.CustomerID
GROUP BY soh.CustomerID, c.AccountNumber
HAVING SUM(soh.TotalDue) > 10000
ORDER BY TotalGastado DESC;
```

---

### Ejercicio 4: Combinando WHERE y HAVING
**Pregunta:** ¿Cuáles vendedores tuvieron más de $1M en ventas del 2013?

```sql
SELECT 
    sp.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS Vendedor,
    COUNT(soh.SalesOrderID) AS TotalPedidos,
    SUM(soh.TotalDue) AS TotalVentas
FROM Sales.SalesOrderHeader soh
INNER JOIN Sales.SalesPerson sp ON soh.SalesPersonID = sp.BusinessEntityID
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
WHERE YEAR(soh.OrderDate) = 2013    -- ⚡ Filtra por año ANTES
GROUP BY sp.BusinessEntityID, p.FirstName, p.LastName
HAVING SUM(soh.TotalDue) > 1000000  -- ⚡ Filtra por monto DESPUÉS
ORDER BY TotalVentas DESC;
```

---

## 📚 Conclusión

| Concepto | Punto Clave |
|----------|------------|
| **Agregaciones** | SUM, COUNT, AVG, MAX, MIN resumen múltiples filas en un valor |
| **GROUP BY** | Divide datos en grupos y aplica agregaciones a cada grupo |
| **WHERE** | Filtra **filas individuales** ANTES de agrupar (más rápido) |
| **HAVING** | Filtra **grupos** DESPUÉS de agrupar (necesario para agregaciones) |
| **Orden** | FROM → WHERE → GROUP BY → HAVING → SELECT → ORDER BY |

---

**¡Ahora entiendes cómo SQL agrupa, filtra y resume datos! 🚀**
