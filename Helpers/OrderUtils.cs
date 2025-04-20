using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Helpers
{
    public static class OrderUtils
    {
        public static Dictionary<int, string> GetOrderStatusName()
        {
            return new Dictionary<int, string>
            {
                { (int)OrderStatus.Pending, "Chờ xác nhận" },
                { (int)OrderStatus.Processing, "Đang xử lý" },
                { (int)OrderStatus.Packaged, "Đã đóng gói" },
                { (int)OrderStatus.Shipping, "Đang vận chuyển" },
                { (int)OrderStatus.Delivered, "Đã giao hàng" },
                { (int)OrderStatus.Completed, "Đã hoàn thành" },
                { (int)OrderStatus.Cancelled, "Đã hủy" },
                { (int)OrderStatus.Refunded, "Đã hoàn tiền" },
                { (int)OrderStatus.Confirmed, "Đã xác nhận" },
            };
        }
        public static Dictionary<int, string> GetOrderEventTypeForForm()
        {
            return new Dictionary<int, string>
            {
                { (int)OrderEventType.OrderCreated, "Đơn hàng được tạo" },
                { (int)OrderEventType.OrderPaymentReceived, "Đã nhận được tiền" },
                { (int)OrderEventType.OrderAcceptedBySeller, "Đã được người bán xác nhận" },
                { (int)OrderEventType.OrderInvoiceCreated, "Đã tạo hóa đơn" },
                { (int)OrderEventType.OrderInvoiceSent, "Đã gửi hóa đơn cho khách hàng" },
                { (int)OrderEventType.OrderPackaging, "Đang đóng gói" },
                { (int)OrderEventType.OrderReadyToShip, "Đã sẵn sàng giao hàng" },
                { (int)OrderEventType.OrderShipped, "Đã giao hàng" },
                { (int)OrderEventType.OrderDelivered, "Đã giao hàng thành công" },
                { (int)OrderEventType.OrderCompleted, "Đơn hàng hoàn tất" },
                { (int)OrderEventType.OrderCancelled, "Đơn hàng đã bị hủy" },
                { (int)OrderEventType.OrderRefunded, "Đơn hàng đã được hoàn tiền" },
            };
        }
        public static string GetContentForOrderTimeline(OrderTimeline timeline)
        {
            var template = new Dictionary<OrderEventType, string>
            {
                { OrderEventType.OrderCreated, "Đơn hàng được tạo" },
                { OrderEventType.OrderPaymentReceived, "Đã nhận được tiền" },
                { OrderEventType.OrderAcceptedBySeller, "Đơn hàng được người bán xác nhận" },
                { OrderEventType.OrderInvoiceCreated, "Đã tạo hóa đơn" },
                { OrderEventType.OrderInvoiceSent, "Đã gửi hóa đơn cho khách hàng qua email" },
                { OrderEventType.OrderPackaging, "Đang đóng gói" },
                { OrderEventType.OrderReadyToShip, "Đã sẵn sàng giao hàng" },
                { OrderEventType.OrderShipped, "Đã giao hàng" },
                { OrderEventType.OrderDelivered, "Đã giao hàng thành công" },
                { OrderEventType.OrderCompleted, "Đơn hàng hoàn tất" },
                { OrderEventType.OrderCancelled, "Đơn hàng đã bị hủy" },
                { OrderEventType.OrderRefunded, "Đơn hàng đã được hoàn tiền" },
            };
            return template[timeline.EventType];
        }
        public static string GetIconForOrderTimeline(OrderTimelineStatus status)
        {
            return status switch
            {
                OrderTimelineStatus.Pending => "spinner-border spinner-border-sm text-warning",
                OrderTimelineStatus.Completed => "bx bx-check-circle text-success",
                OrderTimelineStatus.Cancelled => "bx bx-x-circle text-danger",
                _ => "bx bx-question-mark text-warning",
            };
        }
    }
}
