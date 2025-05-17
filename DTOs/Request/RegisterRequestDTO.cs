using System.ComponentModel.DataAnnotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; } = "";
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";
        
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = "";
        
        [Required(ErrorMessage = "Vai trò người dùng là bắt buộc")]
        public UserRole UserRole { get; set; } = UserRole.User;
        // Thông tin cửa hàng (chỉ áp dụng khi đăng ký là người bán)
        public string? StoreName { get; set; }
        
        public string? StoreDescription { get; set; }
        
        public string? StoreAddress { get; set; }
        
        public string? StorePhoneNumber { get; set; }
        
        public string? StoreEmail { get; set; }
        
        public StoreType BusinessType { get; set; } = StoreType.Farmer;
        
        public IFormFile? LogoFile { get; set; }
    }
} 