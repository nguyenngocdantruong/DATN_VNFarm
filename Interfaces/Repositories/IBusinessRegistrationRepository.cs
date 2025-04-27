using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Interfaces.Repositories
{
    public interface IBusinessRegistrationRepository : IRepository<BusinessRegistration>
    {
        Task<BusinessRegistration?> GetByUserIdAsync(int userId);
        Task<bool> VerifyRegistrationAsync(int registrationId, RegistrationStatus status, string notes);
        Task<IEnumerable<RegistrationApprovalResult>> GetRegistrationApprovalResultsAsync(int registrationId);
        Task<RegistrationApprovalResult?> AddRegistrationApprovalResultAsync(RegistrationApprovalResult registrationApprovalResult);
    }
} 