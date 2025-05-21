using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Repositories.Interfaces
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<Store?> GetStoreByUserIdAsync(int userId);
        Task<IEnumerable<Store>> GetRecentlyAddedStoresAsync(int count);
        Task<IEnumerable<Store>> GetStoresByVerificationStatusAsync(StoreStatus status);
    }
} 