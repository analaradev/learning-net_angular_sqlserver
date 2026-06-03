using Backend.Data;
using Backend.Middleware;
using Backend.Repositories;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string frontendCorsPolicy = "FrontendCorsPolicy";

var adventureWorksConnectionString = builder.Configuration.GetConnectionString("AdventureWorks")
    ?? throw new InvalidOperationException("Connection string 'AdventureWorks' was not configured.");

builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(
        adventureWorksConnectionString,
        sqlOptions => sqlOptions.CommandTimeout(3)));

// AutoMapper:
// builder.Services.AddAutoMapper(config =>
// {
//     config.AddProfile<ProductProfile>();
// });

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(frontendCorsPolicy, policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseCors(frontendCorsPolicy);

app.MapControllers();

app.Run();
