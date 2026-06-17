# Guía Práctica: Migración Paso a Paso a Clean Architecture

Esta guía te proporciona los comandos exactos y el mapa de archivos para migrar manualmente tu proyecto `Backend` actual (Arquitectura por Capas) a una solución física de **Clean Architecture** llamada `Backend.Clean` paso a paso desde cero.

---

## 🛠️ Paso 1: Crear el Cascarón (Solución y Proyectos)

Abre tu terminal en la carpeta `/Users/usuario/Desktop/proyecto_activos/test` y ejecuta el siguiente bloque de comandos:

```bash
# 1. Crear carpeta raíz y solución
mkdir Backend.Clean
cd Backend.Clean
dotnet new sln -n Backend.Clean

# 2. Crear los 4 proyectos
dotnet new classlib -o Backend.Clean.Domain
dotnet new classlib -o Backend.Clean.Application
dotnet new classlib -o Backend.Clean.Infrastructure
dotnet new webapi -o Backend.Clean.API

# 3. Vincular proyectos a la solución
dotnet sln add Backend.Clean.Domain/Backend.Clean.Domain.csproj
dotnet sln add Backend.Clean.Application/Backend.Clean.Application.csproj
dotnet sln add Backend.Clean.Infrastructure/Backend.Clean.Infrastructure.csproj
dotnet sln add Backend.Clean.API/Backend.Clean.API.csproj

# 4. Crear referencias físicas (Regla de dependencia apuntando hacia adentro)
dotnet add Backend.Clean.Application/Backend.Clean.Application.csproj reference Backend.Clean.Domain/Backend.Clean.Domain.csproj
dotnet add Backend.Clean.Infrastructure/Backend.Clean.Infrastructure.csproj reference Backend.Clean.Domain/Backend.Clean.Domain.csproj
dotnet add Backend.Clean.Infrastructure/Backend.Clean.Infrastructure.csproj reference Backend.Clean.Application/Backend.Clean.Application.csproj
dotnet add Backend.Clean.API/Backend.Clean.API.csproj reference Backend.Clean.Application/Backend.Clean.Application.csproj
dotnet add Backend.Clean.API/Backend.Clean.API.csproj reference Backend.Clean.Infrastructure/Backend.Clean.Infrastructure.csproj

# 5. Instalar paquetes de NuGet específicos por capa
dotnet add Backend.Clean.Application/Backend.Clean.Application.csproj package Mapster
dotnet add Backend.Clean.Application/Backend.Clean.Application.csproj package AutoMapper
dotnet add Backend.Clean.Infrastructure/Backend.Clean.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add Backend.Clean.Infrastructure/Backend.Clean.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add Backend.Clean.API/Backend.Clean.API.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add Backend.Clean.API/Backend.Clean.API.csproj package Swashbuckle.AspNetCore
dotnet add Backend.Clean.API/Backend.Clean.API.csproj package Microsoft.EntityFrameworkCore.Design
```

---

## 📂 Paso 2: Mapa de Migración de Archivos

Una vez creados los proyectos vacíos, debes copiar los archivos de tu proyecto `Backend` original hacia las nuevas carpetas correspondientes:

### 1. Capa de Dominio (Domain)
Copia todos tus modelos puros. **No debe haber ninguna lógica de EF Core aquí.**
* Copiar `Backend/Models/Product.cs` ➡️ `Backend.Clean.Domain/Product.cs`
* Copiar `Backend/Models/ProductNote.cs` ➡️ `Backend.Clean.Domain/ProductNote.cs`

