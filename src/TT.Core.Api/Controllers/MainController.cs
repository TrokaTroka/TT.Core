using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class MainController : ControllerBase
{
    protected readonly INotifier _notifier;
    protected MainController(INotifier notifier)
    {
        _notifier = notifier;
    }

    protected bool OperationValid()
    {
        return !_notifier.HaveNotification();
    }

    public override OkObjectResult Ok([ActionResultObjectValue] object value)
    {
        return new OkObjectResult(new
        {
            success = true,
            data = value
        });
    }

    public override CreatedResult Created(string uri, [ActionResultObjectValue] object value)
    {
        return new CreatedResult(uri, new
        {
            success = true,
            data = value
        });
    }

    public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object error)
    {
        return new BadRequestObjectResult(new
        {
            success = false,
            errors = _notifier.GetAllNotifications().Select(n => n.Message)
        });
    }

    public override BadRequestObjectResult BadRequest([ActionResultObjectValue] ModelStateDictionary modelState)
    {
        if (!modelState.IsValid) NotifyInvalidModelError(modelState);

        return new BadRequestObjectResult(new
        {
            success = false,
            errors = _notifier.GetAllNotifications().Select(n => n.Message)
        });
    }

    public override NotFoundObjectResult NotFound([ActionResultObjectValue] object value)
    {
        return new NotFoundObjectResult(new
        {
            success = false,
            errors = _notifier.GetAllNotifications().Select(n => n.Message)
        });
    }

    protected void NotifyInvalidModelError(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);

        foreach (var erro in erros)
        {
            var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
            NotifyError(errorMsg);
        }
    }

    protected void NotifyError(string msg)
    {
        _notifier.Handle(new Notification(msg));
    }
}
