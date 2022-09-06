using System;

namespace TT.Core.Application.Dtos.ViewModels;

public class UserLoggedVM
{
    public UserLoggedVM(Guid id, string email)
    {
        Id = id;
        Email = email;
    }

    public Guid Id { get; set; }

    public string Email { get; set; }
}
