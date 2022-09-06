using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TT.Core.Application.Notifications;

namespace TT.Core.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class PhotosController : MainController
{
    public PhotosController(INotifier notifier) : base(notifier)
    {

    }
}
