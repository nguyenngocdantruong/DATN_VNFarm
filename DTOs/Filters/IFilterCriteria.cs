using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Filters
{
    public interface IFilterCriteria
    {
        string SearchTerm { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        SortType SortBy { get; set; }
    }
} 