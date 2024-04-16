using Ardalis.Result;
using FastEndpoints;
using FastEndpoints.Security;
using MediatR;
using Wars.Users.UseCases;

namespace Wars.Users.Endpoints;

internal class Login(IMediator mediator) : Endpoint<LoginRequest, AccessTokenResponse>
{
    private readonly IMediator _mediator = mediator;

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand { EmailAddress = req.EmailAddress, Password = req.Password },
            cancellationToken: ct);
        if (result.Status == ResultStatus.Unauthorized)
        {
            await SendUnauthorizedAsync(cancellation: ct);
        }

        var token = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = Config["Auth:JwtSecret"]!;
            o.User["Email"] = result.Value.Email!;
            o.User["UserId"] = result.Value.Id;
        });

        await SendAsync(new AccessTokenResponse(token), cancellation: ct);
    }

    public override void Configure()
    {
        AllowAnonymous();
        Post("login");
    }
}

public record LoginRequest(string EmailAddress, string Password);

public record AccessTokenResponse(string AccessToken);