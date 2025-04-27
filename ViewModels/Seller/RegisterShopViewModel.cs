using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Seller
{
    public class RegisterShopViewModel
    {
        public BusinessRegistrationResponseDTO? BusinessRegistration { get; set; } 
        public List<RegistrationApprovalResultResponseDTO> ApprovalResults { get; set; } = new();
    }
}
