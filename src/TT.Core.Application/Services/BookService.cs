using Microsoft.Extensions.Configuration;
using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Application.Interfaces;
using TT.Core.Application.Notifications;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Integration.Azure;

namespace TT.Core.Application.Services;

public class BookService : BaseService, IBookService
{
    private readonly IAuthenticatedUser _user;
    private readonly IBookRepository _bookRepository;
    private readonly IPhotoRepository _photoRepository;
    private readonly IBookCategoryRepository _bookCategoryRepository;
    private readonly IUserRepository _userRepository;

    private readonly string _blobConnectionString;
    private const string _blobContainerName = "books";

    public BookService(IBookRepository bookRepository,
        IBookCategoryRepository bookCategoryRepository,
        IPhotoRepository photoRepository,
        IUserRepository userRepository,
        IAuthenticatedUser user,
        IConfiguration configuration,
        INotifier notifier) : base(notifier)
    {
        _bookRepository = bookRepository;
        _bookCategoryRepository = bookCategoryRepository;
        _photoRepository = photoRepository;
        _userRepository = userRepository;
        _user = user;
        _blobConnectionString = configuration.GetConnectionString("BlobStorage");
    }

    public async Task<PaginatedDto<BookshellDto>> GetBookshell(PaginationQuery paginationQuery)
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

    public async Task<BookDto> GetBookById(Guid idBook)
    {
        var email = _user.GetEmailUserLogged();

        var user = await _userRepository.GetUserByEmail(email);

        var book = await _bookRepository.GetBookById(idBook);

        if (book is null)
            return null;

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

    public async Task<List<MyBooksDto>> GetMyBooks()
    {
        var email = _user.GetEmailUserLogged();

        var user = await _userRepository.GetUserByEmail(email);

        var books = await _bookRepository.BookByUserId(user.Id);

        if (!books.Any())
        {
            Notify("Usuário não possui livros cadastrados.");
            return null;
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

    public async Task<Guid> CreateBook(CreateBookDto bookDto)
    {
        var photos = new List<PhotosBook>();
        var bookCategories = new List<BookCategory>();
        string uri;
        string name;

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

    public async Task UpdateBook(UpdateBookDto bookInput)
    {
        var book = new Book(
            bookInput.Title,
            bookInput.Author,
            bookInput.Isbn,
            bookInput.Publisher,
            bookInput.Model,
            bookInput.Language,
            bookInput.Reason,
            bookInput.BuyPrice,
            bookInput.BuyDate,
            bookInput.IdUser);

        await _bookRepository.Update(book);
    }

    public async Task DeleteBook(Guid idBook)
    {
        var book = await _bookRepository.GetById(idBook);
        var bookCategory = await _bookCategoryRepository.GetByIdBook(idBook);
        var bookPhotos = await _photoRepository.GetByIdBook(idBook);

        if (book is null)
        {
            Notify("Livro não encontrado");
            return;
        }

        if(bookPhotos is not null)
            bookPhotos.ForEach(async p => await BlobStorage.Delete(_blobConnectionString, _blobContainerName, p.Name));

        _bookRepository.Delete(book);
        _bookCategoryRepository.DeleteMany(bookCategory);
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
