using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Wars.Buildings.Features;
using Wars.Villages.Features;

namespace Wars.IntegrationTests.Features;

[Collection(nameof(Fixture))]
public class UpgradeBuildingIntegrationTests(Fixture fixture) : TestBase<Fixture>
{
    [Fact]
    public async Task QueueUpgrade_NotEnoughResourcesForPayment_Fails()
    {
        var client = await fixture.CreateUserClient();
        var villageId = await CreateVillageAndGetIdAsync(client);

        var request = new QueueUpgrade.Request(villageId.ToString(), QueueUpgrade.RequestBuildingType.Headquarter);
        var response = await client.POSTAsync<QueueUpgrade.Endpoint, QueueUpgrade.Request>(request);
        response.Should().HaveClientError();

        var buildings = await GetBuildingLevelsAsync(client, villageId);
        buildings.UpgradeQueue.Should().BeEmpty();
    }

    [Fact]
    public async Task QueueUpgrade_PaymentGoesThrough_UpgradeIsAddedToQueue()
    {
        var client = await fixture.CreateUserClient();
        var villageId = await CreateVillageAndGetIdAsync(client);

        fixture.TimeProvider.Advance(TimeSpan.FromMinutes(5));
        var queueRequest = new QueueUpgrade.Request(villageId.ToString(), QueueUpgrade.RequestBuildingType.Headquarter);
        var queueResponse = await client.POSTAsync<QueueUpgrade.Endpoint, QueueUpgrade.Request>(queueRequest);
        queueResponse.Should().BeSuccessful();

        var buildings = await GetBuildingLevelsAsync(client, villageId);
        buildings.UpgradeQueue.Should().NotBeEmpty();
    }

    private static async Task<ProcessBuildingLevels.Response> GetBuildingLevelsAsync(HttpClient client, Guid villageId)
    {
        var levelRequest = new ProcessBuildingLevels.Request(villageId.ToString());
        var levelResponse = await client.POSTAsync<ProcessBuildingLevels.Endpoint, ProcessBuildingLevels.Request,
            ProcessBuildingLevels.Response>(levelRequest);
        levelResponse.Response.Should().BeSuccessful();
        return levelResponse.Result;
    }

    private static async Task<Guid> CreateVillageAndGetIdAsync(HttpClient client)
    {
        var request = new CreateVillage.Request("Riverwood");
        var response = await client.POSTAsync<CreateVillage.Endpoint, CreateVillage.Request>(request);
        response.Should().BeSuccessful();

        var listResponse = await client.GETAsync<ListVillages.Endpoint, ListVillages.Response>();
        listResponse.Response.Should().BeSuccessful();
        var villageId = listResponse.Result.Villages.Single().Id;
        return villageId;
    }
}
