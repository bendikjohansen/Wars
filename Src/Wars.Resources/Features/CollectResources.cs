using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Wars.Resources.Domain;

namespace Wars.Resources.Features;

internal static class CollectResources
{
    internal class Endpoint(IMediator mediator) : Endpoint<Endpoint.RequestBody, Endpoint.ResponseBody>
    {
        private readonly IMediator _mediator = mediator;

        public override void Configure()
        {
            Post("/collect-resources");
        }

        public override async Task HandleAsync(RequestBody req, CancellationToken ct)
        {
            var command = new Command(req.VillageId);
            var result = await _mediator.Send(command, ct);

            if (result.Status == ResultStatus.NotFound)
            {
                await SendNotFoundAsync(ct);
            }

            var resourceInventory = result.Value.ResourceInventory;
            var response = new ResponseBody
            {
                Clay = (int)Math.Floor(resourceInventory.Clay),
                Iron = (int)Math.Floor(resourceInventory.Iron),
                Wood = (int)Math.Floor(resourceInventory.Wood)
            };

            await SendAsync(response, cancellation: ct);
        }

        public record RequestBody(string VillageId);

        public record ResponseBody
        {
            public required int Clay { get; init; }
            public required int Wood { get; init; }
            public required int Iron { get; init; }
        }
    }

    internal record Command(string VillageId) : IRequest<Result<Village>>;

    internal class CommandHandler(IResourcesRepository resourcesRepository) : IRequestHandler<Command, Result<Village>>
    {
        private readonly IResourcesRepository _resourcesRepository = resourcesRepository;

        public async Task<Result<Village>> Handle(Command request, CancellationToken ct)
        {
            var village = await _resourcesRepository.GetAsync(request.VillageId, ct);
            if (village is null)
            {
                return Result.NotFound();
            }

            village.CollectResources(DateTimeOffset.UtcNow);
            await _resourcesRepository.SaveChangesAsync(ct);
            return village;
        }
    }
}