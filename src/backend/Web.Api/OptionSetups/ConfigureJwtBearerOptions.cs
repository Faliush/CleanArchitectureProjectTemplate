using Application.Abstractions.Authentication.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Persistence.Authentication.Jwt;

namespace Web.Api.OptionSetups;

public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions;

    public ConfigureJwtBearerOptions(IOptions<JwtOptions> options) 
        => _jwtOptions = options.Value;

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
        };
    }
}
