using Microsoft.EntityFrameworkCore;
using TT.Core.Domain.Entities;
using TT.Core.Domain.Interfaces.Repositories;
using TT.Core.Infra.Data.Context;

namespace TT.Core.Infra.Data.Repositories;

public class BookCategoryRepository : BaseRepository<BookCategory>, IBookCategoryRepository
{
    public BookCategoryRepository(TrokaTrokaDbContext context) : base(context)
    { }

    public async Task<List<BookCategory>> GetByIdBook(Guid idBook)
    {
        return await _context.BookCategories.Where(c => c.IdBook == idBook).ToListAsync();
    }

    public async Task CreateMany(List<BookCategory> bookCategories)
    {
        await _context.BookCategories.AddRangeAsync(bookCategories);
        await _context.SaveChangesAsync();
    }

    public void DeleteMany(List<BookCategory> bookCategories)
    {
        _context.BookCategories.RemoveRange(bookCategories);
        _context.SaveChanges();
    }
}
