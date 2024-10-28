using FluentAssertions;
using Wars.Buildings.Domain;

namespace Wars.Buildings.Tests;

public class BuildingUpgradeTests
{
    private static DateTimeOffset Now => DateTimeOffset.UtcNow;
    private static BuildingCostLookup NoCost => (_, _) => ResourceCollection.Empty;
    private static BuildingDurationLookup Time(TimeSpan duration) => (_, _) => duration;

    [Fact]
    public void NewVillage_NoUpgradesAreQueued_JobsAreEmpty()
    {
        var village = Village.CreateFrom(Guid.NewGuid().ToString());
        village.UpgradeQueue.Should().BeEmpty();
    }

    [Fact]
    public void QueueUpgrade_NoUpgradesAlready_ContainsSingleUpgradeInQueue()
    {
        var village = Village.CreateFrom(Guid.NewGuid().ToString());

        village.QueueUpgrade(BuildingType.Headquarter, DateTimeOffset.Now, Time(default),  NoCost);

        village.UpgradeQueue.Should().ContainSingle();
    }

    [Fact]
    public void ProcessFinishedUpgrades_QueueContainsFinishedUpgrade_QueueIsEmptied()
    {
        var village = Village.CreateFrom(Guid.NewGuid().ToString());

        village.QueueUpgrade(BuildingType.Headquarter, Now, Time(default), NoCost);
        village.ProcessFinishedUpgrades(Now + TimeSpan.FromMinutes(5));

        village.UpgradeQueue.Should().BeEmpty();
    }

    [Fact]
    public void ProcessFinishedUpgrades_QueueContainsFinishedUpgrade_BuildingLevelsAreIncreased()
    {
        var village = Village.CreateFrom(Guid.NewGuid().ToString());
        var headquartersLevel = village.BuildingLevels.Headquarter;

        village.QueueUpgrade(BuildingType.Headquarter, Now, Time(TimeSpan.FromMinutes(5)), NoCost);
        village.ProcessFinishedUpgrades(Now + TimeSpan.FromMinutes(5));

        village.BuildingLevels.Headquarter.Should().BeGreaterThan(headquartersLevel);
    }

    [Fact]
    public void ProcessFinishedUpgrades_QueueContainsUnfinishedUpgrade_QueueRemainsTheSame()
    {
        var village = Village.CreateFrom(Guid.NewGuid().ToString());
        var headquartersLevel = village.BuildingLevels.Headquarter;

        village.QueueUpgrade(BuildingType.Headquarter, Now, Time(TimeSpan.FromMinutes(5)), NoCost);
        village.ProcessFinishedUpgrades(Now + TimeSpan.FromMinutes(4));

        village.BuildingLevels.Headquarter.Should().Be(headquartersLevel);
        village.UpgradeQueue.Should().ContainSingle();
    }
}
