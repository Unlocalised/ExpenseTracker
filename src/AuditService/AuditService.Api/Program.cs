using JasperFx;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Host.AddInfrastructureHostBuilder();
builder.Services.AddWebApiServices(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseForwardedHeaders();
app.UseHealthChecks("/health");
app.UseStaticFiles();
app.UseOpenApi();
app.UseSwaggerUi(settings =>
{
    settings.Path = "/docs";
    settings.DocumentPath = "/docs/specification.json";
});

app.UseAuthorization();
if (app.Environment.IsDevelopment())
    app.UseCors("DevelopmentCors");
else
    app.UseCors("ProductionCors");
app.MapControllers();

return await app.RunJasperFxCommands(args);
public partial class Program { }
