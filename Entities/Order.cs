using System;
using System.Collections.Generic;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity đơn hàng
    // Quản lý thông tin đơn hàng của người dùng
    public class Order : BaseEntity
    {
        #region Thông tin chung
        // Mã đơn hàng
        public string OrderCode { get; set; } = "";
        // Trạng thái đơn hàng
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        // Ghi chú đơn hàng
        public string Notes { get; set; } = "";
        #endregion
        
        #region Thông tin giá cả
        // Tổng giá trị đơn hàng (Tạm tính)
        public decimal TotalAmount { get; set; }
        
        // Phí vận chuyển
        public decimal ShippingFee { get; set; }
        
        // Thuế VAT
        public decimal TaxAmount { get; set; }
        
        // Số tiền được giảm giá
        public decimal DiscountAmount { get; set; }
        
        // Tổng số tiền phải thanh toán
        public decimal FinalAmount { get; set; }
        #endregion
        
        #region Thông tin thanh toán
        // Trạng thái thanh toán
        public long OrderPaymentId { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
        
        // Phương thức thanh toán
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.BankTransfer;
        
        // Thời điểm thanh toán
        public DateTime? PaidAt { get; set; }

        #endregion
        
        #region Thông tin vận chuyển
         // Mã vận đơn
        public string TrackingNumber { get; set; } = "";
        
        // Phương thức vận chuyển
        public string ShippingMethod { get; set; } = "";
        
        // Đơn vị vận chuyển
        public string ShippingPartner { get; set; } = "";
        // Thời điểm bắt đầu vận chuyển
        public DateTime? ShippedAt { get; set; }
        
        // Thời điểm giao hàng thành công
        public DateTime? DeliveredAt { get; set; }
        
        // Thời điểm hủy đơn hàng
        public DateTime? CancelledAt { get; set; }
        #endregion

        #region Thông tin địa chỉ giao hàng
        public string ShippingName { get; set; } = "";      // Tên người nhận
        public string ShippingPhone { get; set; } = "";     // Số điện thoại người nhận
        public string ShippingAddress { get; set; } = "";   // Địa chỉ chi tiết
        public string ShippingProvince { get; set; } = "";  // Tỉnh/Thành phố
        public string ShippingDistrict { get; set; } = "";  // Quận/Huyện
        public string ShippingWard { get; set; } = "";      // Phường/Xã
        #endregion

        // Foreign keys - Khóa ngoại
        public int BuyerId { get; set; }              // ID người mua
        public int? DiscountId { get; set; }          // ID mã giảm giá

        // Navigation properties - Các thuộc tính liên kết
        public User? Buyer { get; set; }                // Thông tin người mua
        public Discount? Discount { get; set; }         // Thông tin giảm giá
        public ICollection<OrderItem> OrderItems { get; set; } = [];  // Chi tiết đơn hàng
        public ICollection<OrderTimeline> OrderTimelines { get; set; } = [];    // Lịch sử đơn hàng
        public Transaction? Transaction { get; set; }   
    }
} 