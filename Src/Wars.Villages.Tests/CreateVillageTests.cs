using FastEndpoints;
using FastEndpoints.Testing;
using Endpoint = Wars.Villages.Features.CreateVillage.Endpoint;

namespace Wars.Villages.Tests;

[Collection(nameof(Fixture))]
public class CreateVillageTests(Fixture fixture) : TestBase<Fixture>
{
    [Fact]
    public async Task Create_UserDoesNotAlreadyHaveAVillage_VillageIsCreated()
    {
        var request = new Endpoint.Request("Riverwood");
        var response = await fixture.Client.POSTAsync<Endpoint, Endpoint.Request>(request);

        response.EnsureSuccessStatusCode();
    }
}
