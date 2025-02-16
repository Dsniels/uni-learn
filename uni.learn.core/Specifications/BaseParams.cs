
namespace uni.learn.core.Specifications;

public class BaseParams
{
    public int Sort { get; set; }
    public int PageIndex { get; set; } = 1;
    private const int maxPageSize = 20;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize; set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
    public string? Search { get; set; }

}
