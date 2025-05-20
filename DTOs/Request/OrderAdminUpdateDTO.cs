using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class OrderAdminUpdateDTO
    {
        public OrderStatus? Status { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
} 