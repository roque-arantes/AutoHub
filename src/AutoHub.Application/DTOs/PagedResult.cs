namespace AutoHub.Application.DTOs;

/// <summary>
/// Envelope de paginação genérico para listagens v2.
/// </summary>
public class PagedResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
    public IEnumerable<T> Items { get; set; } = [];
}
