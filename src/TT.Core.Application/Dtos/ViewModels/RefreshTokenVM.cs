namespace TT.Core.Application.Dtos.ViewModels;

public class RefreshTokenVM
{
    public string Username { get; set; }
    public Guid Token { get; set; }
    public DateTime ExpirationDate { get; set; }

    public RefreshTokenVM()
    { }
    public RefreshTokenVM(string username, Guid token, DateTime expirationDate)
    {
        Username = username;
        Token = token;
        ExpirationDate = expirationDate;
    }
}
