using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Wars.Users.Domain;

namespace Wars.Users.Features;

internal static class Register
{
    internal class Endpoint(UserManager<ApplicationUser> userManager)
        : Endpoint<RequestBody>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public override void Configure()
        {
            Post("/register");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RequestBody req, CancellationToken ct)
        {
            var newUser = new ApplicationUser { Email = req.EmailAddress, UserName = req.EmailAddress };
            var result = await _userManager.CreateAsync(newUser, req.Password);

            if (!result.Succeeded)
            {
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            await SendOkAsync(cancellation: ct);
        }
    }

    public record RequestBody(string EmailAddress, string Password);
}
