using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.DTOs.Filters;

namespace VNFarm_FinalFinal.ViewModels.Buyer
{
    public class UserDetailViewModel
    {
        public StoreResponseDTO? Store { get; set; } 
        public UserResponseDTO? User { get; set; } 
        public OrderCriteriaFilter OrderCriteriaFilter { get; set; } = new();
        public List<OrderResponseDTO> Orders { get; set; } = new();
    }
}
