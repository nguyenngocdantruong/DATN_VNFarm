using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Helpers
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
        public static Dictionary<int, object> GetOrderEventTypeForForm()
        {
            return new Dictionary<int, object>
            {
                { (int)OrderEventType.OrderCreated, new {
                    value = "Đơn hàng được tạo",
                    complete = true
                } },
                { (int)OrderEventType.OrderPaymentReceived, new {
                    value = "Đã thanh toán",
                    complete = false
                } },
                { (int)OrderEventType.OrderAcceptedBySeller, new {
                    value = "Đơn hàng được người bán xác nhận",
                    complete = false
                } },
                { (int)OrderEventType.OrderReadyToShip, new {
                    value = "Đã sẵn sàng giao hàng",
                    complete = false
                } },
                { (int)OrderEventType.OrderShipped, new {
                    value = "Đã giao hàng",
                    complete = false
                } },
                { (int)OrderEventType.OrderCompleted, new {
                    value = "Đơn hàng hoàn tất",
                    complete = true
                } },
                { (int)OrderEventType.OrderCancelled, new {
                    value = "Đơn hàng đã bị hủy",
                    complete = true
                } },
                { (int)OrderEventType.OrderRefunded, new {
                    value = "Đơn hàng đã được hoàn tiền",
                    complete = true
                } },
                { (int)OrderEventType.OrderShippingUpdated, new {
                    value = "Cập nhật thông tin vận chuyển",
                    complete = false
                } },
                { (int)OrderEventType.OrderAddressUpdated, new {
                    value = "Cập nhật địa chỉ giao hàng",
                    complete = false
                } },
            };
        }
        public static string GetContentForOrderTimeline(OrderTimeline timeline)
        {
            var template = new Dictionary<OrderEventType, string>
            {
                { OrderEventType.OrderCreated, "Đơn hàng được tạo" },
                { OrderEventType.OrderPaymentReceived, "Đã nhận được tiền" },
                { OrderEventType.OrderAcceptedBySeller, "Đơn hàng được người bán xác nhận" },
                { OrderEventType.OrderReadyToShip, "Đã sẵn sàng giao hàng" },
                { OrderEventType.OrderShipped, "Đã giao hàng" },
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
        public static Dictionary<int, string> GetOrderTimeLineStatus(){
            return new Dictionary<int, string>
            {
                { (int)OrderTimelineStatus.Pending, "Đang thực hiện" },
                { (int)OrderTimelineStatus.Completed, "Đã hoàn thành" },
                { (int)OrderTimelineStatus.Cancelled, "Đã hủy" },
            };
        }

        public static Dictionary<int, string> GetOrderItemStatusName()
        {
            return new Dictionary<int, string>
            {
                { (int)OrderItemStatus.Pending, "Chờ xử lý" },
                { (int)OrderItemStatus.Processing, "Đang xử lý" },
                { (int)OrderItemStatus.Packed, "Đã đóng gói" },
                { (int)OrderItemStatus.Shipped, "Đang vận chuyển" },
                { (int)OrderItemStatus.Delivered, "Đã giao hàng" },
                { (int)OrderItemStatus.Cancelled, "Đã hủy" },
                { (int)OrderItemStatus.Returned, "Đã trả hàng" },
            };
        }
        
    }
}
