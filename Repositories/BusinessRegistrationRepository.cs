using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Interfaces.Repositories;
using VNFarm.Helpers;

namespace VNFarm.Repositories
{
    public class BusinessRegistrationRepository : BaseRepository<BusinessRegistration>, IBusinessRegistrationRepository
    {
        private readonly DbSet<RegistrationApprovalResult> _approvalResultsSet;
        
        public BusinessRegistrationRepository(VNFarmContext context) : base(context)
        {
            _approvalResultsSet = context.Set<RegistrationApprovalResult>();
        }

        public async Task<BusinessRegistration?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.UserId == userId && !b.IsDeleted);
        }

        public async Task<bool> VerifyRegistrationAsync(int registrationId, RegistrationStatus status, string notes)
        {
            var registration = await _dbSet.FindAsync(registrationId);
            if (registration == null) return false;

            registration.RegistrationStatus = status;
            registration.Notes = notes;
            registration.UpdatedAt = System.DateTime.Now;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RegistrationApprovalResult>> GetRegistrationApprovalResultsAsync(int registrationId)
        {
            var results = await _approvalResultsSet
                .Where(r => r.RegistrationId == registrationId && !r.IsDeleted)
                .ToListAsync();
            return results;
        }

        public async Task<RegistrationApprovalResult?> AddRegistrationApprovalResultAsync(RegistrationApprovalResult result)
        {
            await _approvalResultsSet.AddAsync(result);
            await _context.SaveChangesAsync();
            return result;
        }
    }
} 