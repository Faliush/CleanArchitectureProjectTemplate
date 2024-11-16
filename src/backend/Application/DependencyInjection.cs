using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Authentication.PermissionService;
using Application.Abstractions.Behaviors;
using Application.Abstractions.Caching;
using Application.Abstractions.Cryptography;
using Application.Abstractions.EmailSender;
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
        
        return services;
    }
}
