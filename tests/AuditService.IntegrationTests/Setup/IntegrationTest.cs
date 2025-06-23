using Microsoft.Extensions.DependencyInjection;
using JasperFx.Core;
using Wolverine;
using Marten;
using Docker.DotNet.Models;

namespace AuditService.IntegrationTests.Setup;
public abstract class IntegrationTest : IAsyncLifetime
{
    protected CustomWebApplicationFactory<Program> _factory = null!;

    private IServiceScope? _serviceScope;
    private IDocumentStore? _documentStore;
    private IMessageBus? _messageBus;
    private IDocumentSession? _documentSession;

    protected IServiceProvider ServiceProvider => _serviceScope!.ServiceProvider;
    protected IMessageBus MessageBus => _messageBus ??= ServiceProvider.GetRequiredService<IMessageBus>();
    protected IDocumentStore DocumentStore => _documentStore ??= ServiceProvider.GetRequiredService<IDocumentStore>();
    protected IDocumentSession DocumentSession => _documentSession ??= DocumentStore.LightweightSession();

    public async Task InitializeAsync()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        await _factory.InitializeAsync();
        _serviceScope = _factory.Services.CreateScope();
        await WaitForPostgresReadinessAsync();
    }

    public async Task DisposeAsync()
    {
        _documentSession?.Dispose();
        _serviceScope?.Dispose();
        await _factory.DisposeAsync();
    }

    protected async Task GenerateProjectionsAsync()
    {
        using var daemon = await DocumentStore.BuildProjectionDaemonAsync();
        await daemon.WaitForNonStaleData(5.Seconds());
    }

    public async Task WaitForPostgresReadinessAsync()
    {
        var maxAttempts = 5;
        var delay = TimeSpan.FromSeconds(20);

        for (int i = 0; i < maxAttempts; i++)
        {
            try
            {
                var connectionString = _factory.PostgreSqlContainer.GetConnectionString();
                await using var conn = new Npgsql.NpgsqlConnection(connectionString);
                await conn.OpenAsync();
                return;
            }
            catch
            {
                if (i == maxAttempts - 1)
                    throw;
                await Task.Delay(delay);
            }
        }
    }
}
