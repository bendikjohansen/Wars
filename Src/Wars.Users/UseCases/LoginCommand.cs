using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wars.Users.Domain;

namespace Wars.Users.UseCases;

internal record LoginCommand : IRequest<Result<ApplicationUser>>
{
    public required string EmailAddress { get; init; }
    public required string Password { get; init; }
}

internal class LoginCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<LoginCommand, Result<ApplicationUser>>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<ApplicationUser>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (user is null)
        {
            return Result.Unauthorized();
        }

        var loginSuccessful = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!loginSuccessful)
        {
            return Result.Unauthorized();
        }

        return user;
    }
}
