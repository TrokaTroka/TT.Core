using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class TradeRepository : BaseRepository<Trade>, ITradeRepository
{
    public TradeRepository(TrokaTrokaDbContext context) : base(context)
    { }
}
