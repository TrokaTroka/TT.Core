using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.ViewModels;

namespace TT.Core.Application.Interfaces;

public interface IFavoriteService
{
    Task<PaginatedDto<BookshellDto>> GetMyFavorites(PaginationQuery paginationQuery);
    Task CreateFavorite(Guid idBook);
    Task DeleteFavorite(Guid idFavorite);
}
