using System;
using System.Collections.Generic;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Entities
{
    /// Entity phòng chat
    /// Quản lý thông tin phòng chat giữa người dùng và người bán
    public class ChatRoom : BaseEntity
    {
        public string NameRoom { get; set; } = "";
        public string Description { get; set; } = "";
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int? OrderId { get; set; }
        public ChatRoomType Type { get; set; } = ChatRoomType.ChatNormal;
        public ChatRoomStatus Status { get; set; } = ChatRoomStatus.InProgress;
        public string LastMessage { get; set; } = "";
        public DateTime? LastMessageTime { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public User? Buyer { get; set; }
        public User? Seller { get; set; }
        public Order? Order { get; set; }
        public ICollection<Chat>? Messages { get; set; } = [];
    }
} 