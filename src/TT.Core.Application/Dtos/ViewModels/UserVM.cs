using System;

namespace TT.Core.Application.Dtos.ViewModels;

public class UserVM
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string Document { get; set; }

    public string Name { get; set; }

    public string AvatarPath { get; set; }

    public UserVM()
    { }

    public UserVM(Guid id, string email, string document, string name, string avatarPath)
    {
        Id = id;
        Email = email;
        Document = document;
        Name = name;
        AvatarPath = avatarPath;
    }

    public void SetUserVM(Guid id, string email, string document, string name, string avatarPath)
    {
        Id = id;
        Email = email;
        Document = document;
        Name = name;
        AvatarPath = avatarPath;
    }
}
