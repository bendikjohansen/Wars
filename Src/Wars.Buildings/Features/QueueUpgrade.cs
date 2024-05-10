using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Microsoft.Extensions.Logging;
using Wars.Buildings.Domain;
using Wars.Resources.Contracts;

namespace Wars.Buildings.Features;

internal static class QueueUpgrade
{
    internal class Endpoint(IMediator mediator) : Endpoint<Request>
    {
        public override void Configure()
        {
            Post("/upgrade-building");
            Claims("UserId");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var command = new Command(req.VillageId, req.BuildingType switch
            {
                RequestBuildingType.Headquarter => BuildingType.Headquarter,
                RequestBuildingType.ClayPit => BuildingType.ClayPit,
                RequestBuildingType.Warehouse => BuildingType.Warehouse,
                RequestBuildingType.IronMine => BuildingType.IronMine,
                RequestBuildingType.LumberCamp => BuildingType.LumberCamp,
                _ => throw new NotSupportedException()
            });
            var result = await mediator.Send(command, cancellationToken: ct);

            // TODO Handle unauthorized
            if (result.Status is ResultStatus.NotFound)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            if (result.Status is ResultStatus.Error)
            {
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            await SendOkAsync(ct);
        }
    }

    internal record Request(string VillageId, RequestBuildingType BuildingType);

    internal enum RequestBuildingType
    {
        ClayPit,
        IronMine,
        LumberCamp,
        Warehouse,
        Headquarter
    }

    internal record Command(string VillageId, BuildingType Building) : IRequest<Result>;

    internal class CommandHandler(
        IBuildingsRepository repository,
        IMediator mediator,
        TimeProvider time,
        BuildingCostLookup costLookup,
        BuildingDurationLookup durationLookup,
        ILogger<CommandHandler> logger)
        : IRequestHandler<Command, Result>
    {
        private readonly IBuildingsRepository _repository = repository;
        private readonly IMediator _mediator = mediator;
        private readonly TimeProvider _time = time;
        private readonly BuildingCostLookup _costLookup = costLookup;
        private readonly BuildingDurationLookup _durationLookup = durationLookup;
        private readonly ILogger<CommandHandler> _logger = logger;

        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            var village = await _repository.GetAsync(request.VillageId, ct);
            if (village is null)
            {
                return Result.NotFound();
            }

            var level = village.GetBuildingLevelAfterQueue(request.Building);
            var cost = _costLookup(request.Building, level);

            var reason = $"Upgrading {request.Building.ToString()} to level {level + 1}.";
            var payCommand = new PayCommand(request.VillageId, cost.Clay, cost.Iron, cost.Wood, reason);
            var now = _time.GetUtcNow();
            _logger.LogInformation("Paying for upgrade at {Now}", now);
            var payResult = await _mediator.Send(payCommand, ct);

            if (!payResult.IsSuccess)
            {
                return Result.Error("Payment did not go through.");
            }

            village.QueueUpgrade(request.Building, now, _durationLookup, _costLookup);
            await _repository.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
