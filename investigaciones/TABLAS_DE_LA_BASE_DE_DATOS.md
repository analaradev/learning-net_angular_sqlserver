# 📊 Tablas de Adventure Works 2022

## 🗂️ Organización

La base de datos está dividida en **9 esquemas** (categorías):
- **Sales** - Ventas y clientes
- **Production** - Productos y fabricación
- **Person** - Personas y direcciones
- **HumanResources** - Empleados y departamentos
- **Purchasing** - Compras a proveedores

---

## 💰 ESQUEMA: SALES (Ventas)

### **Sales.SalesOrderHeader** (31,465 filas)
**¿Qué es?** Los pedidos de venta

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| SalesOrderID | INT | ID único del pedido |
| CustomerID | INT | Quién compró |
| OrderDate | DATE | Cuándo se hizo el pedido |
| TotalDue | MONEY | Total a pagar |
| SalesPersonID | INT | Quién vendió |
| ShipDate | DATE | Cuándo se envió |

**Ejemplo:**
```
SalesOrderID: 43793
CustomerID: 11000
OrderDate: 2011-06-21
TotalDue: $3,756.99
```

---

### **Sales.SalesOrderDetail** (121,317 filas)
**¿Qué es?** Los detalles de cada pedido (qué productos se vendieron)

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| SalesOrderID | INT | A qué pedido pertenece |
| SalesOrderDetailID | INT | ID único del detalle |
| ProductID | INT | Qué producto |
| OrderQty | INT | Cantidad comprada |
| UnitPrice | MONEY | Precio por unidad |
| LineTotal | MONEY | Total de esa línea |

**Ejemplo:**
```
SalesOrderID: 43793
ProductID: 776
OrderQty: 1
UnitPrice: $745.35
LineTotal: $745.35
```

**Relación:** Un pedido (SalesOrderHeader) tiene muchos detalles (SalesOrderDetail)

---

### **Sales.Customer** (19,820 filas)
**¿Qué es?** Los clientes que compran

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| CustomerID | INT | ID único del cliente |
| PersonID | INT | Si es persona física |
| StoreID | INT | Si es tienda |
| TerritoryID | INT | Territorio de ventas |

**Ejemplo:**
```
CustomerID: 11000
PersonID: 7977
TerritoryID: 1
```

---

### **Sales.SalesPerson** (17 filas)
**¿Qué es?** Los vendedores

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| BusinessEntityID | INT | ID del vendedor |
| TerritoryID | INT | Territorio asignado |
| SalesYTD | MONEY | Ventas en el año actual |
| SalesLastYear | MONEY | Ventas del año anterior |
| Bonus | MONEY | Bonificación |

**Ejemplo:**
```
BusinessEntityID: 276
SalesYTD: $4,251,368.55
Bonus: $5,000.00
```

---

### **Sales.SalesTerritory** (10 filas)
**¿Qué es?** Las regiones o territorios de ventas

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| TerritoryID | INT | ID del territorio |
| Name | VARCHAR | Nombre (ej: Northeast, Southwest) |
| CountryRegionCode | CHAR | País |
| Group | VARCHAR | Grupo (North America, Europe, etc.) |
| SalesYTD | MONEY | Ventas totales del territorio |

**Ejemplo:**
```
TerritoryID: 1
Name: Northwest
Group: North America
```

---

## 🏭 ESQUEMA: PRODUCTION (Productos)

### **Production.Product** (504 filas)
**¿Qué es?** Los productos que vende la empresa

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| ProductID | INT | ID único |
| Name | VARCHAR | Nombre del producto |
| ProductNumber | VARCHAR | Código del producto |
| ListPrice | MONEY | Precio de lista |
| ProductSubcategoryID | INT | Subcategoría |
| Color | VARCHAR | Color |
| Size | VARCHAR | Tamaño |

**Ejemplo:**
```
ProductID: 1
Name: Adjustable Race
ListPrice: $0.00
Color: Silver
```

---

