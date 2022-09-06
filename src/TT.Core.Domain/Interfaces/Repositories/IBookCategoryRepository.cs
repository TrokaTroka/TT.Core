using TT.Core.Domain.Entities;

namespace TT.Core.Domain.Interfaces.Repositories;

public interface IBookCategoryRepository : IBaseRepository<BookCategory>
{
    Task<List<BookCategory>> GetByIdBook(Guid idBook);
    Task CreateMany(List<BookCategory> bookCategories);
    void DeleteMany(List<BookCategory> bookCategories);
}
