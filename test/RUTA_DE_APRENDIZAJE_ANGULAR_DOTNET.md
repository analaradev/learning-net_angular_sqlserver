# Ruta de aprendizaje: Angular + .NET

Esta es una guia recomendada para empezar a aprender Angular y .NET juntos, usando el proyecto actual como base.

Para practicar paso a paso con este mismo proyecto, abre tambien:

```text
EJERCICIOS_PRACTICOS_NET_ANGULAR_SQLSERVER.md
```

## 1. Bases de Angular

Primero aprende como funciona la parte visual de Angular.

Temas clave:

- Componentes
- Templates HTML
- Estilos SCSS/CSS
- Interpolacion con `{{ dato }}`
- Eventos como `(click)`
- Propiedades como `[value]`
- Senales con `signal()`

Archivos importantes del proyecto:

- `Frontend/src/app/app.ts`
- `Frontend/src/app/app.html`
- `Frontend/src/app/app.scss`

Ejemplo:

```ts
mensaje = signal('Hola desde Angular');
```

```html
<p>{{ mensaje() }}</p>
```

## 2. Servicios en Angular

Despues de entender los componentes, aprende a separar la logica de llamadas a la API en servicios.

La idea es no dejar todo dentro de `app.ts`, sino crear archivos como:

```text
Frontend/src/app/api.service.ts
```

Temas clave:

- Crear servicios
- Inyectar servicios en componentes
- Separar logica visual de logica de datos

## 3. HTTP en Angular

Luego aprende a consumir APIs usando la herramienta propia de Angular: `HttpClient`.

Temas clave:

- `provideHttpClient()`
- `HttpClient.get()`
- `HttpClient.post()`
- Manejo de errores
- Interfaces de TypeScript para datos

Ejemplo de idea:

```ts
this.http.get('/api/hello');
```

## 4. Bases de .NET Web API

Aprende como .NET expone rutas que Angular puede consumir.

En este proyecto, la ruta principal esta en:

```text
Backend/Program.cs
```

Ejemplo:

```csharp
app.MapGet("/api/hello", () =>
{
    return new
    {
        message = "Hola desde .NET",
        date = DateTime.Now
    };
});
```

Temas clave:

- `MapGet`
- `MapPost`
- `MapPut`
- `MapDelete`
- Parametros en rutas
- Recibir JSON
- Devolver objetos

## 5. Conectar Angular con .NET

Esta es la parte central del proyecto.

Angular llama:

```ts
fetch('/api/hello');
```

Y el proxy manda esa peticion al backend:

```text
Frontend/proxy.conf.json
```

Temas clave:

- Proxy de Angular
- Rutas que empiezan con `/api`
- Puertos distintos
- CORS
- Diferencia entre desarrollo y produccion

Flujo:

```text
Angular -> proxy -> .NET API -> Angular muestra datos
```

## 6. CRUD completo

Cuando ya entiendas lo basico, haz una aplicacion con operaciones completas:

- Listar datos
- Crear datos
- Editar datos
- Eliminar datos

Ideas de proyectos:

- Lista de tareas
- Inventario
- Alumnos
- Clientes
- Productos

Este paso te ayuda a practicar Angular y .NET juntos de forma real.

## 7. Base de datos con .NET

Despues de dominar el CRUD en memoria, aprende a guardar datos en una base de datos.

Temas clave:

- Entity Framework Core
- Modelos
- `DbContext`
- Migraciones
- SQL Server
- SQLite

## Recomendacion final

Empieza con una lista de tareas sencilla:

1. Angular muestra tareas.
2. .NET entrega las tareas desde una API.
3. Angular permite crear nuevas tareas.
4. .NET recibe y guarda las tareas.
5. Despues agregas base de datos.

Ese camino te va a ayudar a entender como se conectan realmente las dos tecnologias.
