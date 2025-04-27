using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{ 
    public class ChatResponseDTO : BaseResponseDTO
    {
        public required int ChatRoomId { get; set; }
        public ChatRoomResponseDTO? ChatRoom { get; set; }
        public required int SenderId { get; set; }
        public UserResponseDTO? Sender { get; set; }
        public required string Content { get; set; }
        public required ChatMessageType Type { get; set; }
        public string? ImageUrl { get; set; }
    }
}