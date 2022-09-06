using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.ViewModels;

namespace TT.Core.Application.Interfaces;

public interface IBookService
{
    Task<PaginatedDto<BookshellDto>> GetBookshell(PaginationQuery paginationQuery);
    Task<BookDto> GetBookById(Guid idBook);
    Task<List<MyBooksDto>> GetMyBooks();
    Task<Guid> CreateBook(CreateBookDto bookDto);
    Task UpdateBook(UpdateBookDto bookDto);
    Task DeleteBook(Guid idBook);
}
