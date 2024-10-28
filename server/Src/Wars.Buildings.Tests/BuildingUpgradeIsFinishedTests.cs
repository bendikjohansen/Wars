using FluentAssertions;
using Wars.Buildings.Domain;

namespace Wars.Buildings.Tests;

public class BuildingUpgradeIsFinishedTests
{
    [Fact]
    public void IsFinished_ThereIsTimeLeft_UpgradeIsNotFinished()
    {
        var now = DateTimeOffset.UtcNow;
        var upgrade = BuildingUpgrade.CreateFrom(BuildingType.Headquarter,
            ResourceCollection.Empty,
            TimeSpan.FromMinutes(5),
            now);

        var isFinished = upgrade.IsFinished(now + TimeSpan.FromMinutes(5));

        isFinished.Should().BeFalse();
    }

    [Fact]
    public void IsFinished_NoMoreTimeLeft_UpgradeIsFinished()
    {
        var now = DateTimeOffset.UtcNow;
        var upgrade = BuildingUpgrade.CreateFrom(BuildingType.Headquarter,
            ResourceCollection.Empty,
            TimeSpan.FromMinutes(5),
            now);

        var isFinished = upgrade.IsFinished(now + TimeSpan.FromMinutes(5) + TimeSpan.FromTicks(1));

        isFinished.Should().BeTrue();
    }
}