### **Production.ProductCategory** (4 filas)
**¿Qué es?** Las categorías generales de productos

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| ProductCategoryID | INT | ID único |
| Name | VARCHAR | Nombre (Bikes, Components, Clothing, Accessories) |

**Ejemplo:**
```
ProductCategoryID: 1
Name: Bikes
```

---

### **Production.ProductSubcategory** (37 filas)
**¿Qué es?** Subcategorías dentro de cada categoría

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| ProductSubcategoryID | INT | ID único |
| ProductCategoryID | INT | Categoría padre |
| Name | VARCHAR | Nombre (Road Bikes, Mountain Bikes, etc.) |

**Ejemplo:**
```
ProductSubcategoryID: 1
ProductCategoryID: 1
Name: Road Bikes
```

**Relación:** Category (4) → Subcategory (37) → Product (504)

---

### **Production.WorkOrder** (72,591 filas)
**¿Qué es?** Órdenes de trabajo/fabricación

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| WorkOrderID | INT | ID único |
| ProductID | INT | Qué se está fabricando |
| OrderQty | INT | Cantidad a fabricar |
| StockedQty | INT | Cantidad fabricada |
| ScrappedQty | INT | Cantidad descartada |
| StartDate | DATE | Cuándo comienza |
| EndDate | DATE | Cuándo termina |
| DueDate | DATE | Fecha de entrega |

---

### **Production.BillOfMaterials** (2,679 filas)
**¿Qué es?** Listado de componentes necesarios para fabricar un producto

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| BillOfMaterialsID | INT | ID único |
| ProductAssemblyID | INT | Producto final |
| ComponentID | INT | Componente necesario |
| PerAssemblyQty | INT | Cuántos componentes se necesitan |

**Ejemplo:**
```
ProductAssemblyID: 3 (Producto terminado)
ComponentID: 461 (Componente)
PerAssemblyQty: 2 (Se necesitan 2 de ese componente)
```

---

## 👤 ESQUEMA: PERSON (Personas)

### **Person.Person** (19,972 filas)
**¿Qué es?** Información de personas (clientes, empleados, contactos)

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| BusinessEntityID | INT | ID único |
| FirstName | VARCHAR | Nombre |
| LastName | VARCHAR | Apellido |
| MiddleName | VARCHAR | Segundo nombre |
| EmailPromotion | INT | Recibe promociones (0=No, 1=Sí) |

**Ejemplo:**
```
BusinessEntityID: 1
FirstName: Ken
LastName: Sánchez
```

---

### **Person.Address** (32,516 filas)
**¿Qué es?** Direcciones de personas/negocios

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| AddressID | INT | ID único |
| AddressLine1 | VARCHAR | Calle |
| City | VARCHAR | Ciudad |
| StateProvinceID | INT | Estado/Provincia |
| PostalCode | VARCHAR | Código postal |

**Ejemplo:**
```
AddressID: 1
AddressLine1: 2500 Purchase Street
City: Seattle
PostalCode: 98104
```

---

### **Person.EmailAddress**
**¿Qué es?** Correos electrónicos de personas

**Columnas:**
- BusinessEntityID
- EmailAddress
- Ejemplo: Ken@adventure-works.com

---

### **Person.PhoneNumberType**
**¿Qué es?** Tipos de teléfono (Home, Work, Mobile)

---

### **Person.PersonPhone**
**¿Qué es?** Números telefónicos de personas

---

## 👔 ESQUEMA: HUMANRESOURCES (Empleados)

### **HumanResources.Employee** (290 filas)
**¿Qué es?** Empleados de la empresa

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| BusinessEntityID | INT | ID del empleado |
| NationalIDNumber | VARCHAR | Cédula/Pasaporte |
| LoginID | VARCHAR | Login corporativo (adventure-works\nombre) |
| JobTitle | VARCHAR | Puesto (Sales Representative, Manager, etc.) |
| HireDate | DATE | Fecha de contratación |

