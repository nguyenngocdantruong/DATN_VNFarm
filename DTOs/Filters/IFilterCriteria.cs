using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public interface IFilterCriteria
    {
        string SearchTerm { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        SortType SortBy { get; set; }
    }
} 