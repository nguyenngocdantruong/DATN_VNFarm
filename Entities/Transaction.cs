using System;
using VNFarm.Helpers;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity giao dịch
    // Quản lý thông tin giao dịch thanh toán
    public class Transaction : BaseEntity
    {
        public string TransactionCode { get; set; } = string.Empty;
        // ID đơn hàng liên quan
        public int OrderId { get; set; }
        
        // ID người dùng thực hiện giao dịch
        public int BuyerId { get; set; }
        
        // Số tiền giao dịch
        public decimal Amount { get; set; }
        
        // Phương thức thanh toán
        public PaymentMethodEnum PaymentMethod { get; set; }
        
        // Trạng thái giao dịch
        public TransactionStatus Status { get; set; }
        // Thời hạn cuối cùng thanh toán
        public DateTime? PaymentDueDate { get; set; }
        // Ngày đã thanh toán
        public DateTime? PaymentDate { get; set; }
        // Thông tin bổ sung thêm về giao dịch
        public string Details { get; set; } = string.Empty;
        
        // Navigation properties - Các thuộc tính liên kết
        public Order? Order { get; set; }     // Thông tin đơn hàng
        public User? Buyer { get; set; }       // Thông tin người dùng
    }
} 