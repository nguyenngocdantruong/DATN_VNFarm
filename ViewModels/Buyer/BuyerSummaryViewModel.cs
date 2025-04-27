using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Buyer
{
    public class BuyerSummaryViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalPendingOrders { get; set; }
        public OrderResponseDTO? LatestOrder { get; set; } 
        public List<NotificationResponseDTO> Notifications { get; set; } = new();
    }
}
