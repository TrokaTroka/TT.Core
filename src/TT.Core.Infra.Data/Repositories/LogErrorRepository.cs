using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class LogErrorRepository : BaseRepository<LogError>, ILogErrorRepository
{
    public LogErrorRepository(TrokaTrokaDbContext context) : base(context)
    { }
}
