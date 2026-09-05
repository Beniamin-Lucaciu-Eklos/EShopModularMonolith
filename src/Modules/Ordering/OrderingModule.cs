using EShop.Ordering.Data;
using EShop.Shared.Data;
using EShop.Shared.Data.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Eshop.Modules.Ordering;

public static class OrderingModule
{
    public static IServiceCollection AddOrderingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddScoped<ICatalogService, CatalogService>();

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        var connectionString = configuration["ConnectionStrings:EShopDbContext"];
        services.AddDbContext<OrderingDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
    {
        app.UseMigration<OrderingDbContext>();

        return app;
    }
}
