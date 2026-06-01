using Backend.Data;
using Backend.Middleware;
using Backend.Repositories;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AdventureWorks"),
        sqlOptions => sqlOptions.CommandTimeout(3)));

// AutoMapper:
// builder.Services.AddAutoMapper(config =>
// {
//     config.AddProfile<ProductProfile>();
// });

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
