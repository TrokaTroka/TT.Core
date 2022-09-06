using TT.Core.Domain.Entities;

namespace TT.Core.Domain.Interfaces.Repositories;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<Book> GetBookById(Guid idBook);
    Task<PaginationResponse<Book>> GetBooks(int pageSize, int pageNumber, Guid idCategory);
    Task<PaginationResponse<Book>> GetBookByUserId(int pageSize, int pageNumber, Guid idUser);
    Task<IEnumerable<Book>> BookByUserId(Guid idUser);
}
