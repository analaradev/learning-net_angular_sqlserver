-- ==============================================
-- SUBCONSULTAS: EJEMPLOS PRÁCTICOS EJECUTABLES
-- Adventure Works 2022 Database
-- ==============================================

-- ============================================
-- EJEMPLO 1: SUBCONSULTA NO CORRELACIONADA
-- ============================================
-- Obtener vendedores con ventas mayores al promedio GLOBAL

-- Primero, veamos el promedio
SELECT AVG(SalesYTD) AS PromedioVentasGlobal
FROM Sales.SalesPerson;
-- Resultado: ~2,480,786.87

-- Ahora obtener vendedores arriba del promedio
SELECT 
    sp.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS NombreVendedor,
    sp.SalesYTD,
    (SELECT AVG(SalesYTD) FROM Sales.SalesPerson) AS PromedioGlobal,
    sp.SalesYTD - (SELECT AVG(SalesYTD) FROM Sales.SalesPerson) AS DiferenciaAlPromedio
FROM Sales.SalesPerson sp
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
WHERE sp.SalesYTD > (
    SELECT AVG(SalesYTD)  -- Se ejecuta UNA SOLA VEZ
    FROM Sales.SalesPerson
)
ORDER BY sp.SalesYTD DESC;


-- ============================================
-- EJEMPLO 2: SUBCONSULTA CORRELACIONADA
-- ============================================
-- Obtener órdenes MAYORES AL PROMEDIO DE ESE CLIENTE

-- Primero, ver el promedio por cliente (para entender)
SELECT 
    CustomerID,
    COUNT(*) AS NumeroOrdenes,
    AVG(TotalDue) AS PromedioOrdenes,
    MIN(TotalDue) AS OrdenMinima,
    MAX(TotalDue) AS OrdenMaxima
FROM Sales.SalesOrderHeader
GROUP BY CustomerID
ORDER BY AVG(TotalDue) DESC;

-- Ahora obtener órdenes superiores al promedio del cliente
SELECT 
    soh.SalesOrderID,
    soh.CustomerID,
    soh.TotalDue,
    soh.OrderDate,
    (SELECT AVG(soh2.TotalDue)
     FROM Sales.SalesOrderHeader soh2
     WHERE soh2.CustomerID = soh.CustomerID) AS PromedioClienteActual
FROM Sales.SalesOrderHeader soh
WHERE soh.TotalDue > (
    SELECT AVG(soh2.TotalDue)
    FROM Sales.SalesOrderHeader soh2
    WHERE soh2.CustomerID = soh.CustomerID  -- ⚠️ CORRELACIÓN: se ejecuta para cada fila
)
ORDER BY soh.CustomerID, soh.TotalDue DESC
OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY;


-- ============================================
-- EJEMPLO 3: OPCIÓN FAST - USANDO JOIN
-- ============================================
-- Mismo resultado que Ejemplo 2 pero MÁS RÁPIDO

SELECT 
    soh.SalesOrderID,
    soh.CustomerID,
    soh.TotalDue,
    soh.OrderDate,
    prom.PromedioClienteActual,
    soh.TotalDue - prom.PromedioClienteActual AS DiferenciaAlPromedio
FROM Sales.SalesOrderHeader soh
INNER JOIN (
    SELECT 
        CustomerID, 
        AVG(TotalDue) AS PromedioClienteActual
    FROM Sales.SalesOrderHeader
    GROUP BY CustomerID
) prom ON soh.CustomerID = prom.CustomerID 
    AND soh.TotalDue > prom.PromedioClienteActual
ORDER BY soh.CustomerID, soh.TotalDue DESC
OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY;


-- ============================================
-- EJEMPLO 4: USANDO CTE (RECOMENDADO)
-- ============================================
-- Más legible y mantenible

WITH PromedioPorCliente AS (
    SELECT 
        CustomerID, 
        AVG(TotalDue) AS PromedioCliente,
        COUNT(*) AS NumeroOrdenes,
        MIN(TotalDue) AS MinOrdenes,
        MAX(TotalDue) AS MaxOrdenes
    FROM Sales.SalesOrderHeader
    GROUP BY CustomerID
)
SELECT 
    soh.SalesOrderID,
    soh.CustomerID,
    soh.TotalDue,
    soh.OrderDate,
    ppc.PromedioCliente,
    ppc.NumeroOrdenes,
    soh.TotalDue - ppc.PromedioCliente AS DiferenciaAlPromedio
FROM Sales.SalesOrderHeader soh
INNER JOIN PromedioPorCliente ppc 
    ON soh.CustomerID = ppc.CustomerID
    AND soh.TotalDue > ppc.PromedioCliente
