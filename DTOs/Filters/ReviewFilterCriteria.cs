namespace VNFarm.DTOs.Filters
{
    public class ReviewFilterCriteria : BaseFilterCriteria
    {
        public int? ProductId { get; set; }
        public int? UserId { get; set; }
        public int? OrderId { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? HasImage { get; set; }
        public bool? HasContent { get; set; }
    }
}
