using VNFarm.Enums;

namespace DTOs.Request;

// Class DTO cho cập nhật trạng thái OrderItem
public class OrderItemStatusUpdateDTO
{
    public required OrderItemStatus Status { get; set; }
}

