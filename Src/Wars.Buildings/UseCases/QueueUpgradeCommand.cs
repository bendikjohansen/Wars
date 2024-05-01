using Ardalis.Result;
using MediatR;
using Wars.Buildings.Domain;
using Wars.Resources.Contracts;

namespace Wars.Buildings.UseCases;

internal record QueueUpgradeCommand(string VillageId, BuildingType Building) : IRequest<Result>;

internal class QueueUpgradeCommandHandler(IBuildingsRepository repository, IMediator mediator)
    : IRequestHandler<QueueUpgradeCommand, Result>
{
    private readonly IBuildingsRepository _repository = repository;
    private readonly IMediator _mediator = mediator;

    public async Task<Result> Handle(QueueUpgradeCommand request, CancellationToken ct)
    {
        var village = await _repository.GetAsync(request.VillageId);
        if (village is null)
        {
            return Result.NotFound();
        }

        var level = village.GetBuildingLevelAfterQueue(request.Building);
        var buildingCostCalculator = new BuildingCost();
        var cost = buildingCostCalculator.ForUpgrading(request.Building, level + 1);

        var hasFundsQuery = new HasFundsQuery(request.VillageId, cost.Clay, cost.Iron, cost.Wood);
        var hasFundsResponse = await _mediator.Send(hasFundsQuery, ct);
        if (!hasFundsResponse.IsSuccess)
        {
            return Result.Error(hasFundsResponse.Errors.ToArray());
        }
        if (!hasFundsResponse.Value.Available)
        {
            return Result.Error("Not enough funds");
        }

        village.QueueUpgrade(request.Building, buildingCostCalculator);
        await _repository.SaveChangesAsync(ct);

        var reason = $"Upgrading {request.Building.ToString()} to level {level + 1}.";
        var payCommand = new PayCommand(request.VillageId, cost.Clay, cost.Iron, cost.Wood, reason);
        await _mediator.Send(payCommand, ct);

        return Result.Success();
    }
}