ORDER BY soh.CustomerID, soh.TotalDue DESC
OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY;


-- ============================================
-- EJEMPLO 5: WINDOW FUNCTIONS (MODERNO)
-- ============================================
-- Forma contemporánea de hacer esto

SELECT 
    soh.SalesOrderID,
    soh.CustomerID,
    soh.TotalDue,
    soh.OrderDate,
    AVG(soh.TotalDue) OVER (PARTITION BY soh.CustomerID) AS PromedioClienteActual,
    soh.TotalDue - AVG(soh.TotalDue) OVER (PARTITION BY soh.CustomerID) AS DiferenciaAlPromedio,
    ROW_NUMBER() OVER (PARTITION BY soh.CustomerID ORDER BY soh.TotalDue DESC) AS RangoOrdenesPorCliente
FROM Sales.SalesOrderHeader soh
WHERE soh.TotalDue > (
    SELECT AVG(soh2.TotalDue)
    FROM Sales.SalesOrderHeader soh2
    WHERE soh2.CustomerID = soh.CustomerID
)
ORDER BY soh.CustomerID, soh.TotalDue DESC
OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY;


-- ============================================
-- EJEMPLO 6: PRODUCTOS CAROS POR SUBCATEGORÍA
-- ============================================

-- VERSIÓN CON SUBCONSULTA CORRELACIONADA
SELECT 
    p.ProductID,
    p.Name,
    p.ListPrice,
    p.ProductSubcategoryID,
    (SELECT AVG(p2.ListPrice)
     FROM Production.Product p2
     WHERE p2.ProductSubcategoryID = p.ProductSubcategoryID
        AND p2.ProductSubcategoryID IS NOT NULL
    ) AS PromedioSubcategoria
FROM Production.Product p
WHERE p.ListPrice > (
    SELECT AVG(p2.ListPrice)
    FROM Production.Product p2
    WHERE p2.ProductSubcategoryID = p.ProductSubcategoryID
        AND p2.ProductSubcategoryID IS NOT NULL
)
    AND p.ProductSubcategoryID IS NOT NULL
ORDER BY p.ProductSubcategoryID, p.ListPrice DESC;

-- VERSIÓN CON JOIN (MÁS RÁPIDA) ✅
SELECT 
    p.ProductID,
    p.Name,
    p.ListPrice,
    p.ProductSubcategoryID,
    psc_avg.PromedioSubcategoria,
    p.ListPrice - psc_avg.PromedioSubcategoria AS DiferenciaAlPromedio
FROM Production.Product p
INNER JOIN (
    SELECT 
        ProductSubcategoryID,
        AVG(ListPrice) AS PromedioSubcategoria
    FROM Production.Product
    WHERE ProductSubcategoryID IS NOT NULL
    GROUP BY ProductSubcategoryID
) psc_avg ON p.ProductSubcategoryID = psc_avg.ProductSubcategoryID
    AND p.ListPrice > psc_avg.PromedioSubcategoria
ORDER BY p.ProductSubcategoryID, p.ListPrice DESC;


-- ============================================
-- EJEMPLO 7: CLIENTES CON MUCHAS ÓRDENES
-- ============================================

-- Clientes con más órdenes que el promedio
SELECT 
    c.CustomerID,
    s.Name AS NombreTienda,
    COUNT(soh.SalesOrderID) AS NumeroOrdenes
FROM Sales.Customer c
LEFT JOIN Sales.Store s ON c.StoreID = s.BusinessEntityID
LEFT JOIN Sales.SalesOrderHeader soh ON c.CustomerID = soh.CustomerID
GROUP BY c.CustomerID, s.Name
HAVING COUNT(soh.SalesOrderID) > (
    SELECT AVG(OrdenesCliente)
    FROM (
        SELECT COUNT(SalesOrderID) AS OrdenesCliente
        FROM Sales.SalesOrderHeader
        GROUP BY CustomerID
    ) subquery
)
ORDER BY NumeroOrdenes DESC;


-- ============================================
-- EJEMPLO 8: IN CON SUBCONSULTA NO CORRELACIONADA
-- ============================================

-- Obtener órdenes de clientes que compraron en un territorio específico
SELECT DISTINCT soh.SalesOrderID, soh.CustomerID, soh.TotalDue
FROM Sales.SalesOrderHeader soh
WHERE soh.CustomerID IN (
    SELECT DISTINCT c.CustomerID
    FROM Sales.Customer c
    WHERE c.TerritoryID IN (
        SELECT TerritoryID
        FROM Sales.SalesTerritory
        WHERE [Group] = 'North America'
    )
)
ORDER BY soh.CustomerID
OFFSET 0 ROWS FETCH NEXT 30 ROWS ONLY;


