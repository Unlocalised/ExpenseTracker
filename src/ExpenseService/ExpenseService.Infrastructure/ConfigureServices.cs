using ExpenseService.Application.Account.CreateAccount;
using ExpenseService.Infrastructure.Account;
using ExpenseService.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.Application.Common;
using ExpenseTracker.Domain.Account;
using Microsoft.Extensions.Hosting;
using Wolverine.FluentValidation;
using Wolverine;
using Marten;
using JasperFx.Events.Daemon;
using JasperFx;

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
            options.Projections.Add<AccountProjection>(JasperFx.Events.Projections.ProjectionLifecycle.Inline);
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
    public static IHostBuilder AddInfrastructureHostBuilder(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseWolverine(options =>
         {
             options.UseFluentValidation();
             options.Services.AddSingleton(typeof(IFailureAction<>), typeof(ValidationFailureAction<>));
             options.Discovery.IncludeAssembly(typeof(CreateAccountCommand).Assembly);
         });

        return hostBuilder;
    }
}
