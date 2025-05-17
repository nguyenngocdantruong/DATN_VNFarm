using System.ComponentModel.DataAnnotations;

namespace VNFarm.Entities
{
    // Entity ContactRequest
    // Quản lý thông tin yêu cầu liên hệ từ người dùng
    public class ContactRequest : BaseEntity
    {
        // Họ và tên người liên hệ
        [Required]
        [StringLength(50)]
        public string FullName { get; set; } = "";
        
        // Email của người liên hệ
        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = "";
        
        // Loại dịch vụ yêu cầu
        [Required]
        public string ServiceType { get; set; } = "";
        
        // Số điện thoại liên hệ
        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = "";
        
        // Nội dung tin nhắn
        [Required]
        [StringLength(500)]
        public string Message { get; set; } = "";
    }
} 