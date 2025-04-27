using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class DiscountCriteriaFilter : BaseFilterCriteria
    {
        public DiscountStatus Status { get; set; } = DiscountStatus.All;
        public DiscountType Type { get; set; } = DiscountType.All;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

