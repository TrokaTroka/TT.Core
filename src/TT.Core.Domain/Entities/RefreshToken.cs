namespace TT.Core.Domain.Entities;

public class RefreshToken : EntityBase
{
    public RefreshToken()
    {
        Token = Guid.NewGuid();
    }

    public string Username { get; set; }
    public Guid Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}
