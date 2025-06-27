using ExpenseService.Application.Account.CreateAccount;
using ExpenseService.Application.Common;
using ExpenseService.Domain.Account;
using ExpenseService.Infrastructure.Common;
using ExpenseTracker.Contracts.Account;
using ExpenseTracker.Contracts.Transaction;
using JasperFx;
using JasperFx.CodeGeneration;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Wolverine;
using Wolverine.FluentValidation;
using Wolverine.Marten;
using Wolverine.RabbitMQ;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        services.AddMarten(options =>
        {
            options.Connection(configuration.GetConnectionString("Marten")!);
            options.UseSystemTextJsonForSerialization();
            options.AutoCreateSchemaObjects = AutoCreate.All;
            options.Schema.For<AccountAggregate>().UseOptimisticConcurrency(true);
        })
        .IntegrateWithWolverine()
        .UseLightweightSessions();
        services.CritterStackDefaults(x =>
        {
            x.Production.GeneratedCodeMode = TypeLoadMode.Static;
            x.Production.ResourceAutoCreate = AutoCreate.None;
        });
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
             options.UseRabbitMqUsingNamedConnection("RabbitMQ")
              .UseSenderConnectionOnly()
              .DisableSystemRequestReplyQueueDeclaration();
             options.PublishMessage<AccountCreatedIntegrationEvent>().ToRabbitQueue("Account");
             options.PublishMessage<AccountUpdatedIntegrationEvent>().ToRabbitQueue("Account");
             options.PublishMessage<AccountDeletedIntegrationEvent>().ToRabbitQueue("Account");
             options.PublishMessage<AccountBalanceUpdatedIntegrationEvent>().ToRabbitQueue("Account");
             options.PublishMessage<TransactionCreatedIntegrationEvent>().ToRabbitQueue("Transaction");
             options.Policies.UseDurableOutboxOnAllSendingEndpoints();
         });

        return hostBuilder;
    }
}
