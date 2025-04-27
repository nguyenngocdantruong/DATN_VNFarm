using System;
using System.ComponentModel.DataAnnotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class NotificationRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Mã người dùng không được để trống")]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Nội dung không được để trống")]
        [StringLength(500, ErrorMessage = "Nội dung thông báo không được vượt quá 500 ký tự")]
        public string Content { get; set; } = "";
        
        [StringLength(255, ErrorMessage = "Liên kết không được vượt quá 255 ký tự")]
        public string LinkUrl { get; set; } = "";
        
        [EnumDataType(typeof(NotificationType))]
        public NotificationType Type { get; set; } = NotificationType.System;
        public bool IsRead { get; set; } = false;
    }
}