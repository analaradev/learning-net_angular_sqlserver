-- SIN INDICES
SELECT * FROM Sales.SalesOrderDetail 
WHERE UnitPrice > 2000;

-- Ver qué índices tiene SalesOrderDetail
SELECT name, type_desc, is_primary_key
FROM sys.indexes 
WHERE OBJECT_NAME(object_id) = 'SalesOrderDetail'
ORDER BY type_desc;


-- ACTIVAR contador de tiempo
SET STATISTICS TIME ON;

-- Búsqueda SIN ÍNDICE
SELECT * FROM Sales.SalesOrderDetail 
WHERE UnitPrice > 2000;

-- DESACTIVAR contador
SET STATISTICS TIME OFF;

--TIEMPO DE EJECUCION SIN INDICE DE PRECIO UNITARIO

--SQL Server parse and compile time: 
--   CPU time = 0 ms, elapsed time = 0 ms.

--(10500 filas afectadas)

-- SQL Server Execution Times:
--   CPU time = 46 ms,  elapsed time = 271 ms.

--Hora de finalización: 2026-03-18T17:53:57.2267269-06:00

-- CREAR ÍNDICE
CREATE INDEX IX_SalesOrderDetail_UnitPrice
ON Sales.SalesOrderDetail (UnitPrice ASC);

-- Verificar que el índice se creó
SELECT name, type_desc
FROM sys.indexes 
WHERE OBJECT_NAME(object_id) = 'SalesOrderDetail'
ORDER BY type_desc;

SET STATISTICS TIME ON;

-- MISMA BÚSQUEDA, pero ahora COM ÍNDICE
SELECT * FROM Sales.SalesOrderDetail 
WHERE UnitPrice > 2000;

-- DESACTIVAR contador
SET STATISTICS TIME OFF;
