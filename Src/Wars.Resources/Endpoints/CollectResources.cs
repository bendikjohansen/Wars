using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Wars.Resources.UseCases;

namespace Wars.Resources.Endpoints;

internal class CollectResources(IMediator mediator) : Endpoint<CollectResourcesRequest, ResourcesDto>
{
    private readonly IMediator _mediator = mediator;

    public override async Task HandleAsync(CollectResourcesRequest req, CancellationToken ct)
    {
        var command = new CollectResourcesCommand(req.VillageId);
        var result = await _mediator.Send(command, ct);

        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
        }

        var resourceInventory = result.Value.ResourceInventory;
        var response = new ResourcesDto
        {
            Clay = (int)Math.Floor(resourceInventory.Clay),
            Iron = (int)Math.Floor(resourceInventory.Iron),
            Wood = (int)Math.Floor(resourceInventory.Wood)
        };

        await SendAsync(response, cancellation: ct);
    }

    public override void Configure()
    {
        Post("/collect-resources");
    }
}

public record CollectResourcesRequest(string VillageId);

public record ResourcesDto
{
    public required int Clay { get; init; }
    public required int Wood { get; init; }
    public required int Iron { get; init; }
}
