using System.ComponentModel.DataAnnotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class OrderItemRequestDTO : BaseRequestDTO
    {
        // ID đơn hàng
        [Required]
        public int OrderId { get; set; }
        
        // ID sản phẩm
        [Required]
        public int ProductId { get; set; }
        
        // Số lượng sản phẩm
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
        
        // Đơn giá sản phẩm
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không được âm")]
        public decimal UnitPrice { get; set; }
        
        // ID cửa hàng
        [Required]
        public int ShopId { get; set; }
        
        // Trạng thái đóng gói
        public OrderItemStatus PackagingStatus { get; set; } = OrderItemStatus.Pending;
    }
} 