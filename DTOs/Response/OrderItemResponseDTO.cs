using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class OrderItemResponseDTO : BaseResponseDTO
    {
        // ID đơn hàng
        public int OrderId { get; set; }
        
        // ID sản phẩm
        public int ProductId { get; set; }
        
        // Số lượng sản phẩm
        public int Quantity { get; set; }
        
        // Đơn vị tính
        public Unit Unit { get; set; }
        
        // Đơn giá sản phẩm
        public decimal UnitPrice { get; set; }
        
        // Phí vận chuyển cho sản phẩm này
        public decimal ShippingFee { get; set; }
        
        // Thuế VAT cho sản phẩm này
        public decimal TaxAmount { get; set; }
        
        // Tổng giá trị của sản phẩm (số lượng * đơn giá) + VAT
        public decimal Subtotal { get; set; }
        
        // Trạng thái đóng gói
        public OrderItemStatus PackagingStatus { get; set; }
        
        // ID cửa hàng
        public int ShopId { get; set; }
        
        // Navigation properties - Các thuộc tính liên kết
        public ProductResponseDTO? Product { get; set; }  // Thông tin sản phẩm
        public StoreResponseDTO? Shop { get; set; }       // Thông tin cửa hàng
    }
} 