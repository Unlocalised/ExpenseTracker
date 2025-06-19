using JasperFx.Core;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace AuditService.IntegrationTests.Setup;
public abstract class IntegrationTest(CustomWebApplicationFactory<Program> factory) : IAsyncLifetime
{
    private IDocumentStore? _documentStore;
    private IMessageBus? _messageBus;
    private IDocumentSession? _documentSession;

    protected IMessageBus MessageBus => _messageBus ??= factory.Services.GetRequiredService<IMessageBus>();
    protected IDocumentStore DocumentStore => _documentStore ??= factory.Services.GetRequiredService<IDocumentStore>();
    protected IDocumentSession DocumentSession => _documentSession ??= DocumentStore.LightweightSession();
    protected HttpClient? Client { get; private set; }

    public Task DisposeAsync()
    {
        _documentSession?.Dispose();
        Client?.Dispose();
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        Client = factory.CreateClient();

        return Task.CompletedTask;
    }

    protected async Task GenerateProjectionsAsync()
    {
        using var daemon = await DocumentStore.BuildProjectionDaemonAsync();
        await daemon.WaitForNonStaleData(5.Seconds());
    }
}
