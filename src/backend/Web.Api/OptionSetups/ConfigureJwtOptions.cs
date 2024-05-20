using Application.Abstractions.Authentication.Jwt;
using Microsoft.Extensions.Options;

namespace Web.Api.OptionsSetup;

public class ConfigureJwtOptions(IConfiguration configuration) : 
    IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
