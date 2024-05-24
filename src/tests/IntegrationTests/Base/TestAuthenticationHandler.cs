using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Permissions = Domain.Enums.Permissions;

namespace IntegrationTests.Base;

internal class TestAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    private static readonly string CustomClaim = "permission";

    private static readonly List<Claim> _claims =
        [
            new(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, "developer@gmail.com"),
            new(CustomClaim, nameof(Permissions.ManageUsers)),
            new(CustomClaim, nameof(Permissions.User))
        ];

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(_claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
