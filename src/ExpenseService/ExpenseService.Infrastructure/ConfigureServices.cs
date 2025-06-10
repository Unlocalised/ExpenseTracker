using ExpenseService.Infrastructure.Transaction;
using ExpenseTracker.Application.Transaction;
using ExpenseService.Infrastructure.Account;
using ExpenseTracker.Application.Account;
using Microsoft.Extensions.Configuration;
using Marten.Events.Daemon.Resiliency;
using ExpenseTracker.Domain.Account;
using Microsoft.Extensions.Hosting;
using Marten.Events.Projections;
using Weasel.Core;
using Marten;
using ExpenseTracker.Application.Common;
using ExpenseService.Infrastructure.Common;

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

        services.AddScoped<IUnitOfWork, MartenUnitOfWork>();
        return services;
    }
}
