namespace TT.Core.Domain.Entities;

public class PaginationResponse<T>
{
    public PaginationResponse()
    {}

    public PaginationResponse(IEnumerable<T> objects)
    {
        Objects = objects;
    }

    public IEnumerable<T> Objects { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int NextPage { get; set; }

    public int PreviusPage { get; set; }

    public int TotalPage { get; set; }

    public int TotalItems { get; set; }
}
