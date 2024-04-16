using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wars.Users.Domain;

namespace Wars.Users.UseCases;

internal record RegisterUserCommand(string EmailAddress, string Password) : IRequest<Result>;

internal class RegisterUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new ApplicationUser { Email = request.EmailAddress, UserName = request.EmailAddress };
        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
            return Result.Error(result.Errors.Select(e => e.Description).ToArray());
        }

        return Result.Success();
    }
}
