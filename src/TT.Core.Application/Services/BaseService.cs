using FluentValidation;
using FluentValidation.Results;
using TT.Core.Application.Notifications;

namespace TT.Core.Application.Services;

public class BaseService
{
    protected readonly INotifier _notifier;
    public BaseService(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected bool OperationValid()
    {
        return !_notifier.HaveNotification();
    }

    protected void Notify(string message)
    {
        _notifier.Handle(new Notification(message));
    }

    protected void Notify(ValidationResult result)
    {
        foreach (var error in result.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }

    protected bool Validate<TR, TE>(TR validator, TE entity) where TR : AbstractValidator<TE>
    {
        var result = validator.Validate(entity);

        if (result.IsValid) return true;

        Notify(result);
        return false;
    }
}
