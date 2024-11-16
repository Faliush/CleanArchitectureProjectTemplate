using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Authentication.PermissionService;
using Application.Abstractions.Caching;
using Application.Abstractions.Cryptography;
using Application.Abstractions.EmailSender;
using Application.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Authentication.Jwt;
using Persistence.Authentication.PermisionService;
using Persistence.Caching;
using Persistence.Cryptography;
using Persistence.Events;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IPermissionService, PermissionService>();
        
        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IPasswordHashChecker, PasswordHasher>();
        
        services.AddTransient<IEmailSender, EmailSender.EmailSender>();
        
        services.AddSingleton<InMemmoryMessageQueue>();
        services.AddSingleton<IEventBus, EventBus>();
        services.AddHostedService<IntegrationEventProcessorJob>();

        return services;
    }
}