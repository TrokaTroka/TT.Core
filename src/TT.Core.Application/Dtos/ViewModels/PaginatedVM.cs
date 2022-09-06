namespace TT.Core.Application.Dtos.ViewModels;

public class PaginatedDto<T>
{
    public PaginatedDto(IEnumerable<T> objects, int totalPage, int totalItem, int pageNumber, int pageSize)
    {
        Objects = objects;
        TotalPage = totalPage;
        TotalItem = totalItem;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public PaginatedDto()
    {

    }

    public int TotalPage { get; set; }

    public int TotalItem { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public bool HasPreviousPage { get; set; }

    public bool HasNextPage { get; set; }

    public bool IsFirstPage { get; set; }

    public bool IsLastPage { get; set; }

    public IEnumerable<T> Objects { get; set; }
}
