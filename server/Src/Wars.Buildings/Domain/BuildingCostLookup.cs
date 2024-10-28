namespace Wars.Buildings.Domain;

internal delegate ResourceCollection BuildingCostLookup(BuildingType building, int buildingLevel);
internal delegate TimeSpan BuildingDurationLookup(BuildingType building, int buildingLevel);
