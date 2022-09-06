using Microsoft.EntityFrameworkCore;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(TrokaTrokaDbContext context) : base(context)
    { }

    public async Task<Favorite> GetByBookUserId(Guid idBook, Guid idUser)
    {
        return await _contextSet.FirstOrDefaultAsync(f => f.IdBook == idBook && f.IdUser == idUser);
    }
}
