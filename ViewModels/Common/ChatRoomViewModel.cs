using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.DTOs.Filters;

namespace VNFarm_FinalFinal.ViewModels.Common
{
    public class ChatRoomViewModel
    {
        public ChatRoomCriteriaFilter ChatRoomCriteriaFilter { get; set; } = new();
        public List<ChatRoomResponseDTO> ChatRooms { get; set; } = new();
    }
}
