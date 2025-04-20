using VNFarm_FinalFinal.DTOs.Response;

namespace VNFarm_FinalFinal.ViewModels.Common
{
    public class OrderDetailViewModel
    {
        public List<OrderTimelineResponseDTO> OrderTimelines { get; set; } = new();
        public UserResponseDTO? User { get; set; } 
        public OrderResponseDTO? Order { get; set; } 
    }
}
