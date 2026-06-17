# 49. Arquitectura por Capas (Layered Architecture)

La **Arquitectura por Capas** (Layered Architecture) es uno de los patrones arquitectónicos más comunes y utilizados en el desarrollo de software. Consiste en dividir la aplicación en capas horizontales diferenciadas, donde cada una tiene una única responsabilidad y se comunica únicamente con la capa inmediatamente inferior.

---

## 📖 Conceptos Clave de la Arquitectura por Capas

El principio fundamental de este patrón es la **Separación de Responsabilidades** (Separation of Concerns). El flujo de la aplicación siempre viaja de arriba hacia abajo:

```text
  ┌────────────────────────────────────────────────────────┐
  │         Capa de Presentación (Controllers)             │
  └──────────────────────────┬─────────────────────────────┘
                             ▼
  ┌────────────────────────────────────────────────────────┐
  │            Capa de Negocio (Services)                  │
  └──────────────────────────┬─────────────────────────────┘
                             ▼
  ┌────────────────────────────────────────────────────────┐
  │         Capa de Acceso a Datos (Repositories)          │
  └──────────────────────────┬─────────────────────────────┘
                             ▼
  ┌────────────────────────────────────────────────────────┐
  │      Base de Datos / Contexto (EF Core DbContext)      │
  └────────────────────────────────────────────────────────┘
```


### 1. Capa de Presentación (Controllers)
* **¿Qué hace?** Es el punto de entrada de la aplicación. En nuestro Web API, son los controladores (ej. [ProductsController](file:///Users/usuario/Desktop/proyecto_activos/test/Backend/Controllers/ProductsController.cs)).
* **Responsabilidad:** Recibir peticiones HTTP (`GET`, `POST`, etc.), validar los datos de entrada básicos, delegar la ejecución a la capa de negocio y retornar el código de estado HTTP correcto (`200 OK`, `404 Not Found`, `400 BadRequest`).
* **Regla de oro:** No debe contener lógica de negocio compleja ni consultas directas SQL / Entity Framework.

### 2. Capa de Negocio (Services)
* **¿Qué hace?** Contiene las reglas del negocio de la aplicación (ej. [ProductService](file:///Users/usuario/Desktop/proyecto_activos/test/Backend/Services/ProductService.cs)).
* **Responsabilidad:** Tomar las decisiones lógicas (ej. calcular descuentos, aplicar reglas de validación de negocio, estructurar respuestas, mapear entidades a DTOs usando herramientas como Mapster).
* **Regla de oro:** Actúa como mediador. Recibe datos de la presentación, solicita información al repositorio, la procesa y la devuelve limpia.

### 3. Capa de Acceso a Datos (Repositories)
* **¿Qué hace?** Abstrae la forma en que se obtienen o guardan los datos (ej. [ProductRepository](file:///Users/usuario/Desktop/proyecto_activos/test/Backend/Repositories/ProductRepository.cs)).
* **Responsabilidad:** Realizar consultas a la base de datos (con Entity Framework Core, Dapper, SQL puro, etc.). 
* **Regla de oro:** No le importa la lógica de negocio ni HTTP. Su único trabajo es ir a la base de datos, traer la información y entregarla.

### 4. Capa de Dominio (Models y DTOs)
* **¿Qué hace?** Define las estructuras de datos que viajan entre las capas.
  * **Modelos (Entities):** Clases C# que mapean directamente a las tablas físicas de la base de datos (ej. [Product](file:///Users/usuario/Desktop/proyecto_activos/test/Backend/Models/Product.cs)).
  * **DTOs (Data Transfer Objects):** Clases optimizadas que definen qué datos se envían o reciben por la API, evitando exponer directamente la estructura interna de la base de datos al exterior.

---

## 🛠️ Flujo de Ejecución Paso a Paso

Analicemos cómo interactúan estas capas en nuestro proyecto mediante el flujo de **Obtener todos los productos**:

### Paso 1: Petición entra por la Presentación
El cliente llama a `GET /api/productos`. La petición cae en [ProductsController.cs](file:///Users/usuario/Desktop/proyecto_activos/test/Backend/Controllers/ProductsController.cs#L20-L26):

```csharp
[HttpGet]
public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
{
    // Llama al servicio (Capa de Negocio)
    var products = await _productService.GetAllAsync();
    return Ok(products); // Devuelve respuesta HTTP 200 con los datos
}
```

### Paso 2: Ejecución de Reglas y Mapeo en el Servicio
El controlador llama al método en [ProductService.cs](file:///Users/usuario/Desktop/proyecto_activos/test/Backend/Services/ProductService.cs#L18-L22):

```csharp
public async Task<List<ProductDto>> GetAllAsync()
{
    // Solicita datos al repositorio (Capa de Acceso a Datos)
    var products = await _productRepository.GetAllAsync();
    
    // Transforma entidades de base de datos (Product) a objetos de transferencia (ProductDto)
    return products.Adapt<List<ProductDto>>();
}
```

### Paso 3: Consulta en el Repositorio
El servicio llama al repositorio en [ProductRepository.cs](file:///Users/usuario/Desktop/proyecto_activos/test/Backend/Repositories/ProductRepository.cs#L17-L24):

```csharp
public async Task<List<Product>> GetAllAsync()
{
    // Hace la consulta real usando Entity Framework Core
    return await _context.Products
        .AsNoTracking()
        .OrderBy(product => product.Name)
        .Take(10)
        .ToListAsync();
}
```

---

## ⚖️ Ventajas de usar la Arquitectura por Capas

1. **Separación de Responsabilidades:** Si necesitas cambiar de SQL Server a PostgreSQL, solo modificas la Capa de Datos (los Repositorios). El resto de tu aplicación (servicios y controladores) sigue funcionando intacto.
2. **Mantenibilidad:** Cada clase hace una sola cosa y la hace bien. Es mucho más fácil buscar y corregir bugs.
3. **Facilidad de Pruebas (Testability):** 
   * Puedes hacer **Pruebas Unitarias** a la lógica de negocio (`ProductService`) usando simulaciones (Mocks) del repositorio, sin necesidad de tocar la base de datos real.
   * Puedes hacer **Pruebas de Integración** a los repositorios de datos de forma aislada.
