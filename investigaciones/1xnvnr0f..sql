-- create database TestDB
-- create schema Ventas;
-- create schema Inventario;
-- NombreColumna TipoDato

use TestDB;

create table Tienda.Productos(
	ProductoID INT IDENTITY(1,1) PRIMARY KEY,
	Nombre NVARCHAR(100) NOT NULL,
	Precio MONEY NOT NULL CHECK(Precio >= 0),
	Stock SMALLINT NOT NULL CHECK(Stock >= 0),
	Activo BIT NOT NULL DEFAULT 1,
	FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);

INSERT INTO Tienda.Productos(Nombre, Precio, Stock) 
values
	('Capibara', 15, 10),
	('Muñecas', 35, 4),
	('Lomocards', 5, 55);


select * from tienda.productos;

INSERT INTO Tienda.Productos(Nombre, Precio, Stock) 
values
	('Burbujas', -15, 10);

create table Tienda.Clientes (
	ClienteID int not null identity(1,1) primary key,
	Nombre nvarchar(100) not null,
	Email nvarchar(100) not null unique,
	FechaRegistro Datetime not null default getdate()
);

create table Tienda.Pedidos (
	PedidoID int identity(1,1) primary key,
	ClienteID int not null,
	FechaPedido DateTime not null default getdate(),
	Total Money not null check (Total >= 0),

	constraint FK_pedidos_clientes
	 foreign key (ClienteID) references Tienda.Clientes(ClienteID) 
);

create table Tienda.PedidosDetalle (
	DetalleID int identity(1,1) primary key,
	PedidoID int not null,
	ProductoID int not null,
	Cantidad smallint not null check(Cantidad >= 0),
	PrecioUnitario money not null check (PrecioUnitario >= 0),
	
	constraint fk_detalle_pedidos
		foreign key (PedidoID) references Tienda.Pedidos(PedidoID),

	constraint fk_detalle_productos
		foreign key (ProductoID) references Tienda.Productos(ProductoID),

	constraint uq_detalle_producto
		unique (PedidoID, ProductoID),
		
);

insert into Tienda.Clientes (Nombre, Email)
	values
	('Ana', 'ana@gmail.com'),
	('Eder', 'eder@gmail.com');
	
select * from Tienda.Clientes;

insert into Tienda.Pedidos(ClienteID, Total)
values
(1, 100),
(1, 30),
(2, 90);

select 
	p.PedidoID,
	c.Nombre,
	p.Total,
	p.FechaPedido
from Tienda.Pedidos p
inner join Tienda.Clientes c on c.ClienteID = p.ClienteID;

insert into Tienda.PedidosDetalle (PedidoID, ProductoID, Cantidad, PrecioUnitario)
values
	(1, 1, 2, 15), -- capibara
	(1, 2, 2, 35) -- muñecas
	;

select p.PedidoID, c.Nombre as NombreCliente, pr.Nombre as Producto, pd.PrecioUnitario, 
	(pd.PrecioUnitario * pd.Cantidad) as Subtotal  
from Tienda.PedidosDetalle pd
inner join Tienda.Pedidos p on p.PedidoID = pd.PedidoID
inner join Tienda.Clientes c on c.ClienteID = p.ClienteID
inner join Tienda.Productos pr on pr.ProductoID = pd.ProductoID
order by p.PedidoID;