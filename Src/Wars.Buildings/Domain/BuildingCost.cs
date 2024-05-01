namespace Wars.Buildings.Domain;

internal class BuildingCost
{
    public ResourceCollection ForUpgrading(BuildingType building, int toLevel)
    {
        return GetCost(toLevel);
    }

    public ResourceCollection ForDemolishing(BuildingType building, int toLevel)
    {
        var costToUpgrade = GetCost(toLevel);
        const double recyclingFactor = 0.75;
        var recycleValue = new ResourceCollection
        {
            Clay = (int)(costToUpgrade.Clay * recyclingFactor),
            Iron = (int)(costToUpgrade.Iron * recyclingFactor),
            Wood = (int)(costToUpgrade.Wood * recyclingFactor),
        };
        return recycleValue;
    }

    private static ResourceCollection GetCost(int buildingLevel) => new()
    {
        Clay = 30 + (int)Math.Pow(buildingLevel, 2.8),
        Iron = 30 + (int)Math.Pow(buildingLevel, 2.8),
        Wood = 30 + (int)Math.Pow(buildingLevel, 2.8)
    };
}
