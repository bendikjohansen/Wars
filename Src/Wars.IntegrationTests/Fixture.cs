using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using FastEndpoints.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Testcontainers.PostgreSql;
using Wars.Buildings.Infrastructure.Data;
using Wars.Resources.Infrastructure.Data;
using Wars.Users.Infrastructure.Data;
using Wars.Villages.Infrastructure.Data;
using Xunit.Abstractions;

namespace Wars.IntegrationTests;

public class Fixture(IMessageSink messageSink) : AppFixture<Program>(messageSink)
{
    private static readonly PostgreSqlContainer DatabaseContainer = new PostgreSqlBuilder().Build();

    internal FakeTimeProvider TimeProvider => Services.GetRequiredService<TimeProvider>() as FakeTimeProvider ??
                                              throw new ApplicationException();

    protected override async Task PreSetupAsync()
    {
        await DatabaseContainer.StartAsync();
    }

    protected override async Task SetupAsync()
    {
        // Remember to add a connection string overload as well
        await Migrate<UsersContext>();
        await Migrate<VillagesContext>();
        await Migrate<ResourcesContext>();
        await Migrate<BuildingsContext>();
    }

    public async Task<HttpClient> CreateUserClient()
    {
        return await UserClientFactory.Create(CreateClient);
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.AddSingleton<TimeProvider, FakeTimeProvider>();
    }

    private async Task Migrate<TContext>() where TContext : DbContext
    {
        var context = Services.GetRequiredService<TContext>();
        var migrations = await context.Database.GetPendingMigrationsAsync();
        if (migrations.Any())
        {
            await context.Database.MigrateAsync();
        }
    }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        // Remember to migrate for the db context as well
        a.UseSetting("ConnectionStrings:Users", DatabaseContainer.GetConnectionString());
        a.UseSetting("ConnectionStrings:Villages", DatabaseContainer.GetConnectionString());
        a.UseSetting("ConnectionStrings:Resources", DatabaseContainer.GetConnectionString());
        a.UseSetting("ConnectionStrings:Buildings", DatabaseContainer.GetConnectionString());
    }
}
