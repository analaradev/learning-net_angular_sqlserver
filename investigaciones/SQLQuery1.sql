--Enunciado:
--Usa INNER JOIN para unir Person.Person con Person.EmailAddress y mostrar el nombre completo y email de cada persona.

select p.BusinessEntityID,
		p.FirstName + ' ' + p.LastName as NombreCompleto,
		e.EmailAddress as Correo
from Person.Person p
inner join Person.EmailAddress e on e.BusinessEntityID = p.BusinessEntityID
order by p.BusinessEntityID

--Ejercicio 1.2 — Pedidos de venta con detalles de productos
--Enunciado:
--Une Sales.SalesOrderDetail con Production.Product para ver qué productos se vendieron, en qué cantidad y a qué precio.

select top 10
s.SalesOrderID as VentaID,
p.ProductID,
p.Name,
s.OrderQty,
s.UnitPrice,
s.LineTotal
from Sales.SalesOrderDetail s
inner join Production.Product p on p.ProductID = s.ProductID

--Ejercicio 1.3 — Vendedores con sus territorios
--Enunciado:
--Une Sales.SalesPerson con Person.Person y Sales.SalesTerritory para ver 
--el nombre del vendedor, su cuota de ventas y su territorio.

select
p.FirstName + ' ' + p.LastName as NombreCompleto,
sp.SalesQuota as CuotaVentas,
st.Name,
st.SalesYTD
from Person.Person p
inner join Sales.SalesPerson sp on sp.BusinessEntityID = p.BusinessEntityID
inner join Sales.SalesTerritory st on st.TerritoryID = sp.TerritoryID

--Ejercicio 2.1 — Productos vendidos en un pedido específico
--Enunciado:
--Muestra todos los productos del pedido #43659. Incluye 
--nombre del producto, categoría, cantidad y precio.

select 
p.Name as Producto,
pc.Name as Categoria,
psc.Name as SubCategoria,
sod.OrderQty as Cantidad,
sod.UnitPrice as PrecioUnitario

from Sales.SalesOrderDetail sod
inner join Production.Product p on p.ProductID = sod.ProductID
inner join Production.ProductSubcategory psc on psc.ProductSubcategoryID = p.ProductSubcategoryID
inner join Production.ProductCategory pc on pc.ProductCategoryID = psc.ProductCategoryID

where sod.SalesOrderID = 43659


--Ejercicio 2.2 — Pedidos de un cliente específico
--Enunciado:
--El cliente "Linda" → Encuentra y muestra todos sus pedidos con detalles: 
--fecha, total, nombre del vendedor que lo atendió

select
soh.OrderDate,
soh.TotalDue,
sp.
from Sales.SalesOrderHeader soh
inner join Sales.