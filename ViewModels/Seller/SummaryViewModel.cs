using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.DTOs.Filters;

namespace VNFarm_FinalFinal.ViewModels.Seller
{
    public class SummaryViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalRevenue { get; set; }
        public int TotalReviews { get; set; }
        public OrderCriteriaFilter OrderCriteriaFilter { get; set; } = new();
        public List<OrderResponseDTO> RecentOrders { get; set; } = new();
        public List<NotificationResponseDTO> RecentNotifications { get; set; } = new();
        
    }
}
