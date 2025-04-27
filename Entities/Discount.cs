using System;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    public class Discount : BaseEntity
    {
        // Mã code
        public string Code { get; set; } = "";
        // Mô tả
        public string Description { get; set; } = "";
        // Số lượng còn lại
        public int RemainingQuantity { get; set; }
        // Trạng thái
        public DiscountStatus Status { get; set; } = DiscountStatus.Active;
        // Ngày bắt đầu
        public DateTime StartDate { get; set; }
        // Ngày kết thúc
        public DateTime EndDate { get; set; }
        // Loại hình giảm giá
        public DiscountType Type { get; set; } = DiscountType.Percentage;
        // Số tiền giảm giá
        public decimal DiscountAmount { get; set; }
        // Số tiền tối thiểu để áp dụng
        public decimal MinimumOrderAmount { get; set; }
        // Số tiền giảm tối đa
        public decimal MaximumDiscountAmount { get; set; }
        
        // Can be null if global discount   
        public int? StoreId { get; set; }
        // Can be null if global discount
        public int? UserId { get; set; }
        
        // Navigation properties
        public virtual Store? Store { get; set; }
        public virtual User? User { get; set; }
    }
} 