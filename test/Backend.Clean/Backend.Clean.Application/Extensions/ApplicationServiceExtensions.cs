using Backend.Clean.Application.Interfaces;
using Backend.Clean.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Clean.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
