using Application.Abstractions.Behaviors;
using Application.Abstractions.Caching;
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
            cfg.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBihavior<,>));

        services.AddMemoryCache();

        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
