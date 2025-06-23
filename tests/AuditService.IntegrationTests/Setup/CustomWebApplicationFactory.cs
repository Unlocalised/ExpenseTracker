using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace AuditService.IntegrationTests.Setup;
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    public RabbitMqContainer RabbitMqContainer { get; } =
        new RabbitMqBuilder()
            .WithImage("rabbitmq:management-alpine")
            .WithUsername("rabbitmq")
            .WithPassword("rabbitmq")
            .Build();

    public RedisContainer RedisContainer { get; } =
        new RedisBuilder()
            .WithImage("redis")
            .Build();

    public PostgreSqlContainer PostgreSqlContainer { get; } =
        new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithPortBinding(5432, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
            .UntilCommandIsCompleted("pg_isready -U postgres -d db")
            ).Build();

    public async Task InitializeAsync()
    {
        await PostgreSqlContainer.StartAsync();
        await RedisContainer.StartAsync();
        await RabbitMqContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await PostgreSqlContainer.DisposeAsync();
        await RedisContainer.DisposeAsync();
        await RabbitMqContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    ["ConnectionStrings:Marten"] = PostgreSqlContainer.GetConnectionString(),
                    ["ConnectionStrings:Redis"] = RedisContainer.GetConnectionString(),
                    ["ConnectionStrings:RabbitMQ"] = RabbitMqContainer.GetConnectionString(),
                }!);
        });
        return base.CreateHost(builder);
    }

}
