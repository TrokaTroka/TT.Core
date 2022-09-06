using Microsoft.Extensions.Configuration;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Application.Interfaces;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Integration.Azure;

namespace TT.Core.Application.Services;

public class BookService : IBookService
{
    private readonly IAuthenticatedUser _user;
    private readonly IBookRepository _bookRepository;
    private readonly IPhotoRepository _photoRepository;
    private readonly IBookCategoryRepository _bookCategoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogErrorRepository _log;

    private readonly string _blobConnectionString;
    private const string _blobContainerName = "books";
    public BookService(IBookRepository bookRepository,
        IBookCategoryRepository bookCategoryRepository,
        IPhotoRepository photoRepository,
        IUserRepository userRepository,
        IAuthenticatedUser user,
        ILogErrorRepository log,
        IConfiguration configuration)
    {
        _bookRepository = bookRepository;
        _bookCategoryRepository = bookCategoryRepository;
        _photoRepository = photoRepository;
        _userRepository = userRepository;
        _log = log;
        _user = user;
        _blobConnectionString = configuration.GetConnectionString("BlobStorage");
    }

    public async Task<Attempt<Failure, PaginatedDto<BookshellDto>>> GetBookshell(PaginationQuery paginationQuery)
    {
        var failure = new Failure();

        try
        {
            var email = _user.GetEmailUserLogged();

            var user = await _userRepository.GetUserByEmail(email);

            var books = await _bookRepository.GetBooks(paginationQuery.PageSize, paginationQuery.PageNumber, paginationQuery.Filter == Guid.Empty ? Guid.Empty : paginationQuery.Filter);

            var bookshell = new List<BookshellDto>();

            foreach (var book in books.Objects)
            {
                var photo = book.PhotosBooks.Select(c => c.Path).FirstOrDefault();

                var favorite = IsFavorite(book.Favorites, user);

                bookshell.Add(new BookshellDto(
                        book.Id,
                        book.Title,
                        book.Owner.Name,
                        GetRatingAverage(book.Owner.Ratings),
                        photo,
                        favorite
                    ));
            }

            return new PaginatedDto<BookshellDto>(bookshell, books.TotalPage, books.TotalItems, books.PageNumber, books.PageSize);
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(nameof(BookService), nameof(GetBookshell), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, BookDto>> GetBookById(Guid idBook)
    {
        var failure = new Failure();

        try
        {
            var email = _user.GetEmailUserLogged();

            var user = await _userRepository.GetUserByEmail(email);

            var book = await _bookRepository.GetBookById(idBook);
            if (book is null)
            {
                failure.SetMessage("Livro nao encontrado");
                return failure;
            }

            var imagePath = new List<string>();
            book.PhotosBooks.ForEach(i => imagePath.Add(i.Path));

            var categories = new List<string>();
            book.BookCategories.ForEach(c => categories.Add(c.Category.Name));

            return new BookDto(
                book.Id,
                book.Title,
                book.Author,
                book.Isbn,
                book.Publisher,
                book.Model,
                book.Language,
                book.Reason,
                book.BuyPrice,
                book.BuyDate,
                book.Owner.Name,
                GetRatingAverage(book.Owner.Ratings),
                IsFavorite(book.Favorites, user),
                imagePath,
                categories
            );
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(nameof(BookService), nameof(GetBookById), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, List<MyBooksDto>>> GetMyBooks()
    {
        var failure = new Failure();

        try
        {
            var email = _user.GetEmailUserLogged();

            var user = await _userRepository.GetUserByEmail(email);

            var books = await _bookRepository.BookByUserId(user.Id);
            if (!books.Any())
            {
                failure.SetMessage("Usuário não possui livros cadastrados.");
                return failure;
            }

            var bookVM = new List<MyBooksDto>();

            books.ToList().ForEach(b =>
                bookVM.Add(new MyBooksDto()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Model = b.Model,
                    BuyPrice = b.BuyPrice,
                    Language = b.Language
                }));

            return bookVM;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(nameof(BookService), nameof(GetMyBooks), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, Guid>> CreateBook(CreateBookDto bookDto)
    {
        var failure = new Failure();
        var photos = new List<PhotosBook>();
        var bookCategories = new List<BookCategory>();
        string uri;
        string name;

        try
        {

            var email = _user.GetEmailUserLogged();

            var user = await _userRepository.GetUserByEmail(email);

            var book = new Book(
                bookDto.Title,
                bookDto.Author,
                bookDto.Isbn,
                bookDto.Publisher,
                bookDto.Model,
                bookDto.Language,
                bookDto.Reason,
                bookDto.BuyPrice,
                bookDto.BuyDate,
                user.Id
                );

            if (bookDto.Images.Any())
            {
                foreach (var image in bookDto.Images)
                {
                    name = GenerateImageName(book.Id);

                    uri = await BlobStorage.Upload(_blobConnectionString, _blobContainerName, image, name);

                    photos.Add(new PhotosBook(uri, name, book.Id));
                }
            }

            var categories = bookDto.IdCategory.Split(',').ToList();

            categories.ForEach(c => bookCategories.Add(new BookCategory(book.Id, Guid.Parse(c))));

            await _bookRepository.Create(book);

            await _photoRepository.CreateMany(photos);

            await _bookCategoryRepository.CreateMany(bookCategories);

            return book.Id;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(nameof(BookService), nameof(CreateBook), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, bool>> UpdateBook(UpdateBookDto bookDto)
    {
        var failure = new Failure();

        try
        {
            var book = new Book(
               bookDto.Title,
               bookDto.Author,
               bookDto.Isbn,
               bookDto.Publisher,
               bookDto.Model,
               bookDto.Language,
               bookDto.Reason,
               bookDto.BuyPrice,
               bookDto.BuyDate,
               bookDto.IdUser);

            await _bookRepository.Update(book);

            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(nameof(BookService), nameof(UpdateBook), ex.Message));

            return failure;
        }
    }

    public async Task<Attempt<Failure, bool>> DeleteBook(Guid idBook)
    {
        var failure = new Failure();

        try
        {
            var book = await _bookRepository.GetById(idBook);
            var bookCategory = await _bookCategoryRepository.GetByIdBook(idBook);
            var bookPhotos = await _photoRepository.GetByIdBook(idBook);

            if (book is null)
            {
                failure.SetMessage("Livro não encontrado.");
                return failure;
            }

            if (bookPhotos is not null)
                bookPhotos.ForEach(async p => await BlobStorage.Delete(_blobConnectionString, _blobContainerName, p.Name));

            _bookRepository.Delete(book);
            _bookCategoryRepository.DeleteMany(bookCategory);

            return true;
        }
        catch (Exception ex)
        {
            failure.SetMessage(ex.Message);

            await _log.Create(new LogError(nameof(BookService), nameof(DeleteBook), ex.Message));

            return failure;
        }
    }

    private static string GenerateImageName(Guid id)
    {
        return $"{Guid.NewGuid()}id{id}";
    }

    private static double GetRatingAverage(List<Rating> ratings)
    {
        return ratings.Count == 0
             ? 0.0
             : (double)ratings.Sum(r => r.Grade) / (double)ratings.ToArray().Length;
    }

    private static bool IsFavorite(List<Favorite> favorites, User user)
    {
        return user != null
            && favorites.Any(f => f.IdUser == user.Id);
    }
}
