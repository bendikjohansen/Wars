using Ardalis.Result;
using FastEndpoints;
using MediatR;
using Wars.Users.UseCases;

namespace Wars.Users.Endpoints;

internal class Register(IMediator mediator) : Endpoint<RegisterRequest, AccessTokenResponse>
{
    private readonly IMediator _mediator = mediator;

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var registerCommand = new RegisterUserCommand(req.EmailAddress, req.Password);
        var result = await _mediator.Send(registerCommand, ct);

        if (result.Status == ResultStatus.Error)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendOkAsync(cancellation: ct);
    }

    public override void Configure()
    {
        Post("/register");
        AllowAnonymous();
    }
}

public record RegisterRequest
{
    public required string EmailAddress { get; init; }
    public required string Password { get; init; }
}
