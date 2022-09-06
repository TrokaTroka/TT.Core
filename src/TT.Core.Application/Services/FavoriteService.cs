using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Application.Interfaces;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;

namespace TT.Core.Application.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IAuthenticatedUser _user;
    private readonly IFavoriteRepository _favRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogErrorRepository _log;

    public FavoriteService(IFavoriteRepository favRepository,
        IBookRepository bookRepository,
        IAuthenticatedUser user,
        IUserRepository userRepository,
        ILogErrorRepository log)
    {
        _favRepository = favRepository;
        _user = user;
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _log = log;
    }

    public async Task<Attempt<Failure, bool>> CreateFavorite(Guid idBook)
    {
        var failure = new Failure();
        try
        {
            var email = _user.GetEmailUserLogged();

            var user = await _userRepository.GetUserByEmail(email);

            var favorite = new Favorite(user.Id, idBook);

            await _favRepository.Create(favorite);

            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(FavoriteService), nameof(CreateFavorite), ex.Message));
            return failure;
        }
    }

    public async Task<Attempt<Failure, bool>> DeleteFavorite(Guid idBook)
    {
        var failure = new Failure();
        try
        {
            var email = _user.GetEmailUserLogged();

            var user = await _userRepository.GetUserByEmail(email);

            var favorite = await _favRepository.GetByBookUserId(idBook, user.Id);

            if (favorite is null)
            {
                failure.SetMessage("Favorito não encontrado");
                return failure;
            }

            _favRepository.Delete(favorite);
            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(FavoriteService), nameof(DeleteFavorite), ex.Message));
            return failure;
        }
    }

    public async Task<Attempt<Failure, PaginatedDto<BookshellDto>>> GetMyFavorites(PaginationQuery paginationQuery)
    {
        var failure = new Failure();
        try
        {
            var email = _user.GetEmailUserLogged();

            var user = await _userRepository.GetUserByEmail(email);

            var books = await _bookRepository.GetBookByUserId(paginationQuery.PageSize, paginationQuery.PageNumber, user.Id);

            var bookshell = new List<BookshellDto>();

            foreach (var book in books.Objects)
            {
                var photo = book.PhotosBooks.Select(c => c.Path).FirstOrDefault();

                var favorite = user != null && book.Favorites.Any(f => f.IdUser == user.Id);

                bookshell.Add(new BookshellDto(
                        book.Id,
                        book.Title,
                        book.Owner.Name,
                        GetRatingAverage(book.Owner.Ratings),
                        photo,
                        favorite
                    ));
            }

            var paginated =
                new PaginatedDto<BookshellDto>(bookshell, books.TotalPage, books.TotalItems, books.PageNumber, books.PageSize);

            return paginated;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(nameof(FavoriteService), nameof(GetMyFavorites), ex.Message));
            return failure;
        }
    }

    private static double GetRatingAverage(List<Rating> ratings)
    {
        return ratings.Count == 0
             ? 0.0
             : (double)ratings.Sum(r => r.Grade) / (double)ratings.ToArray().Length;
    }
}
