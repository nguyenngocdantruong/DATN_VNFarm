using System.ComponentModel.DataAnnotations;

namespace VNFarm_FinalFinal.DTOs.Request
{
    public class UserRequestDTO : BaseRequestDTO
    {   
        #region Thông tin cơ bản
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; } = "";
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";
        
        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; } = "";
        public string? PasswordNew { get; set; }
        #endregion
        
        #region Thông tin địa chỉ
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string? Address { get; set; }
        #endregion
        
        #region Thông tin tài khoản
        public string? ImageUrl { get; set; }
        // Only for admin
        public bool? IsActive { get; set; }
        #endregion
        
        #region Cài đặt thông báo
        public bool? EmailNotificationsEnabled { get; set; }
        public bool? OrderStatusNotificationsEnabled { get; set; }
        public bool? DiscountNotificationsEnabled { get; set; }
        public bool? AdminNotificationsEnabled { get; set; }
        #endregion
        public IFormFile? ImageFile { get; set; }
    }
}