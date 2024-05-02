using FluentAssertions;
using Wars.Resources.Domain;

namespace Wars.Resources.Tests;

public class CollectResourcesTests
{
    [Fact]
    public void CollectResources_VillageWasJustCreated_NoResources()
    {
        var now = new DateTimeOffset(2024, 5, 2, 19, 42, 30, TimeSpan.Zero);
        var village = new Village(now)
        {
            Id = Guid.NewGuid().ToString()
        };

        village.ResourceInventory.Clay.Should().Be(0);
        village.ResourceInventory.Iron.Should().Be(0);
        village.ResourceInventory.Wood.Should().Be(0);
    }

    [Fact]
    public void CollectResources_VillageWasCreatedMinutesAgo_SomeResources()
    {
        var now = new DateTimeOffset(2024, 5, 2, 19, 42, 30, TimeSpan.Zero);
        var village = new Village(now)
        {
            Id = Guid.NewGuid().ToString()
        };

        village.CollectResources(now + TimeSpan.FromMinutes(5));

        village.ResourceInventory.Clay.Should().BePositive();
        village.ResourceInventory.Iron.Should().BePositive();
        village.ResourceInventory.Wood.Should().BePositive();
    }
}
