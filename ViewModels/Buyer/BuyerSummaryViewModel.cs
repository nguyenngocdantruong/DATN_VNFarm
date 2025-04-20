using VNFarm_FinalFinal.DTOs.Response;

namespace VNFarm_FinalFinal.ViewModels.Buyer
{
    public class BuyerSummaryViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalPendingOrders { get; set; }
        public OrderResponseDTO? LatestOrder { get; set; } 
        public List<NotificationResponseDTO> Notifications { get; set; } = new();
    }
}