**Ejemplo:**
```
BusinessEntityID: 1
LoginID: adventure-works\ken0
JobTitle: Chief Executive Officer
HireDate: 2009-01-14
```

---

### **HumanResources.Department** (16 filas)
**¿Qué es?** Departamentos de la empresa

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| DepartmentID | INT | ID único |
| Name | VARCHAR | Nombre (Sales, Production, etc.) |
| GroupName | VARCHAR | Grupo (Sales and Marketing, Manufacturing, etc.) |

**Ejemplo:**
```
DepartmentID: 1
Name: Engineering
GroupName: Research and Development
```

---

### **HumanResources.EmployeeDepartmentHistory**
**¿Qué es?** Historial de qué departamento trabajó cada empleado y cuándo

**Columnas principales:**
- BusinessEntityID
- DepartmentID
- StartDate
- EndDate (NULL si sigue en ese departamento)

---

### **HumanResources.EmployeePayHistory**
**¿Qué es?** Historial de salarios de empleados

**Columnas principales:**
- BusinessEntityID
- Rate (salario por hora)
- RateChangeDate

---

## 🛒 ESQUEMA: PURCHASING (Compras)

### **Purchasing.Vendor** (104 filas)
**¿Qué es?** Proveedores de la empresa

**Columnas principales:**
| Columna | Tipo | Significado |
|---|---|---|
| BusinessEntityID | INT | ID único |
| Name | VARCHAR | Nombre del proveedor |
| ActiveFlag | BIT | ¿Está activo? (1=Sí, 0=No) |

---

### **Purchasing.PurchaseOrderHeader**
**¿Qué es?** Órdenes de compra a proveedores

**Columnas principales:**
- PurchaseOrderID
- VendorID
- OrderDate
- TotalDue

---

### **Purchasing.PurchaseOrderDetail**
**¿Qué es?** Detalles de qué se compra en cada orden

---

### **Purchasing.ProductVendor**
**¿Qué es?** Relación entre productos y proveedores (quién vende qué)

---

## 📋 RELACIONES PRINCIPALES

```
VENTA:
    Customer (19,820)
        ↓
    SalesOrderHeader (31,465)  ← Qué se vendió
        ↓
    SalesOrderDetail (121,317)  ← Detalles del pedido
        ↓
    Product (504)  ← Qué productos se vendieron


PRODUCTO:
    ProductCategory (4)
        ↓
    ProductSubcategory (37)
        ↓
    Product (504)
        ↓
    BillOfMaterials  ← Componentes para fabricar


EMPLEADO:
    Department (16)
        ↓
    Employee (290)
        ↓
    EmployeeDepartmentHistory  ← Historial de departamentos
    EmployeePayHistory  ← Historial de salarios


PERSONA:
    Person (19,972)
        ↓
    Address (32,516)  ← Direcciones
    EmailAddress  ← Correos
    PersonPhone  ← Teléfonos
```

---

## 🔢 Números Clave

| Recurso | Cantidad |
|---|---|
| Clientes | 19,820 |
| Órdenes | 31,465 |
| Detalles de órdenes | 121,317 |
| Productos | 504 |
| Categorías | 4 |
| Subcategorías | 37 |
| Empleados | 290 |
| Departamentos | 16 |
| Proveedores | 104 |
| Órdenes de compra | ~500 |
| Órdenes de fabricación | 72,591 |

---

## 💡 Ejemplos de Preguntas que Puedes Responder

✅ **Ventas:** ¿Cuál fue la orden más grande? ¿Qué vendedor más vendió?

✅ **Productos:** ¿Cuál es el producto más caro? ¿Cuántos productos hay por categoría?

✅ **Clientes:** ¿Quién compró más? ¿Cuántas órdenes tiene cada cliente?

✅ **Empleados:** ¿Cuántos empleados por departamento? ¿Quién gana más?

✅ **Fabricación:** ¿Cuántos componentes se necesitan para un producto?

---

**Última actualización:** 21 de abril de 2026
