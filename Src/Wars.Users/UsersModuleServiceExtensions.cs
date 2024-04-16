using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Wars.Users.Domain;
using Wars.Users.Infrastructure.Data;

namespace Wars.Users;

public static class UsersModuleServiceExtensions
{
    public static IServiceCollection AddUserModuleServices(this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        IList<Assembly> mediatRAssemblies)
    {
        // Add services
        services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Users")));
        services.AddIdentityCore<ApplicationUser>().AddEntityFrameworkStores<UsersDbContext>();

        // Opt into using MediatR
        mediatRAssemblies.Add(typeof(UsersModuleServiceExtensions).Assembly);

        logger.Information("{module} module services added!", "Users");
        return services;
    }
}