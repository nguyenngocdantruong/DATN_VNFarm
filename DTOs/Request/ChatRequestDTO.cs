using System;
using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace VNFarm_FinalFinal.DTOs.Request
{ 
    public class ChatRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "ID phòng chat là bắt buộc")]
        public int ChatRoomId { get; set; }
        [Required(ErrorMessage = "ID người gửi là bắt buộc")]
        public int SenderId { get; set; }
        [StringLength(500, ErrorMessage = "Nội dung tin nhắn không được vượt quá 500 ký tự")]
        public string Content { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public IFormFile? ImageFile { get; set; }
    }
}