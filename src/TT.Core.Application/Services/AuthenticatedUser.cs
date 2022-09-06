using Microsoft.AspNetCore.Http;
using TT.Core.Application.Interfaces;

namespace TT.Core.Application.Services;

public class AuthenticatedUser : IAuthenticatedUser
{
    private readonly IHttpContextAccessor _accessor;
    public AuthenticatedUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }


    public string GetEmailUserLogged()
    {
        var email = _accessor.HttpContext.User.Identity.Name;

        if (email is null)
            return string.Empty;

        return email;
    }
}
