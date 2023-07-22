namespace Contacts.Api.Infrastructure.Repositories;

public class PaginationMetadata
{
    public int TotalItemCount { get; init; }
    public int TotalPageCount { get; init; }
    public int PageSize { get; init; }
    public int CurrentPage { get; init; }

    public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
    {
        TotalItemCount = totalItemCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
    }
}