using FastEndpoints;
using MediatR;
using Wars.Villages.UseCases;

namespace Wars.Villages.Endpoints;

internal class Create(IMediator mediator) : Endpoint<CreateVillageRequest>
{
    private readonly IMediator _mediator = mediator;

    public override async Task HandleAsync(CreateVillageRequest req, CancellationToken ct)
    {
        var userId = User.FindFirst("UserId")!.Value;
        var createCommand = new CreateVillageCommand(userId, req.Name);

        var result = await _mediator.Send(createCommand, ct);

        if (result.IsSuccess)
        {
            await SendOkAsync(ct);
            return;
        }

        await SendErrorsAsync(cancellation: ct);
    }

    public override void Configure()
    {
        Post("create-village");
        Claims("UserId");
    }
}

public record CreateVillageRequest(string Name);
