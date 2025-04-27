using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Admin
{
    public class OrderListsViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalRevenue { get; set; }
        public int TotalCanceledOrders { get; set; }
        public int TotalRefundedOrders { get; set; }
        public int TotalCompletedOrders { get; set; }
        public int TotalPendingOrders { get; set; }
        public int TotalProcessingOrders { get; set; }
        public int TotalShippedOrders { get; set; }
        public int TotalDeliveredOrders { get; set; }
        public OrderCriteriaFilter OrderCriteriaFilter { get; set; } = new();
        public List<OrderResponseDTO> Orders { get; set; } = new();
    }
}
