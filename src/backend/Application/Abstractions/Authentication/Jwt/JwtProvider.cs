using Application.Abstractions.Authentication.Claims;
using Application.Abstractions.Authentication.PermissionService;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Abstractions.Authentication.Jwt;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IPermissionService _permissionService;

    public JwtProvider(IOptions<JwtOptions> options, IPermissionService permissionService)
        => (_jwtOptions,_permissionService)  = (options.Value, permissionService);

    public async Task<string> Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value)
        };

        // for permissions in jwt token
        var permissions = await _permissionService.GetPermissionsAsync(user.Id);

        foreach (var permission in permissions)
        {
            claims.Add(new(CustomClaims.Permission, permission));
        }
        //

        var signinCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            signinCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
