using FastEndpoints;
using FastEndpoints.Testing;
using FluentAssertions;
using Wars.Villages.Features;

namespace Wars.IntegrationTests.Features;

[Collection(nameof(Fixture))]
public class ListVillagesTests(Fixture fixture) : TestBase<Fixture>
{
    [Fact]
    public async Task ListVillages_UserHasNoVillages_EmptyList()
    {
        var client = await fixture.CreateUserClient();
        var response = await client.GETAsync<ListVillages.Endpoint, ListVillages.Response>();

        response.Response.EnsureSuccessStatusCode();

        response.Result.VillageDto.Should().BeEmpty();
    }

    [Fact]
    public async Task ListVillages_UserHasOneVillage_ReturnsVillage()
    {
        var client = await fixture.CreateUserClient();

        var createRequest = new CreateVillage.Request("Riverwood");
        var createResponse = await client.POSTAsync<CreateVillage.Endpoint, CreateVillage.Request>(createRequest);
        createResponse.EnsureSuccessStatusCode();

        var response = await client.GETAsync<ListVillages.Endpoint, ListVillages.Response>();

        response.Response.EnsureSuccessStatusCode();

        response.Result.VillageDto.Should().ContainSingle(village => village.Name == "Riverwood");
    }
}
