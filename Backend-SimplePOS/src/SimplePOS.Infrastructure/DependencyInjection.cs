using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplePOS.Application.Products;
using SimplePOS.Application.ProductGroups;
using SimplePOS.Infrastructure.Data;
using SimplePOS.Infrastructure.Repositories;
using SimplePOS.Application.Tenants;
using SimplePOS.Application.Users;

namespace SimplePOS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}