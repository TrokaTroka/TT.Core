using TT.Core.Domain.Entities;

namespace TT.Core.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetUserByEmail(string email);
    Task<User> UserAuth(string email, string password);
}
