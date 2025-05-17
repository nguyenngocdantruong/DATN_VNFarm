using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class ContactRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(50, ErrorMessage = "Họ và tên không được vượt quá 50 ký tự")]
        public string FullName { get; set; } = "";
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [StringLength(200, ErrorMessage = "Email không được vượt quá 200 ký tự")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";
        
        [Required(ErrorMessage = "Loại dịch vụ là bắt buộc")]
        public string ServiceType { get; set; } = "";
        
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; } = "";
        
        [Required(ErrorMessage = "Nội dung tin nhắn là bắt buộc")]
        [StringLength(500, ErrorMessage = "Nội dung tin nhắn không được vượt quá 500 ký tự")]
        public string Message { get; set; } = "";
    }
} 