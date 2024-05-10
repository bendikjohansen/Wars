using Ardalis.Result;
using MediatR;

namespace Wars.Resources.Contracts;

public record PayCommand(string VillageId, int Clay, int Iron, int Wood, string Reason) : IRequest<Result>;
