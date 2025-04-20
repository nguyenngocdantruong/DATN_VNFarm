using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm.Infrastructure.Persistence.Context;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Helpers;

namespace VNFarm.Infrastructure.Repositories
{
    public class StoreRepository : BaseRepository<Store>, IStoreRepository
    {
        private readonly DbSet<RegistrationApprovalResult> _approvalResultsSet;
        
        public StoreRepository(VNFarmContext context) : base(context)
        {
            _approvalResultsSet = context.Set<RegistrationApprovalResult>();
        }
        
        public async Task<Store?> GetStoreByUserIdAsync(int userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);
        }
        
        public async Task<IEnumerable<RegistrationApprovalResult>> GetStoreApprovalStatusAsync(int registrationId)
        {
            return await _approvalResultsSet
                .Where(a => a.RegistrationId == registrationId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Store>> GetRecentlyAddedStoresAsync(int count)
        {
            return await _dbSet
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Store>> GetStoresByVerificationStatusAsync(StoreStatus status)
        {
            return await _dbSet
                .Where(s => s.VerificationStatus == status && !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }
    }
} 