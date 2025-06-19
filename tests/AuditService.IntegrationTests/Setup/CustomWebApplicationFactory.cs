using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace AuditService.IntegrationTests.Setup;
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
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
            .Build();

    public override async ValueTask DisposeAsync()
    {
        await PostgreSqlContainer.StopAsync();
        await RedisContainer.StopAsync();
        await RabbitMqContainer.StopAsync();
        await base.DisposeAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        PostgreSqlContainer.StartAsync().GetAwaiter().GetResult();
        RedisContainer.StartAsync().GetAwaiter().GetResult();
        RabbitMqContainer.StartAsync().GetAwaiter().GetResult();
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
