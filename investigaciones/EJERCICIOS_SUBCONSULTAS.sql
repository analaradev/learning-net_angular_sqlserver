-- ============================================
-- EJERCICIOS DE SUBCONSULTAS
-- Adventure Works 2022
-- ============================================

-- NIVEL 1: FÁCIL (Subconsultas NO Correlacionadas)
-- ============================================

-- EJERCICIO 1.1:
-- Encuentra todos los productos cuyo precio es mayor que el promedio de precios
-- Tablas: Production.Product
-- Ayuda: Usa AVG en la subconsulta

-- ESCRIBE AQUÍ:
-- SELECT ...

    USE adventure_works;

    SELECT * FROM Production.Product
    WHERE ListPrice > (SELECT AVG(ListPrice) FROM Production.Product)
   
-- EJERCICIO 1.2:
-- Encuentra todos los clientes que han hecho al menos una orden
-- Tablas: Sales.Customer, Sales.SalesOrderHeader
-- Ayuda: Usa IN con una subconsulta que retorne CustomerID

-- ESCRIBE AQUÍ:
-- SELECT ...

    Select CustomerID, PersonID  from Sales.Customer
    where CustomerID in (Select CustomerID from Sales.SalesOrderHeader)


-- EJERCICIO 1.3:
-- ¿Cuántas órdenes hay? Luego, encuentra órdenes que cuesten más que el promedio
-- Tablas: Sales.SalesOrderHeader
-- Primero: SELECT COUNT(*) FROM Sales.SalesOrderHeader
-- Segundo: Usa AVG para encontrar órdenes superiores al promedio

-- ESCRIBE AQUÍ:
-- SELECT ...
    SELECT COUNT(*) FROM Sales.SalesOrderHeader

    select TotalDue FROM Sales.SalesOrderHeader
    where TotalDue > (select avg(TotalDue) from Sales.SalesOrderHeader)

-- EJERCICIO 1.4:
-- Encuentra el producto más caro (MAX) y luego busca productos que cuesten MENOS que el máximo
-- Tablas: Production.Product
-- Ayuda: SELECT MAX(ListPrice) FROM Production.Product

-- ESCRIBE AQUÍ:
-- SELECT ...
    select Name, ListPrice  from Production.Product
    where ListPrice  <  (select MAX(ListPrice) from Production.Product)
    ORDER BY ListPrice DESC


-- EJERCICIO 1.5:
-- Encuentra vendedores cuyo SalesYTD es mayor que el promedio de todos los vendedores
-- Tablas: Sales.SalesPerson, Person.Person
-- Ayuda: Usa Sales.SalesPerson

-- ESCRIBE AQUÍ:
-- SELECT ...

    select * from Sales.SalesPerson 
    where SalesYTD > ( select avg(SalesYTD) from Sales.SalesPerson)


-- ============================================
-- NIVEL 2: INTERMEDIO (Mezcla de tipos)
-- ============================================

-- EJERCICIO 2.1:
-- Encuentra órdenes que cuesten MÁS que la orden más cara del cliente 11000
-- Tablas: Sales.SalesOrderHeader
-- Ayuda: 
-- - Primero: SELECT MAX(TotalDue) FROM Sales.SalesOrderHeader WHERE CustomerID = 11000
-- - Luego: Usa eso en una subconsulta para encontrar órdenes mayores

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 2.2:
-- Encuentra clientes que han hecho más órdenes que el promedio de órdenes por cliente
-- Tablas: Sales.Customer, Sales.SalesOrderHeader, Person.Person
-- Ayuda: 
-- - Calcula cuántas órdenes tiene cada cliente
-- - Luego compara con el promedio de órdenes por cliente
-- - Usa COUNT() en la subconsulta

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 2.3:
-- Encuentra productos que NUNCA han sido vendidos
-- Tablas: Production.Product, Sales.SalesOrderDetail
-- Ayuda: Usa NOT IN con una subconsulta que retorne ProductIDs vendidos

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 2.4:
-- Encuentra órdenes del cliente 11000 que cuesten más que su promedio personal
-- Tablas: Sales.SalesOrderHeader
-- Ayuda: Esta es una subconsulta CORRELACIONADA
-- - Para cada orden del cliente 11000
-- - Compárala con el promedio de TODAS sus órdenes

