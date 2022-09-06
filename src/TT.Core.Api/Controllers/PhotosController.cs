using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TT.Core.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
public class PhotosController : ControllerBase
{
    public PhotosController()
    {

    }
}
