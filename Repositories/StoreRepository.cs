using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;
using VNFarm.Repositories.Interfaces;

namespace VNFarm.Repositories
{
    public class StoreRepository(VNFarmContext context) : BaseRepository<Store>(context), IStoreRepository
    {
        public async Task<Store?> GetStoreByUserIdAsync(int userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);
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