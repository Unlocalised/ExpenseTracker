using AuditService.Application.Account.GetAccountById;
using AuditService.Infrastructure.Transaction;
using AuditService.Infrastructure.Account;
using AuditService.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using AuditService.Application.Common;
using Microsoft.Extensions.Hosting;
using AuditService.Domain.Account;
using Wolverine.FluentValidation;
using Wolverine.RabbitMQ;
using Wolverine;
using JasperFx;
using Marten;
using JasperFx.Events.Projections;
using JasperFx.CodeGeneration;
using JasperFx.Events;
using JasperFx.Core;
using Wolverine.Marten;
using AuditService.Infrastructure.Account.IntegrationHandlers;


namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        var martenConnectionString = configuration.GetConnectionString("Marten");
        if (martenConnectionString is null)
            throw new ArgumentNullException("Connection string for Marten is not provided");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "ExpenseTracker_";
        });
        services.AddMarten(options =>
        {
            // Establish the connection string to your Marten database
            options.Connection(martenConnectionString);
            options.UseSystemTextJsonForSerialization();
            options.AutoCreateSchemaObjects = AutoCreate.All;
            options.Events.AppendMode = EventAppendMode.Quick;
            options.Events.UseMandatoryStreamTypeDeclaration = true;

            options.Projections.Add<AccountProjection>(ProjectionLifecycle.Inline);
            options.Projections.Add<TransactionProjection>(ProjectionLifecycle.Inline);

        })
            .IntegrateWithWolverine()
            .UseLightweightSessions();
        services.CritterStackDefaults(x =>
        {
            x.Production.GeneratedCodeMode = TypeLoadMode.Static;
            x.Production.ResourceAutoCreate = AutoCreate.None;
        });
        services.AddScoped<IAccountQueryRepository, AccountQueryRepository>();
        services.AddSingleton<ICacheService, RedisCacheService>();
        return services;
    }
    public static IHostBuilder AddInfrastructureHostBuilder(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseWolverine(options =>
        {
            options.UseFluentValidation();
            options.Services.AddSingleton(typeof(IFailureAction<>), typeof(ValidationFailureAction<>));
            options.Discovery.IncludeAssembly(typeof(GetAccountByIdQuery).Assembly);
            options.UseRabbitMqUsingNamedConnection("RabbitMQ")
            .DisableSystemRequestReplyQueueDeclaration();

            options.ListenToRabbitQueue("Account")
            .PreFetchCount(100)
            .ListenerCount(5)
            .CircuitBreaker(cb =>
            {
                cb.PauseTime = 1.Minutes();
                cb.FailurePercentageThreshold = 10;
            })
            .UseDurableInbox();
        });

        return hostBuilder;
    }
}
