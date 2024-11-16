using Microsoft.Extensions.Options;
using Persistence.Authentication.Google;

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
