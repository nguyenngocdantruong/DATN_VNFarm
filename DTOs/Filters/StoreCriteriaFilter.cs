using VNFarm.DTOs.Filters;
using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class StoreCriteriaFilter : BaseFilterCriteria
    {
        public StoreType? Type { get; set; } = StoreType.All;
        public StoreStatus? Status { get; set; } = StoreStatus.All;
        public int? MinRating { get; set; } = 0;
        public int? MaxRating { get; set; } = 5;
    }
} 