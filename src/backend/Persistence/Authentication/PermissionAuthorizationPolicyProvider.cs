﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Persistence.Authentication;

public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) 
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if(policy is not null)
        {
            return policy;
        }

        var permissions = policyName.Split(',');

        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(permissions))
            .Build();
    }
}
