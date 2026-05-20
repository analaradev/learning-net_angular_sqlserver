# Ejercicios sencillos: Angular + .NET + SQL Server con AdventureWorks

Estos ejercicios son para empezar poco a poco. No buscan hacer una app grande todavia.

La idea que debes repetir muchas veces es esta:

```text
Angular pide datos -> .NET recibe la peticion -> .NET consulta AdventureWorks -> Angular muestra los datos
```

## Antes de empezar

Abre el backend:

```bash
dotnet run --project Backend
```

Abre el frontend:

```bash
cd Frontend
npm start
```

Luego abre:

```text
http://localhost:4200
```

## Ejercicio 1: Ver productos en SQL Server

Objetivo: conocer la tabla antes de usarla en la app.

Tabla:

- `Production.Product`

Que hacer:

1. Abre SQL Server.
2. Mira los primeros productos.
3. Revisa estas columnas:
   - `ProductID`
   - `Name`
   - `ProductNumber`
   - `ListPrice`
4. Ordena los productos por nombre.
5. Ordena los productos por precio.

No uses Angular ni .NET todavia.

## Ejercicio 2: Mostrar productos desde .NET

Objetivo: hacer que .NET lea productos desde AdventureWorks.

Archivos:

- `Backend/Program.cs`
- `Backend/Models/ProductoDto.cs`

Que hacer:

1. Revisa la ruta de productos que ya existe.
2. Asegurate de que consulte `Production.Product`.
3. Haz que solo muestre pocos productos.
4. Haz que solo mande datos simples:
   - Id
   - Nombre
   - Numero de producto
   - Precio
5. Prueba la ruta en el navegador.

No modifiques Angular en este ejercicio.

## Ejercicio 3: Mostrar productos en Angular

Objetivo: enseñar en pantalla los productos que vienen de .NET.

Archivos:

- `Frontend/src/app/app.ts`
- `Frontend/src/app/app.html`

Que hacer:

1. Desde Angular, llama la ruta de productos.
2. Guarda los productos en una variable o signal.
3. En el HTML, recorre la lista.
4. Muestra:
   - Nombre
   - Numero de producto
   - Precio

No agregues busquedas ni filtros todavia.

## Ejercicio 4: Cambiar el orden de los productos

Objetivo: practicar cambios pequenos en SQL.

Archivo:

- `Backend/Program.cs`

Que hacer:

1. Cambia la consulta para ordenar por nombre.
2. Prueba la pantalla.
3. Cambia la consulta para ordenar por precio.
4. Prueba la pantalla otra vez.

Pregunta para ti:

- Que cambio viste en Angular si solo modificaste .NET/SQL?

## Ejercicio 5: Buscar productos por texto

Objetivo: agregar una busqueda sencilla.

Tabla:

- `Production.Product`

Que hacer en SQL Server:

1. Haz una consulta que busque productos por nombre.
2. Prueba palabras como `bike`, `road` o `helmet`.

Que hacer en .NET:

1. Crea una ruta de busqueda.
2. La ruta debe recibir un texto.
3. La ruta debe buscar productos que tengan ese texto en el nombre.

Que hacer en Angular:

1. Agrega una caja de texto.
2. Agrega un boton de buscar.
3. Cuando presiones el boton, llama la ruta de busqueda.
4. Muestra los productos encontrados.

Este es el primer ejercicio donde usas las tres tecnologias juntas de forma mas completa.

## Ejercicio 6: Ver detalle de un producto

Objetivo: seleccionar un producto y ver mas informacion.

Tabla:

- `Production.Product`

Que hacer:

1. En .NET, crea una ruta que reciba el id de un producto.
2. La ruta debe buscar ese producto en AdventureWorks.
3. En Angular, agrega un boton en cada producto.
4. Al presionar el boton, muestra mas datos del producto.

Datos sugeridos:

- Nombre
- Numero de producto
- Color
- Precio

## Ejercicio 7: Mini reto final

Objetivo: juntar lo aprendido sin hacerlo enorme.

Haz una pantalla sencilla con:

1. Titulo de la app.
2. Lista de productos.
3. Buscador por nombre.
4. Boton para ver detalle.
5. Mensaje cuando no haya productos.

Usa solo esta tabla:

- `Production.Product`

## Checklist

- [ ] Puedo consultar productos en SQL Server.
- [ ] Puedo crear o modificar una ruta en .NET.
- [ ] Puedo ver la respuesta de .NET en el navegador.
- [ ] Puedo pedir datos desde Angular.
- [ ] Puedo mostrar una lista en Angular.
- [ ] Puedo buscar productos por texto.
- [ ] Puedo ver el detalle de un producto.

## Regla simple

- SQL Server: guarda y consulta datos.
- .NET: crea rutas y habla con SQL Server.
- Angular: muestra pantallas y botones.

