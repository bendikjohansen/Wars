using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Wars.Buildings.Domain;
using Wars.Buildings.Infrastructure.Data;

namespace Wars.Buildings;

public static class BuildingsModuleServicesExtensions
{
    public static IServiceCollection AddBuildingsModuleServices(this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        List<Assembly> mediatrAssemblies)
    {
        // Set up database
        var connectionString = configuration.GetConnectionString("Buildings");
        services.AddDbContext<BuildingsContext>(options => options.UseNpgsql(connectionString));

        // Opt into mediatr
        mediatrAssemblies.Add(typeof(BuildingsModuleServicesExtensions).Assembly);

        // Register services
        services.AddScoped<IBuildingsRepository, EfCoreBuildingsRepository>();

        services.AddScoped<BuildingCostLookup>(_ => (_, level) => ResourceCollection.CreateFrom(
            (int)Math.Pow(level, 2),
            (int)Math.Pow(level, 2),
            (int)Math.Pow(level, 2)
        ));
        services.AddScoped<BuildingDurationLookup>(_ => (_, level) => TimeSpan.FromMinutes(Math.Pow(level, 2.2)));

        logger.Information("{Module} module services added!", "Buildings");

        return services;
    }
}
