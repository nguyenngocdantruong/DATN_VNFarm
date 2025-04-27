using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Interfaces.Repositories
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<Store?> GetStoreByUserIdAsync(int userId);
        Task<IEnumerable<Store>> GetRecentlyAddedStoresAsync(int count);
        Task<IEnumerable<Store>> GetStoresByVerificationStatusAsync(StoreStatus status);
    }
} 