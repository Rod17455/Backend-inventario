using AspNetCoreRateLimit;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace API.Extension;

public static class ApplicationServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        });

    public static void AddAplicationServices(this IServiceCollection services)
    {
       /* services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<IReporteRepository, ReporteRepository>();    
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IProveedorRepository, ProveedorRepository>();*/
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureRateLimition(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();

        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
            options.RealIpHeader = "X-Real-IP";
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "10s",
                    Limit = 2
                }
            };
        });
    }

    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            //options.ApiVersionReader = new QueryStringApiVersionReader("ver");
            //options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            options.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("ver"),
                new HeaderApiVersionReader("X-Version")
                );
            options.ReportApiVersions = true;

        });
    }




}
