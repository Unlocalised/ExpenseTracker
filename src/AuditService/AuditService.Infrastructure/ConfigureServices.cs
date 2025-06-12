using AuditService.Infrastructure.Account;
using ExpenseTracker.Application.Account;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.Domain.Account;
using Microsoft.Extensions.Hosting;
using Weasel.Core;
using Marten;

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
            // If we're running in development mode, let Marten just take care
            // of all necessary schema building and patching behind the scenes
            if (hostEnvironment.IsDevelopment())
            {
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }
            options.Schema.For<AccountAggregate>().UseOptimisticConcurrency(true);
        }).UseLightweightSessions();
        services.AddScoped<IAccountQueryRepository, AccountQueryRepository>();
        return services;
    }
}
