using MediatR;

namespace ExpenseTracker.Domain.Common;

public abstract record BaseEvent : INotification
{
}
