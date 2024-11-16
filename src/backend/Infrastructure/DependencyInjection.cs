using Domain.Repositories;
using Domain.UnitOfWork;
using Infrastructure.Database;
using Infrastructure.Interceptors;
using Infrastructure.Outbox;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<ProccessMigrationsJob>();

        services.AddSingleton<ISaveChangesInterceptor, DbSaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, option) => option
                .UseNpgsql(configuration.GetConnectionString(nameof(ApplicationDbContext)))
                .AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>()));

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure.AddJob<ProcessOutboxMessagesJob>(jobKey)
                     .AddTrigger(trigger =>
                        trigger.ForJob(jobKey)
                                .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10)
                                                                        .RepeatForever())); 
        });

        services.AddScoped<IUnitOfWork<ApplicationDbContext>,  UnitOfWork<ApplicationDbContext>>();
        services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();

        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
