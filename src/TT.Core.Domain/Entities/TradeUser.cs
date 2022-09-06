namespace TT.Core.Domain.Entities;

public class TradeUser
{
    public Guid IdUser { get; private set; }
    public Guid IdTrade { get; private set; }

    public TradeUser()
    { }
}
