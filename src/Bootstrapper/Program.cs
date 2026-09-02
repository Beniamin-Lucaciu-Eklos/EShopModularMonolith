namespace Eshop.Api
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            // Add services to the container.
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var catalogAssembly = typeof(CatalogModule).Assembly;
            var basketAssembly = typeof(BasketModule).Assembly;
            Assembly[] moduleAssemblies = [catalogAssembly, basketAssembly];

            builder.Services
                .AddCarterWithAssemblies(moduleAssemblies);
            builder.Services
                .AddMediatorRWithAssemblies(moduleAssemblies);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["ConnectionStrings:Redis"];
            });

            builder.Services
                .AddMassTransitWithAssemblies(builder.Configuration,
                moduleAssemblies);

            builder.Services
                .AddCatalogModule(builder.Configuration)
                .AddBasketModule(builder.Configuration)
                .AddOrderingModule(builder.Configuration);

            builder.Services
                .AddExceptionHandler<CustomExceptionHandler>();

            var app = builder.Build();

            app.MapCarter();
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseExceptionHandler(options =>
            {
            });

            app.UseCatalogModule()
               .UseBasketModule()
               .UseOrderingModule();

            app.Run();
        }
    }
}
