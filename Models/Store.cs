using System;
using System.Collections.Generic;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Entities
{
    // Entity cửa hàng
    // Quản lý thông tin cửa hàng của người bán
    public class Store : BaseEntity
    {
        // Tên cửa hàng
        public string Name { get; set; } = "";
        
        // Mô tả cửa hàng
        public string Description { get; set; } = "";
        
        // Logo cửa hàng
        public string LogoUrl { get; set; } = "";
        
        // Địa chỉ cửa hàng
        public string Address { get; set; } = "";
        
        // Số điện thoại liên hệ
        public string PhoneNumber { get; set; } = "";
        
        // Email liên hệ
        public string Email { get; set; } = "";
        
        // Loại cửa hàng
        public StoreType BusinessType { get; set; } = StoreType.Farmer;
        
        // Trạng thái hoạt động của cửa hàng
        public bool IsActive { get; set; } = true;
        
        // Trạng thái xác thực của cửa hàng
        public StoreStatus VerificationStatus { get; set; } = StoreStatus.Pending;
        
        // Điểm đánh giá trung bình của cửa hàng
        public decimal AverageRating { get; set; } = 5;
        
        // Số lượt đánh giá của cửa hàng
        public int ReviewCount { get; set; } = 0;

        // Foreign keys
        public int UserId { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public ICollection<Discount>? Discounts { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
} 