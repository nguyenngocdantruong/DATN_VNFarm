namespace VNFarm.Enums
{
    public enum OrderPaymentStatusFilter
    {
        All = 0,
        Pending = 1,
        Paid = 2,
        Refunded = 3,
    }
    public enum OrderStatus
    {
        All = -999,
        TempCart = -1,
        Pending = 0,
        Processing = 1,
        Packaged = 2,
        Shipping = 3,
        Delivered = 4,
        Completed = 5,
        Cancelled = 6,
        Refunded = 7,
        Confirmed = 8
    }
    public enum OrderDetailStatus
    {
        Pending = 0,
        Packaging = 1,
        ReadyToShip = 2,
    }
    public enum OrderEventType
    {
        OrderCreated = 0, // Đơn hàng được tạo
        OrderPaymentReceived = 1, // Đã nhận được tiền
        OrderAcceptedBySeller = 2, // Đã được người bán xác nhận
        OrderInvoiceCreated = 3, // Đã tạo hóa đơn
        OrderInvoiceSent = 4, // Đã gửi hóa đơn cho khách hàng
        OrderPackaging = 5, // Đang đóng gói
        OrderReadyToShip = 6, // Đã sẵn sàng giao hàng
        OrderShipped = 7, // Đã giao hàng
        OrderCompleted = 9, // Đơn hàng hoàn tất
        OrderCancelled = 10, // Đơn hàng đã bị hủy
        OrderRefunded = 11, // Đơn hàng đã được hoàn tiền
    }
    public enum OrderTimelineStatus
    {
        Pending = 0,
        Completed = 1,
        Cancelled = 2,
    }
    public enum OrderItemStatus
    {
        Pending = 0,
        Processing = 1,
        Packed = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Returned = 6,
        All = -999
    }
} 