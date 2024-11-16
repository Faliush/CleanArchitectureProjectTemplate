using Application.Abstractions.Authentication.Jwt;
using Microsoft.Extensions.Options;
using Persistence.Authentication.Jwt;

namespace Web.Api.OptionSetups;

public class ConfigureJwtOptions(IConfiguration configuration) : 
    IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
