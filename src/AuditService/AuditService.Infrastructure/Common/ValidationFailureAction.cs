using ExpenseTracker.Application.Common.Exceptions;
using Wolverine.FluentValidation;
using FluentValidation.Results;

namespace AuditService.Infrastructure.Common;
internal class ValidationFailureAction<T> : IFailureAction<T>
{
    public void Throw(T message, IReadOnlyList<ValidationFailure> failures)
    {
        throw new ValidationException(failures);
    }
}
