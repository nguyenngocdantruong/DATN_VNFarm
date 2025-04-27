using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Interfaces.Repositories;
using VNFarm.Enums;
using VNFarm.Helpers;

namespace VNFarm.Repositories
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