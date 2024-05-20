using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Authentication.PermissionService;
using Application.Abstractions.Behaviors;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Events;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBihavior<,>));

        services.AddSingleton<InMemmoryMessageQueue>();
        services.AddSingleton<IEventBus, EventBus>();
        services.AddHostedService<IntegrationEventProcessorJob>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IPasswordHashChecker, PasswordHasher>();

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IPermissionService, PermissionService>();

        return services;
    }
}
