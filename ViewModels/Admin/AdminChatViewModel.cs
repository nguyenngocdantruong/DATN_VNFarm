using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Admin
{
    public class AdminChatViewModel
    {
        public List<ChatRoomResponseDTO> ChatRooms { get; set; } = new();
    }
    
    public class ChatRoomDetailViewModel
    {
        public ChatRoomResponseDTO? ChatRoom { get; set; }
        public List<VNFarm.Entities.Chat> Chats { get; set; } = new();
        public UserResponseDTO? BuyerInfo { get; set; }
        public UserResponseDTO? SellerInfo { get; set; }
    }
} 