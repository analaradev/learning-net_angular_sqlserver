# 50. Clean Architecture Básica (Arquitectura Limpia)

**Clean Architecture** (Arquitectura Limpia) es un patrón de diseño arquitectónico promovido por Robert C. Martin (Uncle Bob). Su objetivo principal es el **desacoplamiento total** de las reglas de negocio (el núcleo del software) de los detalles técnicos como la base de datos, los frameworks web, las APIs externas o la interfaz de usuario.

---

## 📖 Conceptos Clave de Clean Architecture

A diferencia de la **Arquitectura por Capas** convencional, donde todas las capas dependen en última instancia de la base de datos, en **Clean Architecture** las dependencias se invierten. Todo apunta hacia adentro, hacia el **Dominio** (las reglas de negocio).

```text
 ┌────────────────────────────────────────────────────────┐
 │            CAPAS EXTERNAS (Detalles Técnicos)          │
 │                                                        │
 │   [Presentación / API]       [Infraestructura]         │
 │     (Controllers)            (EF Core, SQL Server)     │
 └───────────┬───────────────────────────┬────────────────┘
             │                           │
             ▼                           ▼
 ┌────────────────────────────────────────────────────────┐
 │                 NÚCLEO / CORE (Lógica Pura)            │
 │                                                        │
 │               [Aplicación (Casos de Uso)]              │
 │                           │                            │
 │                           ▼                            │
 │                  [Dominio (Entidades)]                 │
 └────────────────────────────────────────────────────────┘
```


### 1. La Regla de Dependencia (Dependency Rule)
La regla de oro de Clean Architecture es: **El código de las capas internas no puede saber nada sobre el código de las capas externas**.
* Las clases de la capa **Dominio** o **Aplicación** no pueden importar namespaces de Entity Framework, ASP.NET Core ni ninguna librería externa de persistencia.
* Si necesitas comunicarte con la base de datos desde la capa de Aplicación, defines una **Interfaz** (un contrato) en el núcleo, y la capa de Infraestructura (externa) se encarga de implementarla.

---

## 🏛️ Las Capas en Clean Architecture

Típicamente un proyecto en Clean Architecture se divide en 4 capas principales (a menudo representadas como proyectos independientes en una solución .NET):

| Capa | Rol | Dependencias |
| :--- | :--- | :--- |
| **1. Dominio (Domain)** | Entidades del negocio, reglas lógicas puras del negocio. | **Ninguna** (Cero dependencias externas). |
| **2. Aplicación (Application)** | Casos de uso de la aplicación, DTOs, interfaces. | Depende únicamente de **Dominio**. |
| **3. Infraestructura (Infrastructure)** | Acceso a base de datos (EF Core, SQL Server), servicios de correo, llamadas a APIs externas. | Depende de **Aplicación** y **Dominio**. |
| **4. Presentación (Presentation / WebAPI)** | Controladores HTTP, configuración del arranque del programa (`Program.cs`). | Depende de **Aplicación** (e inyecta la Infraestructura). |

---

## ⚖️ Comparativa: Capas vs. Clean Architecture

Para entender la diferencia, analicemos dónde se definen e implementan los componentes:

### Caso 1: En Arquitectura por Capas (Data-Centric)
Las dependencias fluyen hacia abajo. Todo depende de la base de datos:
* **Servicio (Negocio)** llama a **Repositorio (Datos)**.
* Si quieres cambiar de Entity Framework Core a Dapper, tienes que alterar el Repositorio y posiblemente arrastrar cambios hacia los servicios porque están acoplados al modelo de datos.

### Caso 2: En Clean Architecture (Domain-Centric / Dependency Inversion)
Aplicamos el principio de **Inversión de Dependencias (DIP)**:
1. En la capa interna **Aplicación (Core)** definimos la interfaz:
   ```csharp
   // Ubicación: Core/Application/Interfaces/IProductRepository.cs
   public interface IProductRepository
   {
       Task<Product?> GetByIdAsync(int id);
   }
   ```
2. En la capa externa **Infraestructura** implementamos la interfaz usando EF Core:
   ```csharp
   // Ubicación: Infrastructure/Persistence/ProductRepository.cs
   using Microsoft.EntityFrameworkCore; // <-- Librería externa permitida aquí
   
   public class ProductRepository : IProductRepository
   {
       private readonly MyDbContext _context;
       public ProductRepository(MyDbContext context) => _context = context;

       public async Task<Product?> GetByIdAsync(int id)
       {
           return await _context.Products.FindAsync(id);
       }
   }
   ```
3. El **Caso de Uso / Servicio (en Aplicación)** utiliza la interfaz `IProductRepository` para obtener los datos. **No sabe ni le importa si los datos vienen de SQL Server, de un archivo de texto o de memoria RAM**.

---

## 🛠️ Ventajas de Clean Architecture

1. **Independencia de Frameworks:** Si el día de mañana decides cambiar de ASP.NET Core a otra tecnología, o cambiar de SQL Server a MongoDB, tu lógica de negocio (las reglas más valiosas de tu aplicación) no sufre ninguna modificación.
2. **Altamente Testeable:** Como el Dominio y la Aplicación no dependen de bases de datos ni herramientas externas, puedes escribir miles de pruebas unitarias ultrarrápidas simulando las interfaces con mocks sencillos.
3. **Independencia de la Interfaz de Usuario:** Puedes cambiar la API Web por una aplicación de consola o una cola de mensajería (RabbitMQ) sin alterar el funcionamiento del negocio.
