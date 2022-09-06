using Microsoft.EntityFrameworkCore;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(TrokaTrokaDbContext context) : base(context)
    { }

    public async Task<Account> GetByKey(Guid key)
    {
        return await _contextSet.FirstOrDefaultAsync(x => x.Key == key);
    }
}
