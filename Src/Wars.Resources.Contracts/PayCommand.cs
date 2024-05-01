namespace Wars.Resources.Contracts;

public record PayCommand(string VillageId, int Clay, int Iron, int Wood, string Reason);
