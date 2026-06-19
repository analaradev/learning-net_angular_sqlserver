# 50. Clean Architecture Full-Stack (.NET + Angular)

Este documento explica cómo conviven e interactúan **Angular (Frontend)** y **.NET (Backend)** en un entorno profesional aplicando los conceptos de **Clean Architecture** (Arquitectura Limpia).

---

## 🗺️ Mapa Arquitectónico Completo (Full-Stack)

Angular y .NET son dos aplicaciones totalmente independientes. Se ejecutan en entornos separados (Angular en el navegador, .NET en el servidor) y se comunican únicamente mediante peticiones HTTP transmitiendo datos en formato **JSON**.

```text
 ┌──────────────────────────────────────────────────────────────────┐
 │                    FRONTEND (Cliente Angular)                    │
 │                                                                  │
 │  ┌────────────────────────────────────────────────────────────┐  │
 │  │                 1. Componentes (Vista/UI)                  │  │
 │  │        (HTML, CSS y TypeScript - ej: ProductComponent)     │  │
 │  └─────────────────────────────┬──────────────────────────────┘  │
 │                                │ (Llama a)                       │
 │                                ▼                                 │
 │  ┌────────────────────────────────────────────────────────────┐  │
 │  │                 2. Servicios (Persistencia)                │  │
 │  │        (Llamadas HttpClient - ej: ProductService.ts)       │  │
 │  └─────────────────────────────┬──────────────────────────────┘  │
 │                                │ (Usa tipos de)                  │
 │                                ▼                                 │
 │  ┌────────────────────────────────────────────────────────────┐  │
 │  │                     3. Modelos (Dominio)                   │  │
 │  │        (Interfaces TypeScript - ej: product.model.ts)      │  │
 │  └────────────────────────────────────────────────────────────┘  │
 └────────────────────────────────┬─────────────────────────────────┘
                                  │
                                  │ Petición HTTP (POST /api/productos)
                                  │ con JSON Payload
                                  ▼
 ┌──────────────────────────────────────────────────────────────────┐
 │                    BACKEND (.NET Web API)                        │
 │                                                                  │
 │  ┌────────────────────────────────────────────────────────────┐  │
 │  │               4. Capa de Presentación (API)                │  │
 │  │        (Controladores C# - ej: ProductsController.cs)      │  │
 │  └─────────────────────────────┬──────────────────────────────┘  │
 │                                │ (Llama al caso de uso)          │
 │                                ▼                                 │
 │  ┌────────────────────────────────────────────────────────────┐  │
 │  │               5. Capa de Aplicación (Core)                 │  │
 │  │        (Servicios de Aplicación - ej: ProductService.cs)   │  │
 │  └─────────────────────────────┬──────────────────────────────┘  │
 │                                │ (Orquesta e interactúa)         │
 │                                ▼                                 │
 │  ┌─────────────────────────────┴──────────────────────────────┐  │
 │  │                 6. Capa de Dominio (Core)                  │  │
 │  │        (Entidades C# Puras - ej: Product.cs)               │  │
 │  └─────────────────────────────▲──────────────────────────────┘  │
 │                                │ (Persistido por)                │
 │                                │                                 │
 │  ┌─────────────────────────────┴──────────────────────────────┐  │
 │  │                 7. Capa de Infraestructura                 │  │
 │  │        (Entity Framework Core - ej: ProductRepository.cs)  │  │
 │  └────────────────────────────────────────────────────────────┘  │
 └──────────────────────────────────────────────────────────────────┘
```

---

## 🔄 El Ciclo de Vida de una Petición (Paso a Paso)

Analicemos cómo viaja la información cuando un usuario quiere **Crear un Producto** en el sistema:

### 1. El Formulario (Angular UI)
El usuario rellena el formulario de creación en la pantalla y pulsa el botón **"Guardar"**.
El archivo TypeScript del componente recibe los datos del formulario y llama al servicio de Angular:
```typescript
// En angular: product-create.component.ts
this.productService.createProduct(newProductData).subscribe(result => {
    console.log('¡Producto guardado!', result);
});
```

### 2. El Servicio Cliente (Angular HTTP)
El servicio de Angular toma los datos, los convierte en JSON y realiza la llamada de red HTTP POST hacia el servidor backend:
```typescript
// En angular: product.service.ts
createProduct(product: CreateProductDto): Observable<ProductDto> {
    return this.http.post<ProductDto>('http://localhost:5000/api/productos', product);
}
```

### 3. El Controlador (API .NET)
El servidor .NET recibe la petición HTTP. El controlador de la API toma el JSON del cuerpo de la petición y lo mapea automáticamente al DTO correspondiente de la capa de Aplicación:
```csharp
// En .NET: ProductsController.cs (Capa API)
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
{
    var result = await _productService.CreateAsync(dto);
    return Ok(result);
}
```

### 4. El Servicio y Reglas (Aplicación y Dominio .NET)
El controlador delega al servicio de **Aplicación** (`ProductService`).
1. El servicio valida las reglas (ej: usando `CreateProductValidator`).
2. Transforma el DTO en una entidad de **Dominio** (`Product`).
3. Llama al repositorio (mediante la interfaz `IProductRepository`) para guardar el producto.

### 5. La Persistencia (Infraestructura .NET)
La **Infraestructura** ejecuta el comando real en SQL Server:
```csharp
// En .NET: ProductRepository.cs (Capa Infrastructure)
public async Task AddAsync(Product product)
{
    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();
}
```

### 6. El Retorno y Renderizado (JSON de vuelta)
1. El Backend toma la entidad guardada, la convierte a `ProductDto` y la devuelve como JSON en la respuesta HTTP `200 OK`.
2. Angular recibe el JSON, lo lee como un objeto TypeScript tipado, y actualiza la pantalla del navegador agregando el nuevo producto a la lista visible.
