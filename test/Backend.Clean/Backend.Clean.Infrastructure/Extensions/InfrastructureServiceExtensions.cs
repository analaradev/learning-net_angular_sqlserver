using Backend.Clean.Domain.Interfaces;
using Backend.Clean.Infrastructure.Persistence;
using Backend.Clean.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Clean.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AdventureWorks")
            ?? throw new InvalidOperationException("Connection string 'AdventureWorks' was not configured.");

        services.AddDbContext<AdventureWorksContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.CommandTimeout(3)));

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
