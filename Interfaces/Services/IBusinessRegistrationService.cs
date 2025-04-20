using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Interfaces.Services
{
    public interface IBusinessRegistrationService : IService<BusinessRegistration, BusinessRegistrationRequestDTO, BusinessRegistrationResponseDTO>
    {
        Task<BusinessRegistrationResponseDTO?> GetByUserIdAsync(int userId);
        Task<bool> VerifyRegistrationAsync(int registrationId, RegistrationStatus status, string notes);
        Task<IEnumerable<RegistrationApprovalResultResponseDTO>> GetRegistrationApprovalResultsAsync(int registrationId);
        Task<RegistrationApprovalResultResponseDTO?> AddRegistrationApprovalResultAsync(RegistrationApprovalResultRequestDTO registrationApprovalResult, int adminId);
    }
} 