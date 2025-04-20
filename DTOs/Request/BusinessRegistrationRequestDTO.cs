using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Request
{
    public class BusinessRegistrationRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "ID người dùng không hợp lệ !")]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Tên doanh nghiệp là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên doanh nghiệp không được vượt quá 100 ký tự")]
        public string BusinessName { get; set; } = string.Empty;
        
        [EnumDataType(typeof(StoreType))]
        public StoreType BusinessType { get; set; } = StoreType.Farmer;
        
        [StringLength(20, ErrorMessage = "Mã số thuế không được vượt quá 20 ký tự")]
        public string TaxCode { get; set; } = string.Empty;
        public string BusinessLicenseUrl { get; set; } = string.Empty;
        
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Address { get; set; } = string.Empty;
        public IFormFile? BusinessLicenseFile { get; set; }
    }
}