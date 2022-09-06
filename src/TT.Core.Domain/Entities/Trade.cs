using TT.Core.Domain.Enums;

namespace TT.Core.Domain.Entities;

public class Trade : EntityBase
{
    public Trade(Guid idBookReceived, Guid idUserReceived, Guid idBookGived,  Guid idUserGived)
    {
        IdBookReceived = idBookReceived;
        IdUserReceived = idUserReceived;
        IdBookGived = idBookGived;
        IdUserGived = idUserGived;
        TradeDate = DateTime.UtcNow;
        TradeStatus = TradeStatus.Pending;
    }

    public Guid IdBookReceived { get; set; }
    public Guid IdUserReceived { get; set; }
    public Guid IdBookGived { get; set; }
    public Guid IdUserGived { get; set; }
    public DateTime TradeDate { get; private set; }
    public TradeStatus TradeStatus { get; private set; }

    public void CompleteTrade()
    {
        TradeStatus = TradeStatus.Completed;
    }

    public Trade()
    { }
}
