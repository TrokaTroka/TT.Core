using TT.Core.Domain.Entities;

namespace TT.Core.Domain.Interfaces.Repositories;

public interface IFavoriteRepository : IBaseRepository<Favorite>
{
    Task<Favorite> GetByBookUserId(Guid idBook, Guid idUser);
}
