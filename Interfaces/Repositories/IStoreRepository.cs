using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Interfaces.Repositories
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<Store?> GetStoreByUserIdAsync(int userId);
        Task<IEnumerable<Store>> GetRecentlyAddedStoresAsync(int count);
        Task<IEnumerable<Store>> GetStoresByVerificationStatusAsync(StoreStatus status);
    }
} 