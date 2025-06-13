using ExpenseTracker.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[ApiExceptionFilter]
[Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private IMessageBus? _messageBus;

    protected IMessageBus MessageBus => _messageBus ??= HttpContext.RequestServices.GetRequiredService<IMessageBus>();
}
