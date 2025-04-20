using System;
using System.ComponentModel.DataAnnotations.Schema;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Entities
{
    // Entity lịch sử đơn hàng
    // Quản lý thông tin các sự kiện trong quá trình xử lý đơn hàng
    public class OrderTimeline : BaseEntity
    {
        // ID đơn hàng
        public int OrderId { get; set; }
        // Loại sự kiện
        public OrderEventType EventType { get; set; } = OrderEventType.OrderCreated;
        // Trạng thái sự kiện
        public OrderTimelineStatus Status { get; set; } = OrderTimelineStatus.Pending;
        
        // Mô tả sự kiện
        public string Description { get; set; } = "";
        
        // Navigation properties - Các thuộc tính liên kết
        public Order? Order { get; set; }     // Thông tin đơn hàng
    }
} 