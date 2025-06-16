using ExpenseTracker.Application.Common.Exceptions;
using FluentValidation.Results;
using Wolverine.FluentValidation;

namespace ExpenseService.Infrastructure.Common;
internal class ValidationFailureAction<T> : IFailureAction<T>
{
    public void Throw(T message, IReadOnlyList<ValidationFailure> failures)
    {
        throw new ValidationException(failures);
    }
}
