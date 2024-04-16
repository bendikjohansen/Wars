using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Wars.Resources.Infrastructure.Data;

namespace Wars.Resources;

public static class ResourcesModuleServicesExtensions
{
    public static IServiceCollection AddResourceModuleServices(this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        List<Assembly> mediatrAssemblies)
    {
        // Set up database
        var connectionString = configuration.GetConnectionString("Resources");
        services.AddDbContext<ResourcesDbContext>(options => options.UseNpgsql(connectionString));

        // Opt into mediatr
        mediatrAssemblies.Add(typeof(ResourcesModuleServicesExtensions).Assembly);

        // Register services
        services.AddScoped<IResourcesRepository, EfCoreResourceRepository>();

        logger.Information("{module} module services added!", "Resources");

        return services;
    }
}
