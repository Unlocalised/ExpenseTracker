using ExpenseService.Infrastructure.Account;
using ExpenseService.Infrastructure.Transaction;
using ExpenseTracker.Application.Account;
using ExpenseTracker.Application.Transaction;
using ExpenseTracker.Domain.Account;
using ExpenseTracker.Domain.Transaction;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Weasel.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "ExpenseTracker_";
        });
        services.AddMarten(options =>
        {
            // Establish the connection string to your Marten database
            options.Connection(configuration.GetConnectionString("Marten")!);
            // Specify that we want to use STJ as our serializer
            options.UseSystemTextJsonForSerialization();
            options.Projections.Add<AccountProjection>(ProjectionLifecycle.Inline);
            // If we're running in development mode, let Marten just take care
            // of all necessary schema building and patching behind the scenes
            if (hostEnvironment.IsDevelopment())
            {
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }
            options.Schema.For<AccountAggregate>().UseOptimisticConcurrency(true);
        })
            .AddAsyncDaemon(DaemonMode.Solo)
            .UseLightweightSessions();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        return services;
    }
}
