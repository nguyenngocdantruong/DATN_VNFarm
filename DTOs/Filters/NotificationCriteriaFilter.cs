namespace VNFarm.DTOs.Filters
{
    public class NotificationCriteriaFilter : BaseFilterCriteria
    {
        public int? UserId { get; set; }
        public int? StoreId { get; set; }
    }
}
