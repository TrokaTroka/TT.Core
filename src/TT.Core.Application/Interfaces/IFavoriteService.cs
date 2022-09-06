using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Domain.Entities;

namespace TT.Core.Application.Interfaces;

public interface IFavoriteService
{
    Task<Attempt<Failure, PaginatedDto<BookshellDto>>> GetMyFavorites(PaginationQuery paginationQuery);
    Task<Attempt<Failure, bool>> CreateFavorite(Guid idBook);
    Task<Attempt<Failure, bool>> DeleteFavorite(Guid idFavorite);
}
