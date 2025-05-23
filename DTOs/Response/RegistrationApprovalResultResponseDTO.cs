using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class RegistrationApprovalResultResponseDTO: BaseResponseDTO
    {
        public required int RegistrationId { get; set; }
        public required int AdminId { get; set; }
        public required ApprovalResult ApprovalResult { get; set; }
        public required string Note { get; set; }
        public UserResponseDTO? Admin { get; set; }
    }
}
