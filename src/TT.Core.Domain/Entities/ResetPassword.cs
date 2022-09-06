namespace TT.Core.Domain.Entities;

public class ResetPassword
{
    public ResetPassword(string hash, string document, DateTime expiration)
    {
        Id = hash;
        Document = document;
        Expiration = expiration;
        IsActive = true;
    }

    public string Id { get; private set; }
    public string Document { get; private set; }
    public DateTime Expiration { get; private set; }
    public bool IsActive { get; private set; }

    public void DesativeHash()
    {
        IsActive = false;
    }

    public void UpdateReset(string hash, DateTime expiration)
    {
        Id = hash;
        Expiration = expiration;
        IsActive = true;
    }
}

