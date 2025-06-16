using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ZymLabs.NSwag.FluentValidation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var martenConnectionString = configuration.GetConnectionString("Marten");
        var redisConnectionString = configuration.GetConnectionString("Redis");
        if (string.IsNullOrEmpty(martenConnectionString))
            throw new InvalidOperationException("Marten connection string is not configured in settings.");
        if (string.IsNullOrEmpty(redisConnectionString))
            throw new InvalidOperationException("Redis connection string is not configured in settings.");
        services.AddHealthChecks()
            .AddNpgSql(martenConnectionString)
            .AddRedis(redisConnectionString);

        services.AddControllers()
        .AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())).AddNewtonsoftJson(setup =>
        {
            setup.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            setup.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            setup.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
        });

        services.AddSignalR(options => options.MaximumParallelInvocationsPerClient = 10);

        services.AddScoped<FluentValidationSchemaProcessor>(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument((configure, serviceProvider) =>
        {
            var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

            // Add the fluent validations schema processor
            configure.SchemaSettings.SchemaProcessors.Add(fluentValidationSchemaProcessor);
            configure.Title = "AuditService API";
            configure.PostProcess = (document) => document.BasePath = "/audit/api";
        });
        services.AddCors(cors =>
        {
            cors.AddPolicy("ProductionCors", builder => builder.WithOrigins(["http://localhost"]).AllowAnyMethod().AllowAnyHeader());
            cors.AddPolicy("DevelopmentCors", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
        return services;
    }
}
