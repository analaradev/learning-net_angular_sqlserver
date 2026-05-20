using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorks")));

var app = builder.Build();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/api/hello", () =>
{
    return new
    {
        message = "Hola desde .NET Core 10",
        date = DateTime.Now
    };
});

app.MapGet("/api/db-test", async (AdventureWorksContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return new { connected = canConnect };
});

app.MapGet("/api/productos", async (AdventureWorksContext db) =>
{
    var productos = await db.Database
        .SqlQuery<ProductoDto>($"""
            SELECT TOP 20
                ProductID,
                Name,
                ProductNumber,
                ListPrice
            FROM Production.Product
            ORDER BY Name
        """)
        .ToListAsync();

    return productos;
});

app.MapGet("/api/perfil", () =>
{
    return new
    {
        nombre = "Ana Hernandez",
        nivel = "Principiante",
        tecnologiaFavorita = "Flutter :P"
    };
});

app.MapGet("/api/productos/buscar", async (AdventureWorksContext db, string nombre) =>
{
    var productos = await db.Database
        .SqlQuery<ProductoDto>($"""
            select top 20
                ProductID,
                Name,
                ProductNumber,
                ListPrice
            from Production.Product
            where Name like {"%" + nombre + "%"}
            order by Name
        """)
        .ToListAsync();

    return productos;
});

app.MapGet("/api/productos/{id:int}", async (AdventureWorksContext db, int id) =>
{
    var producto = await db.Database
        .SqlQuery<ProductoDetalleDto>($"""
            select
                ProductID,
                Name,
                ProductNumber,
                Color,
                ListPrice,
                Weight
            from Production.Product
            where ProductID = {id}
        """)
        .FirstOrDefaultAsync();

    return producto is null ? Results.NotFound() : Results.Ok(producto);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
