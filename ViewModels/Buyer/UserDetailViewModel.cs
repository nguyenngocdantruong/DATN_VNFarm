using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Buyer
{
    public class UserDetailViewModel
    {
        public StoreResponseDTO? Store { get; set; } 
        public UserResponseDTO? User { get; set; } 
        public OrderCriteriaFilter OrderCriteriaFilter { get; set; } = new();
        public List<OrderResponseDTO> Orders { get; set; } = new();
    }
}
