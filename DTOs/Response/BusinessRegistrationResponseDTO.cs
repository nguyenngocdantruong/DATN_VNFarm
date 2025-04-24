using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;
using System.Text.Json.Serialization;

namespace VNFarm_FinalFinal.DTOs.Response
{
    public class BusinessRegistrationResponseDTO : BaseResponseDTO
    {
        public required int UserId { get; set; }
        public UserResponseDTO? User { get; set; }
        public required string BusinessName { get; set; }
        public required StoreType BusinessType { get; set; }
        public required string TaxCode { get; set; }
        public required string BusinessLicenseUrl { get; set; }
        public required string Address { get; set; }
        public required RegistrationStatus RegistrationStatus { get; set; }
        public required string Notes { get; set; }
        public IEnumerable<RegistrationApprovalResultResponseDTO>? ApprovalResults { get; set; }
    }
}