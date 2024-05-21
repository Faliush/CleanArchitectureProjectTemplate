using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database;

public class MigrationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MigrationService> _logger;

    public MigrationService(
        IServiceProvider serviceProvider,
        ILogger<MigrationService> logger)
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
