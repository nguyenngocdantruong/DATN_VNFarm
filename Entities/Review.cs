using System;

namespace VNFarm.Entities
{
    // Entity đánh giá
    // Quản lý thông tin đánh giá sản phẩm từ người dùng
    public class Review : BaseEntity
    {
        // ID người dùng đánh giá
        public int UserId { get; set; }
        
        // ID sản phẩm được đánh giá
        public int ProductId { get; set; }
        
        // Điểm đánh giá (1-5 sao)
        public int Rating { get; set; }
        
        // Nội dung đánh giá
        public string Content { get; set; } = string.Empty;
        
        // Phản hồi từ cửa hàng
        public string ShopResponse { get; set; } = string.Empty;
        
        // URL ảnh đánh giá
        public string ImageUrl { get; set; } = string.Empty;
        
        
        // Navigation properties - Các thuộc tính liên kết
        public virtual User? User { get; set; }      // Thông tin người đánh giá
    }
} 