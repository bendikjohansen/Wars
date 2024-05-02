using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Wars.Resources.Features;
using Wars.Villages.Features;

namespace Wars.IntegrationTests.Features;

[Collection(nameof(Fixture))]
public class CreateVillageTests(Fixture fixture) : TestBase<Fixture>
{
    [Fact]
    public async Task Create_UserDoesNotAlreadyHaveAVillage_VillageIsCreated()
    {
        var client = await fixture.CreateUserClient();

        var request = new CreateVillage.Request("Riverwood");
        var response = await client.POSTAsync<CreateVillage.Endpoint, CreateVillage.Request>(request);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CollectResources_VillageWasCreated_ResourcesExist()
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
