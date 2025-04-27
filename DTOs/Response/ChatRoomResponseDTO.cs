using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VNFarm.Entities;
using Swashbuckle.AspNetCore.Annotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class ChatRoomResponseDTO : BaseResponseDTO
    {
        public required string NameRoom { get; set; }
        public required string Description { get; set; }
        public required ChatRoomStatus Status { get; set; }
        public required ChatRoomType Type { get; set; }
        public required string LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public bool IsActive { get; set; }
        public required int BuyerId { get; set; }
        public required int SellerId { get; set; }
        public int? OrderId { get; set; }
        public ICollection<ChatResponseDTO> Messages { get; set; } = new List<ChatResponseDTO>();
    }
}