-- ESCRIBE AQUÍ:
-- SELECT ...


-- ============================================
-- NIVEL 3: DIFÍCIL (Subconsultas Correlacionadas)
-- ============================================

-- EJERCICIO 3.1:
-- Encuentra empleados cuyo salario (Rate en EmployeePayHistory) es mayor que el promedio de su departamento
-- Tablas: HumanResources.Employee, HumanResources.EmployeePayHistory, HumanResources.EmployeeDepartmentHistory, Person.Person
-- Ayuda: Subconsulta correlacionada
-- - Correlaciona por DepartmentID

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 3.2:
-- Encuentra órdenes de cada cliente que sean MAYORES que el promedio de TODAS sus órdenes
-- Tablas: Sales.SalesOrderHeader
-- Ayuda: 
-- - Subconsulta correlacionada con CustomerID
-- - Esta es la que ya vimos en clase

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 3.3:
-- Encuentra productos que cuesten más que el promedio de SU PROPIA subcategoría
-- Tablas: Production.Product
-- Ayuda:
-- - Correlaciona por ProductSubcategoryID
-- - Algunos productos NO tienen subcategoría (NULL), filtra esos

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 3.4:
-- Encuentra clientes que hayan hecho al menos una orden superior al promedio GLOBAL de órdenes
-- Tablas: Sales.Customer, Sales.SalesOrderHeader
-- Ayuda:
-- - Primero calcula el promedio global de TotalDue
-- - Luego busca clientes que tengan al menos una orden mayor a eso
-- - Usa EXISTS

-- ESCRIBE AQUÍ:
-- SELECT ...


-- ============================================
-- NIVEL 4: MUY DIFÍCIL (Subconsultas anidadas)
-- ============================================

-- EJERCICIO 4.1:
-- Encuentra productos cuyo precio está en el rango entre el promedio y el máximo
-- Tablas: Production.Product
-- Ayuda:
-- - WHERE ListPrice > (SELECT AVG(ListPrice) FROM Production.Product)
-- - AND ListPrice < (SELECT MAX(ListPrice) FROM Production.Product)
-- - Necesitas DOS subconsultas

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 4.2:
-- Encuentra vendedores que venden más que el promedio PERO MENOS que el máximo
-- Tablas: Sales.SalesPerson, Person.Person
-- Ayuda: Dos subconsultas (AVG y MAX)

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 4.3:
-- Encuentra órdenes del cliente con MAYOR cantidad de órdenes
-- Tablas: Sales.SalesOrderHeader
-- Ayuda:
-- - Primero: Encuentra cuál cliente tiene más órdenes
-- - Luego: Usa ese CustomerID en una subconsulta para obtener sus órdenes
-- - Esto requiere múltiples niveles de subconsultas

-- ESCRIBE AQUÍ:
-- SELECT ...


-- EJERCICIO 4.4 (EXTRA):
-- Encuentra la categoría de producto con mayor precio promedio, 
-- luego obtén todos los productos de esa categoría que estén arriba del promedio de su categoría
-- Tablas: Production.Product, Production.ProductSubcategory, Production.ProductCategory
-- Ayuda: Subconsultas anidadas con INNER JOIN

-- ESCRIBE AQUÍ:
-- SELECT ...


-- ============================================
-- RESPUESTAS (Descomenta para ver las soluciones)
-- ============================================

