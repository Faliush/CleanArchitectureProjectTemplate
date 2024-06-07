using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database;

public class ProccessMigrationsJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ProccessMigrationsJob> _logger;

    public ProccessMigrationsJob(
        IServiceProvider serviceProvider,
        ILogger<ProccessMigrationsJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>()!;
        try
        {
            await context.Database.MigrateAsync(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unhandled exceptions occurred while migrating database");
            return;
        }
    }
}
