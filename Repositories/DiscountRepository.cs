using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Interfaces.Repositories;

namespace VNFarm.Repositories
{
    public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(VNFarmContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Discount>> GetDiscountsByStoreIdAsync(int storeId)
        {
            return await _dbSet
                .Where(d => d.StoreId == storeId && !d.IsDeleted)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Discount>> GetDiscountsByStatusAsync(DiscountStatus status)
        {
            return await _dbSet
                .Where(d => d.Status == status && !d.IsDeleted)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Discount>> GetExpiredDiscountsAsync()
        {
            var currentDate = DateTime.Now;
            return await _dbSet
                .Where(d => d.EndDate < currentDate && !d.IsDeleted)
                .OrderByDescending(d => d.EndDate)
                .ToListAsync();
        }

        public async Task<bool> IsDiscountValidAsync(string code, int? userId, int? storeId)
        {
            var currentDate = DateTime.Now;
            var discount = await _dbSet
                .FirstOrDefaultAsync(d => 
                    d.Code == code && 
                    !d.IsDeleted &&
                    d.Status == DiscountStatus.Active && 
                    d.StartDate <= currentDate && 
                    d.EndDate >= currentDate && 
                    d.RemainingQuantity > 0 &&
                    (d.StoreId == storeId || d.StoreId == null) &&
                    (d.UserId == userId || d.UserId == null));
                    
            return discount != null;
        }

        public async Task<Discount?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(d => d.Code == code && !d.IsDeleted);
        }

        public async Task<bool> DecrementQuantityAsync(int discountId)
        {
            var discount = await _dbSet.FindAsync(discountId);
            if (discount == null || discount.RemainingQuantity <= 0)
                return false;
                
            discount.RemainingQuantity--;
            discount.UpdatedAt = DateTime.Now;
            
            // Nếu số lượng còn lại = 0, tự động chuyển trạng thái thành không hoạt động
            if (discount.RemainingQuantity == 0)
                discount.Status = DiscountStatus.Inactive;
                
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleStatusAsync(int discountId, DiscountStatus status)
        {
            var discount = await _dbSet.FindAsync(discountId);
            if (discount == null)
                return false;
                
            discount.Status = status;
            discount.UpdatedAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 