/*

-- EJERCICIO 1.1 - SOLUCIÓN
SELECT ProductID, Name, ListPrice
FROM Production.Product
WHERE ListPrice > (SELECT AVG(ListPrice) FROM Production.Product)
ORDER BY ListPrice DESC;

-- EJERCICIO 1.2 - SOLUCIÓN
SELECT c.CustomerID, c.PersonID, c.StoreID
FROM Sales.Customer c
WHERE c.CustomerID IN (SELECT CustomerID FROM Sales.SalesOrderHeader)
ORDER BY c.CustomerID;

-- EJERCICIO 1.3 - SOLUCIÓN
SELECT SalesOrderID, CustomerID, TotalDue, OrderDate
FROM Sales.SalesOrderHeader
WHERE TotalDue > (SELECT AVG(TotalDue) FROM Sales.SalesOrderHeader)
ORDER BY TotalDue DESC;

-- EJERCICIO 1.4 - SOLUCIÓN
SELECT ProductID, Name, ListPrice
FROM Production.Product
WHERE ListPrice < (SELECT MAX(ListPrice) FROM Production.Product)
ORDER BY ListPrice DESC;

-- EJERCICIO 1.5 - SOLUCIÓN
SELECT sp.BusinessEntityID, p.FirstName + ' ' + p.LastName AS Nombre, sp.SalesYTD
FROM Sales.SalesPerson sp
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
WHERE sp.SalesYTD > (SELECT AVG(SalesYTD) FROM Sales.SalesPerson)
ORDER BY sp.SalesYTD DESC;

-- EJERCICIO 2.1 - SOLUCIÓN
SELECT SalesOrderID, CustomerID, TotalDue
FROM Sales.SalesOrderHeader
WHERE TotalDue > (
    SELECT MAX(TotalDue) FROM Sales.SalesOrderHeader 
    WHERE CustomerID = 11000
)
ORDER BY TotalDue DESC;

-- EJERCICIO 2.2 - SOLUCIÓN
SELECT c.CustomerID, p.FirstName + ' ' + p.LastName AS Nombre, COUNT(soh.SalesOrderID) AS NumeroOrdenes
FROM Sales.Customer c
LEFT JOIN Sales.SalesOrderHeader soh ON c.CustomerID = soh.CustomerID
LEFT JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
GROUP BY c.CustomerID, p.FirstName, p.LastName
HAVING COUNT(soh.SalesOrderID) > (
    SELECT AVG(OrdenesCliente)
    FROM (
        SELECT COUNT(SalesOrderID) AS OrdenesCliente
        FROM Sales.SalesOrderHeader
        GROUP BY CustomerID
    ) subquery
)
ORDER BY NumeroOrdenes DESC;

-- EJERCICIO 2.3 - SOLUCIÓN
SELECT ProductID, Name, ListPrice
FROM Production.Product
WHERE ProductID NOT IN (SELECT DISTINCT ProductID FROM Sales.SalesOrderDetail)
ORDER BY ProductID;

-- EJERCICIO 2.4 - SOLUCIÓN
SELECT soh.SalesOrderID, soh.CustomerID, soh.TotalDue, soh.OrderDate
FROM Sales.SalesOrderHeader soh
WHERE soh.CustomerID = 11000
AND soh.TotalDue > (
    SELECT AVG(soh2.TotalDue)
    FROM Sales.SalesOrderHeader soh2
    WHERE soh2.CustomerID = 11000
)
ORDER BY soh.TotalDue DESC;

-- EJERCICIO 3.1 - SOLUCIÓN
SELECT 
    e.BusinessEntityID,
    p.FirstName + ' ' + p.LastName AS Nombre,
    edh.DepartmentID,
    eph.Rate
FROM HumanResources.Employee e
INNER JOIN Person.Person p ON e.BusinessEntityID = p.BusinessEntityID
INNER JOIN HumanResources.EmployeeDepartmentHistory edh ON e.BusinessEntityID = edh.BusinessEntityID
INNER JOIN HumanResources.EmployeePayHistory eph ON e.BusinessEntityID = eph.BusinessEntityID
WHERE eph.Rate > (
    SELECT AVG(eph2.Rate)
    FROM HumanResources.EmployeePayHistory eph2
    INNER JOIN HumanResources.EmployeeDepartmentHistory edh2 ON eph2.BusinessEntityID = edh2.BusinessEntityID
    WHERE edh2.DepartmentID = edh.DepartmentID
);

-- EJERCICIO 3.2 - SOLUCIÓN
SELECT soh.SalesOrderID, soh.CustomerID, soh.TotalDue, soh.OrderDate
FROM Sales.SalesOrderHeader soh
WHERE soh.TotalDue > (
    SELECT AVG(soh2.TotalDue)
    FROM Sales.SalesOrderHeader soh2
    WHERE soh2.CustomerID = soh.CustomerID
)
ORDER BY soh.CustomerID, soh.TotalDue DESC;

-- EJERCICIO 3.3 - SOLUCIÓN
SELECT p.ProductID, p.Name, p.ListPrice, p.ProductSubcategoryID
FROM Production.Product p
WHERE p.ProductSubcategoryID IS NOT NULL
AND p.ListPrice > (
    SELECT AVG(p2.ListPrice)
    FROM Production.Product p2
    WHERE p2.ProductSubcategoryID = p.ProductSubcategoryID
)
ORDER BY p.ProductSubcategoryID, p.ListPrice DESC;

-- EJERCICIO 3.4 - SOLUCIÓN
SELECT DISTINCT c.CustomerID, COUNT(soh.SalesOrderID) AS NumeroOrdenes
FROM Sales.Customer c
INNER JOIN Sales.SalesOrderHeader soh ON c.CustomerID = soh.CustomerID
WHERE EXISTS (
    SELECT 1
    FROM Sales.SalesOrderHeader soh2
    WHERE soh2.CustomerID = c.CustomerID
    AND soh2.TotalDue > (SELECT AVG(TotalDue) FROM Sales.SalesOrderHeader)
)
GROUP BY c.CustomerID
ORDER BY NumeroOrdenes DESC;

-- EJERCICIO 4.1 - SOLUCIÓN
SELECT ProductID, Name, ListPrice
FROM Production.Product
WHERE ListPrice > (SELECT AVG(ListPrice) FROM Production.Product)
AND ListPrice < (SELECT MAX(ListPrice) FROM Production.Product)
ORDER BY ListPrice DESC;

-- EJERCICIO 4.2 - SOLUCIÓN
SELECT sp.BusinessEntityID, p.FirstName + ' ' + p.LastName AS Nombre, sp.SalesYTD
FROM Sales.SalesPerson sp
INNER JOIN Person.Person p ON sp.BusinessEntityID = p.BusinessEntityID
WHERE sp.SalesYTD > (SELECT AVG(SalesYTD) FROM Sales.SalesPerson)
AND sp.SalesYTD < (SELECT MAX(SalesYTD) FROM Sales.SalesPerson)
ORDER BY sp.SalesYTD DESC;

-- EJERCICIO 4.3 - SOLUCIÓN
SELECT SalesOrderID, CustomerID, TotalDue, OrderDate
FROM Sales.SalesOrderHeader
WHERE CustomerID = (
    SELECT TOP 1 CustomerID
    FROM Sales.SalesOrderHeader
    GROUP BY CustomerID
    ORDER BY COUNT(*) DESC
)
ORDER BY TotalDue DESC;

-- EJERCICIO 4.4 - SOLUCIÓN
WITH CategoriaConMayorPrecio AS (
    SELECT TOP 1 psc.ProductCategoryID, AVG(p.ListPrice) AS PromedioPorCategoria
    FROM Production.Product p
    INNER JOIN Production.ProductSubcategory psc ON p.ProductSubcategoryID = psc.ProductSubcategoryID
    GROUP BY psc.ProductCategoryID
    ORDER BY AVG(p.ListPrice) DESC
)
SELECT p.ProductID, p.Name, p.ListPrice, psc.ProductCategoryID
FROM Production.Product p
INNER JOIN Production.ProductSubcategory psc ON p.ProductSubcategoryID = psc.ProductSubcategoryID
INNER JOIN CategoriaConMayorPrecio ccmp ON psc.ProductCategoryID = ccmp.ProductCategoryID
WHERE p.ListPrice > (
    SELECT AVG(p2.ListPrice)
    FROM Production.Product p2
    WHERE p2.ProductSubcategoryID = p.ProductSubcategoryID
)
ORDER BY p.ListPrice DESC;

*/
