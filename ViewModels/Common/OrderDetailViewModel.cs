using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Common
{
    public class OrderDetailViewModel
    {
        public List<OrderTimelineResponseDTO> OrderTimelines { get; set; } = new();
        public UserResponseDTO? User { get; set; } 
        public OrderResponseDTO? Order { get; set; } 
    }
}
