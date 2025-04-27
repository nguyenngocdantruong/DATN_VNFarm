using System;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity thông báo
    // Quản lý thông tin thông báo cho người dùng
    public class Notification : BaseEntity
    {
        // ID người dùng nhận thông báo
        public int UserId { get; set; }
        
        // Nội dung thông báo
        public string Content { get; set; } = "";
        
        // URL liên kết của thông báo
        public string LinkUrl { get; set; } = "";
        
        // Loại thông báo
        public NotificationType Type { get; set; }
        
        // Trạng thái đã đọc của thông báo
        public bool IsRead { get; set; } = false;
    }
} 