using System.ComponentModel.DataAnnotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class StoreRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Tên cửa hàng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên cửa hàng không được vượt quá 100 ký tự")]
        public required string Name { get; set; }
        
        [Required(ErrorMessage = "Mô tả cửa hàng là bắt buộc")]
        [StringLength(500, ErrorMessage = "Mô tả cửa hàng không được vượt quá 500 ký tự")]
        public required string Description { get; set; }
        public string? LogoUrl { get; set; }
        
        [Required(ErrorMessage = "Địa chỉ cửa hàng là bắt buộc")]
        [StringLength(255, ErrorMessage = "Địa chỉ cửa hàng không được vượt quá 255 ký tự")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Số điện thoại cửa hàng là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20, ErrorMessage = "Số điện thoại cửa hàng không được vượt quá 20 ký tự")]
        public required string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Email cửa hàng là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email cửa hàng không được vượt quá 100 ký tự")]
        public required string Email { get; set; }
        
        [EnumDataType(typeof(StoreType))]
        public StoreType? BusinessType { get; set; }
        public bool? IsActive { get; set; }
        public StoreStatus? VerificationStatus { get; set; }
        public IFormFile? LogoFile { get; set; }
        public int UserId { get; set; } = -1;
    }
}