using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity chi tiết đơn hàng
    // Quản lý thông tin chi tiết của từng sản phẩm trong đơn hàng
    public class OrderItem : BaseEntity
    {
        // ID đơn hàng
        public int OrderId { get; set; }
        
        // ID sản phẩm
        public int ProductId { get; set; }
        
        // Số lượng sản phẩm
        public int Quantity { get; set; }
        
        // Đơn vị tính
        public Unit Unit { get; set; }
        
        // Đơn giá sản phẩm tại thời điểm đặt hàng
        public int UnitPrice { get; set; }
        
        // Phí vận chuyển cho sản phẩm này
        public int ShippingFee { get; set; }
        
        // Thuế VAT cho sản phẩm này
        public int TaxAmount { get; set; }
        
        // Tổng giá trị của sản phẩm (số lượng * đơn giá) + VAT
        public int Subtotal { get; set; }
        
        // Trạng thái đóng gói
        public OrderItemStatus PackagingStatus { get; set; } = OrderItemStatus.Pending;
        
        // ID cửa hàng
        public int ShopId { get; set; }
        
        // Navigation properties - Các thuộc tính liên kết
        public Order? Order { get; set; }      // Thông tin đơn hàng
        public Product? Product { get; set; }  // Thông tin sản phẩm
        public Store? Shop { get; set; }       // Thông tin cửa hàng
    }
} 