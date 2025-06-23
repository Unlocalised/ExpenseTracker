using Microsoft.Extensions.DependencyInjection;
using JasperFx.Core;
using Wolverine;
using Marten;

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
}
