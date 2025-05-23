using System;
using System.Collections.Generic;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity sản phẩm
    // Quản lý thông tin sản phẩm trong hệ thống
    public class Product : BaseEntity
    {
        // Tên sản phẩm
        public string Name { get; set; } = "";
        
        // Mô tả chi tiết sản phẩm
        public string Description { get; set; } = "";
        
        // Giá bán (đơn giá, đơn vị VNĐ)
        public int Price { get; set; }
        
        // URL ảnh chính của sản phẩm
        public string ImageUrl { get; set; } = "";
        
        // Số lượng tồn kho
        public int StockQuantity { get; set; }
        
        // Đơn vị tính (kg, hộp, etc)
        public Unit Unit { get; set; } = Unit.Kg;
        
        // Số lượng đã bán
        public int SoldQuantity { get; set; } = 0;
        
        // ID cửa hàng sở hữu sản phẩm
        public int StoreId { get; set; }
        
        // ID danh mục sản phẩm
        public int? CategoryId { get; set; }
        
        // Trạng thái hoạt động của sản phẩm
        public bool IsActive { get; set; } = true;
        
        // Xuất xứ sản phẩm
        public string Origin { get; set; } = "";
        // Đánh giá 
        public double AverageRating { get; set; }
        public int TotalSoldQuantity { get; set; }
        public int ReviewCount { get; set; }
        public int ReviewStar1Count {get;set;} = 0;
        public int ReviewStar2Count {get;set;} = 0;
        public int ReviewStar3Count {get;set;} = 0;
        public int ReviewStar4Count {get;set;} = 0;
        public int ReviewStar5Count {get;set;} = 0;

        // Navigation properties - Các thuộc tính liên kết
        public Store? Store { get; set; }                                    // Cửa hàng sở hữu
        public Category? Category { get; set; }                             // Danh mục sản phẩm
        public ICollection<Review>? Reviews { get; set; }                   // Đánh giá sản phẩm
    }
} 