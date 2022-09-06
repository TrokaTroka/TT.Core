using Microsoft.EntityFrameworkCore;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(TrokaTrokaDbContext context) : base(context)
    { }

    public async Task<PaginationResponse<Book>> GetBooks(int pageSize, int pageNumber, Guid idCategory)
    {
        var skip = (pageNumber - 1) * pageSize;
        var books = new List<Book>();
        int totalItems;

        if (idCategory != Guid.Empty)
        {
            books = await _contextSet
               .Include(c => c.Owner)
               .Include(b => b.PhotosBooks)
               .Include(u => u.Owner.Ratings)
               .Include(b => b.BookCategories)
               .Include(b => b.Favorites)
               .Where(b => b.BookCategories.Any(c => c.IdCategory == idCategory))
               .Skip(skip)
               .Take(pageSize)
               .ToListAsync();

            totalItems = await _contextSet
                .Where(b => b.BookCategories.Any(c => c.IdCategory == idCategory))
                .CountAsync();
        }
        else
        {
            books = await _contextSet
                .Include(c => c.Owner)
                .Include(b => b.PhotosBooks)
                .Include(u => u.Owner.Ratings)
                .Include(b => b.Favorites)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            totalItems = await _contextSet.CountAsync();
        }            
        
        decimal totalPage = totalItems / pageSize;

        return new PaginationResponse<Book>
        {
            Objects = books,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPage = Decimal.ToInt32(Math.Ceiling(Convert.ToDecimal(totalItems) / Convert.ToDecimal(pageSize))),
        };
    }

    public async Task<PaginationResponse<Book>> GetBookByUserId(int pageSize, int pageNumber, Guid idUser)
    {
        var skip = (pageNumber - 1) * pageSize;
        var books = new List<Book>();
        int totalItems;

        books = await _contextSet
              .Include(c => c.Owner)
              .Include(b => b.PhotosBooks)
              .Include(u => u.Owner.Ratings)
              .Include(b => b.BookCategories)
              .Include(b => b.Favorites)
              .Where(b => b.Favorites.Any(c => c.IdUser == idUser))
              .Skip(skip)
              .Take(pageSize)
              .ToListAsync();

        totalItems = await _contextSet
            .Where(b => b.Favorites.Any(c => c.IdUser == idUser))
            .CountAsync();

        decimal totalPage = totalItems / pageSize;

        return new PaginationResponse<Book>
        {
            Objects = books,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPage = Decimal.ToInt32(Math.Ceiling(Convert.ToDecimal(totalItems) / Convert.ToDecimal(pageSize))),
        };
    }

    public async Task<IEnumerable<Book>> BookByUserId(Guid idUser)
    {
        return await _context.Books
            .Where(b => b.IdUser == idUser)
            .ToListAsync();
    }

    public async Task<Book> GetBookById(Guid idBook)
    {
        return await _contextSet
               .Include(b => b.PhotosBooks)
               .Include(b => b.Owner)
               .ThenInclude(bo => bo.Ratings)
               .Include(b => b.Favorites)
               .Include(b => b.BookCategories)
               .ThenInclude(bc => bc.Category)
               .FirstOrDefaultAsync(b => b.Id == idBook);
    }
}
