using Ardalis.Result;
using MediatR;

namespace Wars.Resources.Contracts;

public record HasFundsQuery(string VillageId, int Clay, int Iron, int Wood) : IRequest<Result<FundsAvailableDto>>;

public record FundsAvailableDto(bool Available);
