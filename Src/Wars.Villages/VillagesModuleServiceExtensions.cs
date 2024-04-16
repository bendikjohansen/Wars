using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Wars.Villages.Infrastructure.Data;

namespace Wars.Villages;

public static class VillagesModuleServiceExtensions
{
    public static IServiceCollection AddVillageModuleServices(this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        IList<Assembly> mediatRAssemblies)
    {
        // Setup database
        services.AddDbContext<VillagesDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Villages")));

        // Opt into mediatr
        mediatRAssemblies.Add(typeof(VillagesModuleServiceExtensions).Assembly);

        // Add services
        services.AddScoped<IVillagesRepository, EfCoreVillagesRepository>();

        logger.Information("{module} module services added!", "Villages");
        return services;
    }
}
