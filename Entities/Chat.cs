using System;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity tin nhắn
    // Quản lý thông tin tin nhắn trong phòng chat
    public class Chat : BaseEntity
    {
        // ID người gửi tin nhắn
        public int SenderId { get; set; }
        
        // ID phòng chat
        public int ChatRoomId { get; set; }
        
        // Nội dung tin nhắn
        public string Content { get; set; } = string.Empty;
        
        // URL ảnh đính kèm (nếu có)
        public string ImageUrl { get; set; } = string.Empty;
        
        // Loại tin nhắn (Văn bản/Ảnh/Khiếu nại)
        public ChatMessageType Type { get; set; } = ChatMessageType.Text;
        
        // Navigation properties - Các thuộc tính liên kết
        public User? Sender { get; set; }     // Thông tin người gửi
        public ChatRoom? ChatRoom { get; set; } // Thông tin phòng chat
    }
} 