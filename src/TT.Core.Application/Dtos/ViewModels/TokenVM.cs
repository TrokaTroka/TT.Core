namespace TT.Core.Application.Dtos.ViewModels;

public class TokenVM
{
    public UserVM User { get; set; }

    public string Token { get; set; }

    public Guid RefreshToken { get; set; }

    public DateTime Expiration { get; set; }

    public TokenVM()
    { }
    public TokenVM(UserVM user, string token, Guid refreshToken, DateTime expiration)
    {
        User = user;
        Token = token;
        RefreshToken = refreshToken;
        Expiration = expiration;
    }
}
