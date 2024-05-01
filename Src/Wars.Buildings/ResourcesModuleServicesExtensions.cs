using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Wars.Buildings;

public static class BuildingsModuleServicesExtensions
{
    public static IServiceCollection AddResourceModuleServices(this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        List<Assembly> mediatrAssemblies)
    {
        // Set up database
        var connectionString = configuration.GetConnectionString("Buildings");
        // services.AddDbContext<BuildingsDbContext>(options => options.UseNpgsql(connectionString));

        // Opt into mediatr
        mediatrAssemblies.Add(typeof(BuildingsModuleServicesExtensions).Assembly);

        // Register services
        // services.AddScoped<IBuildingsRepository, EfCoreResourceRepository>();

        logger.Information("{module} module services added!", "Buildings");

        return services;
    }
}
