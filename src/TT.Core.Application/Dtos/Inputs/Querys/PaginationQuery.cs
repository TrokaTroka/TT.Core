namespace TT.Core.Application.Dtos.Inputs.Querys;

public class PaginationQuery
{
    public PaginationQuery()
    {
        PageNumber = 1;
        PageSize = 12;
    }

    public PaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber == 0 ? 1 : pageNumber;
        PageSize = pageSize >= 100 ? 100 : pageSize;
    }
    public PaginationQuery(int pageNumber)
    {
        PageNumber = pageNumber == 0 ? 1 : pageNumber;
        PageSize = 12;
    }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public Guid Filter { get; set; }
}
