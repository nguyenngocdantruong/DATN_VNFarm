using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity chi tiết đơn hàng
    // Quản lý thông tin chi tiết của từng sản phẩm trong đơn hàng
    public class OrderDetail : BaseEntity
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
        
        // Thuế VAT cho sản phẩm này (%)
        public decimal TaxAmount { get; set; }
        
        // Tổng giá trị của sản phẩm (số lượng * đơn giá) + VAT
        public decimal Subtotal { get; set; }
        // Trạng thái đóng gói
        public OrderDetailStatus PackagingStatus { get; set; } = OrderDetailStatus.Pending;
        
        // Navigation properties - Các thuộc tính liên kết
        // public virtual Order Order { get; set; }      // Thông tin đơn hàng
        public virtual Product? Product { get; set; }  // Thông tin sản phẩm
    }
} 