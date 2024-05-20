using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Npgsql;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Base;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("DatabaseName")
        .WithUsername("Username")
        .WithPassword("Password")
        .WithCleanUp(true)
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Remove(services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>))!);
            services.Remove(services.SingleOrDefault(x => x.ServiceType == typeof(DbConnection))!);

            services.AddDbContext<ApplicationDbContext>((_, options) =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Test";
                option.DefaultChallengeScheme = "Test";
            }).AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", op => { });

        });
    }

    public async Task ResetDatabaseAsync() =>
        await _respawner.ResetAsync(_dbConnection);

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await InitializeDatabaseAsync();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        HttpClient = CreateClient();
        await InitializeRespawnerAsync();
    }
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    private async Task InitializeDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var scopedService = scope.ServiceProvider;
        var context = scopedService.GetRequiredService<ApplicationDbContext>();
        
        await context.Database.EnsureCreatedAsync();
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = [ "public" ]
        });
    }
}
