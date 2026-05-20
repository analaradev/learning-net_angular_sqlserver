-- Total de ventas de todos los pedidos
use adventure_works;
SELECT 
    CustomerID,
    OrderDate,
    TotalDue,
    LAG(OrderDate) OVER (PARTITION BY CustomerID ORDER BY OrderDate) AS Pedido_Anterior,
    LEAD(OrderDate) OVER (PARTITION BY CustomerID ORDER BY OrderDate) AS Proximo_Pedido
FROM Sales.SalesOrderHeader
ORDER BY CustomerID, OrderDate;
