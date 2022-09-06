using TT.Core.Domain.Entities;

namespace TT.Core.Domain.Interfaces.Repositories;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<Account> GetByKey(Guid key);
}
