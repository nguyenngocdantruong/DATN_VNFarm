using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace VNFarm_FinalFinal.DTOs.Request
{
    public class ChatRoomRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Tiêu đề phòng chat là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tiêu đề phòng chat không được vượt quá 100 ký tự")]
        public string NameRoom { get; set; } = "";
        
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string Description { get; set; } = "";
        
        [EnumDataType(typeof(ChatRoomStatus))]
        public ChatRoomStatus Status { get; set; } = ChatRoomStatus.InProgress;
        
        [EnumDataType(typeof(ChatRoomType))]
        public ChatRoomType Type { get; set; } = ChatRoomType.ChatNormal;
        public bool? IsActive { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int? OrderId { get; set; }
    }
}