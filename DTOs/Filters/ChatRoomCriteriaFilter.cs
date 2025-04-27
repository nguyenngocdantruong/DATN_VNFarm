using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class ChatRoomCriteriaFilter : BaseFilterCriteria
    {
        public ChatRoomType? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

