using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class ChatRoomCriteriaFilter : BaseFilterCriteria
    {
        public ChatRoomType? Type { get; set; }
        public ChatRoomStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
    }
}

