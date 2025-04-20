using System;
using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace VNFarm_FinalFinal.DTOs.Response
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