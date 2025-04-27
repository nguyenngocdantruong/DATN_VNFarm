using System.Collections.Generic;

namespace VNFarm.Entities
{
    // Entity danh mục
    // Quản lý thông tin danh mục sản phẩm
    public class Category : BaseEntity
    {
        // Tên danh mục
        public string Name { get; set; } = "";
        // Giá thấp nhất
        public decimal MinPrice { get; set; }
        // Giá cao nhất
        public decimal MaxPrice { get; set; }
        // Mô tả danh mục
        public string Description { get; set; } = "";
        
        // URL icon của danh mục
        public string IconUrl { get; set; } = "";

        // Navigation properties - Các thuộc tính liên kết
        public ICollection<Product> Products { get; set; } = [];      // Danh sách sản phẩm
    }
} 