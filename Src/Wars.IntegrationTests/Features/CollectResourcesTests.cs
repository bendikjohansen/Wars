using FakeItEasy;
using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Wars.Resources.Features;
using Wars.Villages.Features;

namespace Wars.IntegrationTests.Features;

[Collection(nameof(Fixture))]
public class CollectResourcesTests(Fixture fixture) : TestBase<Fixture>
{
    [Fact]
    public async Task CollectResources_VillageIsJustCreated_NoResources()
    {
        var client = await fixture.CreateUserClient();

        var villageId = await CreateVillageAndGetId(client);

        var request = new CollectResources.Request(villageId.ToString());
        var response = await client.POSTAsync<CollectResources.Endpoint, CollectResources.Request, CollectResources.Response>(request);
        response.Response.EnsureSuccessStatusCode();
        response.Result.Clay.Should().Be(0);
        response.Result.Iron.Should().Be(0);
        response.Result.Wood.Should().Be(0);
    }

    [Fact]
    public async Task CollectResources_VillageIsFiveMinutesOld_SomeResources()
    {
        var client = await fixture.CreateUserClient();

        A.CallTo(() => fixture.Now()).Returns(DateTimeOffset.UtcNow);
        var villageId = await CreateVillageAndGetId(client);
        A.CallTo(() => fixture.Now()).Returns(DateTimeOffset.UtcNow + TimeSpan.FromMinutes(1));

        var request = new CollectResources.Request(villageId.ToString());
        var response = await client.POSTAsync<CollectResources.Endpoint, CollectResources.Request, CollectResources.Response>(request);
        response.Response.EnsureSuccessStatusCode();
        response.Result.Clay.Should().BeGreaterThan(0);
        response.Result.Iron.Should().BeGreaterThan(0);
        response.Result.Wood.Should().BeGreaterThan(0);
    }

    private static async Task<Guid> CreateVillageAndGetId(HttpClient client)
    {
        var request = new CreateVillage.Request("Riverwood");
        var response = await client.POSTAsync<CreateVillage.Endpoint, CreateVillage.Request>(request);
        response.EnsureSuccessStatusCode();
        var listResponse = await client.GETAsync<ListVillages.Endpoint, ListVillages.Response>();
        listResponse.Response.EnsureSuccessStatusCode();
        var villageId = listResponse.Result.VillageDto.Single().Id;
        return villageId;
    }
}
