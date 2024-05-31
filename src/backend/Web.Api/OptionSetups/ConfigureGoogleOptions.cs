using Application.Abstractions.Authentication.Google;
using Microsoft.Extensions.Options;

namespace Web.Api.OptionSetups;

public sealed class ConfigureGoogleOptions(IConfiguration configuration) 
    : IConfigureOptions<GoogleOptions>
{
    private const string SectionName = "GoogleSettings";

    public void Configure(GoogleOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
