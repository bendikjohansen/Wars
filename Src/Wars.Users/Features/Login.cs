using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using Wars.Users.Domain;

namespace Wars.Users.Features;

internal static class Login
{
    internal class Endpoint(UserManager<ApplicationUser> userManager) : Endpoint<Endpoint.RequestBody, Endpoint.ResponseBody>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public override void Configure()
        {
            AllowAnonymous();
            Post("login");
        }

        public override async Task HandleAsync(RequestBody req, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(req.EmailAddress);
            if (user is null)
            {
                await SendUnauthorizedAsync(cancellation: ct);
                return;
            }

            var loginSuccessful = await _userManager.CheckPasswordAsync(user, req.Password);
            if (!loginSuccessful)
            {
                await SendUnauthorizedAsync(cancellation: ct);
                return;
            }

            var token = JwtBearer.CreateToken(o =>
            {
                o.SigningKey = Config["Auth:JwtSecret"]!;
                o.User["Email"] = user.Email!;
                o.User["UserId"] = user.Id;
            });

            await SendAsync(new ResponseBody(token), cancellation: ct);
        }

        public record RequestBody(string EmailAddress, string Password);

        public record ResponseBody(string AccessToken);
    }
}
