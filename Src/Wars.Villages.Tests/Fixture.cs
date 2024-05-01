using FastEndpoints.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Wars.Villages.Infrastructure.Data;
using Xunit.Abstractions;

namespace Wars.Villages.Tests;

public class Fixture(IMessageSink messageSink) : AppFixture<Program>(messageSink)
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder().Build();

    protected override async Task PreSetupAsync()
    {
        await _databaseContainer.StartAsync();
    }

    protected override async Task SetupAsync()
    {
        Client = CreateClient();

        var context = Services.GetRequiredService<VillagesContext>();
        var migrations = await context.Database.GetPendingMigrationsAsync();
        if (migrations.Any())
        {
            await context.Database.MigrateAsync();
        }
    }

    protected override void ConfigureApp(IWebHostBuilder a)
    {
        a.UseSetting("ConnectionStrings:Villages", _databaseContainer.GetConnectionString());
    }

    protected override async Task TearDownAsync()
    {
        await _databaseContainer.DisposeAsync();
    }
}
