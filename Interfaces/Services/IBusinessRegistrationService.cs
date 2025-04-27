using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Interfaces.Services
{
    public interface IBusinessRegistrationService : IService<BusinessRegistration, BusinessRegistrationRequestDTO, BusinessRegistrationResponseDTO>
    {
        Task<BusinessRegistrationResponseDTO?> GetByUserIdAsync(int userId);
        Task<bool> VerifyRegistrationAsync(int registrationId, RegistrationStatus status, string notes);
        Task<IEnumerable<RegistrationApprovalResultResponseDTO>> GetRegistrationApprovalResultsAsync(int registrationId);
        Task<RegistrationApprovalResultResponseDTO?> AddRegistrationApprovalResultAsync(RegistrationApprovalResultRequestDTO registrationApprovalResult, int adminId);
    }
} 