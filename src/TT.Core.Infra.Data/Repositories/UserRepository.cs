using Microsoft.EntityFrameworkCore;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(TrokaTrokaDbContext context) : base(context)
    { }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> UserAuth(string email, string password)
    {
        return await _context.Users.Where(u => u.Email == email && u.Password == password)
            .Include(u => u.Account)
            .FirstOrDefaultAsync();
    }
}
