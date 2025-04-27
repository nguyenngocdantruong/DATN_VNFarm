using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Interfaces.Repositories
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<IEnumerable<Discount>> GetDiscountsByStoreIdAsync(int storeId);
        Task<IEnumerable<Discount>> GetDiscountsByStatusAsync(DiscountStatus status);
        Task<IEnumerable<Discount>> GetExpiredDiscountsAsync();
        Task<bool> IsDiscountValidAsync(string code, int? userId, int? storeId);
        Task<Discount?> GetByCodeAsync(string code);
        Task<bool> DecrementQuantityAsync(int discountId);
        Task<bool> ToggleStatusAsync(int discountId, DiscountStatus status);
    }
} 