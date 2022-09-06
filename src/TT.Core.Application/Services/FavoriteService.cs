using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;

namespace TT.Core.Application.Services;

public class FavoriteService : BaseService, IFavoriteService
{
    private readonly IAuthenticatedUser _user;
    private readonly IFavoriteRepository _favRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;

    public FavoriteService(IFavoriteRepository favRepository,
        IBookRepository bookRepository,
        IAuthenticatedUser user,
        IUserRepository userRepository,
        INotifier notifier) : base(notifier)
    {
        _favRepository = favRepository;
        _user = user;
        _bookRepository = bookRepository;
        _userRepository = userRepository;
    }

    public async Task CreateFavorite(Guid idBook)
    {
        var email = _user.GetEmailUserLogged();

        var user = await _userRepository.GetUserByEmail(email);

        var favorite = new Favorite(user.Id, idBook);

        await _favRepository.Create(favorite);
    }

    public async Task DeleteFavorite(Guid idBook)
    {
        var email = _user.GetEmailUserLogged();

        var user = await _userRepository.GetUserByEmail(email);

        var favorite = await _favRepository.GetByBookUserId(idBook, user.Id);

        if (favorite is null)
            Notify("Favorito não encontrado");

        if (!OperationValid())
            return;

        _favRepository.Delete(favorite);
    }

    public async Task<PaginatedDto<BookshellDto>> GetMyFavorites(PaginationQuery paginationQuery)
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

    private double GetRatingAverage(List<Rating> ratings)
    {
        return ratings.Count == 0
             ? 0.0
             : (double)ratings.Sum(r => r.Grade) / (double)ratings.ToArray().Length;
    }
}
