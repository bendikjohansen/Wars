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
        ILogger logger)
    {
        // Add services
        services.AddDbContext<UsersContext>(options => options.UseNpgsql(configuration.GetConnectionString("Users")));
        services.AddIdentityCore<ApplicationUser>().AddEntityFrameworkStores<UsersContext>();

        logger.Information("{Module} module services added!", "Users");
        return services;
    }
}
