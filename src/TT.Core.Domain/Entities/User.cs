using System.Text.RegularExpressions;

namespace TT.Core.Domain.Entities;

public class User : EntityBase
{
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public string AvatarName { get; private set; }
    public string AvatarPath { get; private set; }

    public void UpdateUser(string name, string document, string email)
    {
        Name = name ?? Name;
        Email = email ?? Email;

        UnmaskDocument(document);
        Update();
    }

    public void UpdateAvatar(string avatarName, string avatarPath)
    {
        AvatarName = avatarName ?? AvatarName;
        AvatarPath = avatarPath ?? AvatarPath;
        Update();
    }

    public void UnmaskDocument(string document)
    {
        Document = Regex.Replace(document, "[^0-9]", "");
    }

    public void ChangePassword(string password)
    {
        Password = password;
        Update();
    }

    public List<Rating> Ratings { get; set; }
    public List<Book> Books{ get; set; }
    public Account Account { get; set; }

    public User(string email, string password, string name, string document)
    {
        Email = email;
        Password = password;
        Name = name;
        UnmaskDocument(document);
    }
    public User()
    { }
}
