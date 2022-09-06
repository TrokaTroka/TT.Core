namespace TT.Core.Domain.Entities;

public class Account : EntityBase
{
    public Guid Key { get; private set; }
    public bool IsActive { get; private set; }
    public Guid IdUser { get; private set; }
    public User User { get; set; }

    public Account(Guid key, Guid idUser)
    {
        Key = key;
        IdUser = idUser;
        IsActive = false;
    }
    public Account()
    { }
    public void ActiveAccount()
    {
        IsActive = true;
    }
}
