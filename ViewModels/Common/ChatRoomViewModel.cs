using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Common
{
    public class ChatRoomViewModel
    {
        public ChatRoomCriteriaFilter ChatRoomCriteriaFilter { get; set; } = new();
        public List<ChatRoomResponseDTO> ChatRooms { get; set; } = new();
    }
}
