using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class DiscountRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Mã code là bắt buộc")]
        [StringLength(50, ErrorMessage = "Mã code không được vượt quá 50 ký tự")]
        public string Code { get; set; } = "";
        
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string Description { get; set; } = "";
        
        [Required(ErrorMessage = "Số lượng còn lại là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng còn lại phải lớn hơn hoặc bằng 0")]
        public int RemainingQuantity { get; set; }
        
        [EnumDataType(typeof(DiscountStatus))]
        public DiscountStatus Status { get; set; } = DiscountStatus.Active;
        
        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        
        [EnumDataType(typeof(DiscountType))]
        public DiscountType Type { get; set; } = DiscountType.Percentage;
        
        public int? StoreId { get; set; }
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Giá trị giảm giá là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị giảm giá phải lớn hơn 0")]
        public int DiscountAmount { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Giá trị đơn hàng tối thiểu phải lớn hơn hoặc bằng 0")]
        public int MinimumOrderAmount { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Giá trị giảm tối đa phải lớn hơn 0")]
        public int MaximumDiscountAmount { get; set; }
    }
}