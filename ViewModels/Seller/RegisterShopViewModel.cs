using VNFarm_FinalFinal.DTOs.Response;

namespace VNFarm_FinalFinal.ViewModels.Seller
{
    public class RegisterShopViewModel
    {
        public BusinessRegistrationResponseDTO? BusinessRegistration { get; set; } 
        public List<RegistrationApprovalResultResponseDTO> ApprovalResults { get; set; } = new();
    }
}