### 2. Capa de Aplicación (Application)
Aquí colocas todo lo relacionado con DTOs, interfaces de contratos y servicios de lógica.
* Crear carpetas dentro de `Backend.Clean.Application/`: `Interfaces`, `Services`, `Dtos` y `Profiles`.
* Copiar `Backend/Services/IProductService.cs` ➡️ `Backend.Clean.Application/Interfaces/IProductService.cs`
* Copiar `Backend/Repositories/IProductRepository.cs` ➡️ `Backend.Clean.Application/Interfaces/IProductRepository.cs`
* Copiar `Backend/Services/ProductService.cs` ➡️ `Backend.Clean.Application/Services/ProductService.cs`
* Copiar `Backend/Services/ProductWriteResult.cs` ➡️ `Backend.Clean.Application/Services/ProductWriteResult.cs`
* Copiar todos los archivos de `Backend/Dtos/` ➡️ `Backend.Clean.Application/Dtos/`
* Copiar `Backend/Profiles/ProductProfile.cs` ➡️ `Backend.Clean.Application/Profiles/ProductProfile.cs`

### 3. Capa de Infraestructura (Infrastructure)
Aquí va la persistencia (EF Core) y los repositorios con las consultas a la base de datos.
* Crear carpetas dentro de `Backend.Clean.Infrastructure/`: `Data`, `Repositories`.
* Copiar `Backend/Data/AdventureWorksContext.cs` ➡️ `Backend.Clean.Infrastructure/Data/AdventureWorksContext.cs`
* Copiar `Backend/Repositories/ProductRepository.cs` ➡️ `Backend.Clean.Infrastructure/Repositories/ProductRepository.cs`
* Copiar toda la carpeta `Backend/Migrations/` ➡️ `Backend.Clean.Infrastructure/Migrations/`

### 4. Capa de API / Presentación (API)
Aquí van los controladores y el arranque del programa.
* Crear carpeta `Backend.Clean.API/Middleware/`.
* Copiar todos los archivos de `Backend/Controllers/` ➡️ `Backend.Clean.API/Controllers/`
* Copiar `Backend/Middleware/GlobalExceptionMiddleware.cs` ➡️ `Backend.Clean.API/Middleware/GlobalExceptionMiddleware.cs`
* Copiar `Backend/Program.cs` ➡️ `Backend.Clean.API/Program.cs`
* Copiar `Backend/appsettings.json` ➡️ `Backend.Clean.API/appsettings.json`
* Copiar `Backend/appsettings.Development.json` ➡️ `Backend.Clean.API/appsettings.Development.json`

---

## ✏️ Paso 3: Actualizar los Namespaces y Usings

Al mover los archivos, debes corregir la directiva `namespace` al principio de cada archivo y actualizar las declaraciones `using` para apuntar a la nueva estructura.

### Ejemplo de cambio:

* **En tu clase de dominio (`Product.cs`):**
  * Cambiar: `namespace Backend.Models;`
  * Por: `namespace Backend.Clean.Domain;`

* **En tu interfaz de repositorio (`IProductRepository.cs`):**
  * Cambiar: `namespace Backend.Repositories;`
  * Por: `namespace Backend.Clean.Application;`
  * Cambiar: `using Backend.Models;` ➡️ `using Backend.Clean.Domain;`

* **En tu servicio (`ProductService.cs`):**
  * Cambiar: `namespace Backend.Services;`
  * Por: `namespace Backend.Clean.Application;`
  * Cambiar: `using Backend.Repositories;` ➡️ `using Backend.Clean.Application;`
  * Cambiar: `using Backend.Models;` ➡️ `using Backend.Clean.Domain;`
  * Cambiar: `using Backend.Dtos;` ➡️ `using Backend.Clean.Application;` (o la carpeta correspondiente).

---

## 🚀 Paso 4: Construir y Probar

Una vez copiados y corregidos los archivos, entra en la carpeta raíz del nuevo proyecto y ejecuta:

```bash
# Compilar toda la solución
dotnet build
```

Si hay algún error de compilación, serán principalmente `using` faltantes debido al cambio de nombres de las carpetas a proyectos independientes. Una vez corregidos, puedes iniciar el proyecto ejecutando:

```bash
# Ejecutar la API
dotnet run --project Backend.Clean.API/Backend.Clean.API.csproj
```
