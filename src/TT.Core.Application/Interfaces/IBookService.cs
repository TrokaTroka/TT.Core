using TT.Core.Application.Dtos.Inputs;
using TT.Core.Application.Dtos.Inputs.Querys;
using TT.Core.Application.Dtos.ViewModels;
using TT.Core.Domain.Entities;

namespace TT.Core.Application.Interfaces;

public interface IBookService
{
    //Task<PaginatedDto<BookshellDto>> GetBookshell(PaginationQuery paginationQuery);
    /*Task<BookDto> GetBookById(Guid idBook);
    Task<List<MyBooksDto>> GetMyBooks();
    Task<Guid> CreateBook(CreateBookDto bookDto);
    Task UpdateBook(UpdateBookDto bookDto);
    Task DeleteBook(Guid idBook);*/
    Task<Attempt<Failure, PaginatedDto<BookshellDto>>> GetBookshell(PaginationQuery paginationQuery);
    Task<Attempt<Failure, BookDto>> GetBookById(Guid idBook);
    Task<Attempt<Failure, List<MyBooksDto>>> GetMyBooks();
    Task<Attempt<Failure, Guid>> CreateBook(CreateBookDto bookDto);
    Task<Attempt<Failure, bool>> UpdateBook(UpdateBookDto bookDto);
    Task<Attempt<Failure, bool>> DeleteBook(Guid idBook);
}