-- ============================================
-- EJEMPLO 9: NOT IN CON SUBCONSULTA
-- ============================================

-- Obtener productos que NUNCA han sido ordenados
SELECT 
    p.ProductID,
    p.Name,
    p.ListPrice
FROM Production.Product p
WHERE p.ProductID NOT IN (
    SELECT DISTINCT ProductID
    FROM Sales.SalesOrderDetail
)
ORDER BY p.ProductID;


-- ============================================
-- EJEMPLO 10: EXISTS vs IN (RENDIMIENTO)
-- ============================================

-- OPCIÓN 1: Usar IN (busca toda la lista)
SELECT DISTINCT c.CustomerID, s.Name
FROM Sales.Customer c
LEFT JOIN Sales.Store s ON c.StoreID = s.BusinessEntityID
WHERE c.CustomerID IN (
    SELECT CustomerID
    FROM Sales.SalesOrderHeader
    WHERE TotalDue > 5000
);

-- OPCIÓN 2: Usar EXISTS (más eficiente para listas grandes)
SELECT DISTINCT c.CustomerID, s.Name
FROM Sales.Customer c
LEFT JOIN Sales.Store s ON c.StoreID = s.BusinessEntityID
WHERE EXISTS (
    SELECT 1
    FROM Sales.SalesOrderHeader soh
    WHERE soh.CustomerID = c.CustomerID
        AND soh.TotalDue > 5000
);


-- ============================================
-- COMPARACIÓN DE RENDIMIENTO
-- ============================================
-- Ejecuta estas 3 queries y compara los tiempos:

-- SET STATISTICS IO ON;  -- Descomenta para ver el costo de I/O

-- Query 1: LENTA - Subconsulta correlacionada
SELECT 
    soh.SalesOrderID,
    soh.CustomerID,
    soh.TotalDue
FROM Sales.SalesOrderHeader soh
WHERE soh.TotalDue > (
    SELECT AVG(soh2.TotalDue)
    FROM Sales.SalesOrderHeader soh2
    WHERE soh2.CustomerID = soh.CustomerID
)
OFFSET 0 ROWS FETCH NEXT 1000 ROWS ONLY;

-- Query 2: RÁPIDA - JOIN
SELECT 
    soh.SalesOrderID,
    soh.CustomerID,
    soh.TotalDue
FROM Sales.SalesOrderHeader soh
INNER JOIN (
    SELECT CustomerID, AVG(TotalDue) AS Promedio
    FROM Sales.SalesOrderHeader
    GROUP BY CustomerID
) prom ON soh.CustomerID = prom.CustomerID 
    AND soh.TotalDue > prom.Promedio
OFFSET 0 ROWS FETCH NEXT 1000 ROWS ONLY;

-- Query 3: RÁPIDA - CTE
WITH PromedioCliente AS (
    SELECT CustomerID, AVG(TotalDue) AS Promedio
    FROM Sales.SalesOrderHeader
    GROUP BY CustomerID
)
SELECT 
    soh.SalesOrderID,
    soh.CustomerID,
    soh.TotalDue
FROM Sales.SalesOrderHeader soh
INNER JOIN PromedioCliente pc ON soh.CustomerID = pc.CustomerID 
    AND soh.TotalDue > pc.Promedio
OFFSET 0 ROWS FETCH NEXT 1000 ROWS ONLY;

-- SET STATISTICS IO OFF;


-- ============================================
-- RESUMEN DE MEJORES PRÁCTICAS
-- ============================================

/*
✅ DEBERÍAS USAR:

1. Subconsultas NO correlacionadas cuando:
   - Necesitas un valor global (MAX, MIN, AVG, COUNT)
   - Se ejecuta una sola vez
   - Es simple y legible

2. JOINs cuando:
   - Necesitas combinar datos de múltiples tablas
   - Tienes grupos o comparaciones por grupo
   - Importa el rendimiento

3. CTEs cuando:
   - La lógica es compleja
   - Necesitas múltiples niveles de agregación
   - Quieres mejor legibilidad

4. Window Functions cuando:
   - Necesitas análisis avanzado
   - Quieres rangos, totales acumulativos, etc.
   - Requieres rendimiento y poder

❌ DEBERÍAS EVITAR:

- Subconsultas correlacionadas con >100k filas
- Subconsultas en el WHERE cuando hay alternativas de JOIN
- IN con subconsultas grandes (usa EXISTS en su lugar)
- Anidamiento profundo de subconsultas

*/
