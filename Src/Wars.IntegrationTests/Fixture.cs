using FakeItEasy;
using FastEndpoints.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Wars.Common;
using Wars.Users.Infrastructure.Data;
using Wars.Villages.Infrastructure.Data;
using Xunit.Abstractions;

namespace Wars.IntegrationTests;

public class Fixture(IMessageSink messageSink) : AppFixture<Program>(messageSink)
{
    private static readonly PostgreSqlContainer DatabaseContainer = new PostgreSqlBuilder().Build();

    internal readonly Now Now = A.Fake<Now>();

    protected override async Task PreSetupAsync()
    {
        await DatabaseContainer.StartAsync();
    }

    protected override async Task SetupAsync()
    {
        await Migrate<UsersContext>();
        await Migrate<VillagesContext>();
    }

    public async Task<HttpClient> CreateUserClient()
    {
        return await UserClientFactory.Create(CreateClient);
    }

    protected override void ConfigureServices(IServiceCollection s)
    {
        s.AddScoped<Now>(_ => Now);
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
        a.UseSetting("ConnectionStrings:Users", DatabaseContainer.GetConnectionString());
        a.UseSetting("ConnectionStrings:Villages", DatabaseContainer.GetConnectionString());
    }
}
