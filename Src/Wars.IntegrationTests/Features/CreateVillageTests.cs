using FastEndpoints;
using FastEndpoints.Testing;
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
}
