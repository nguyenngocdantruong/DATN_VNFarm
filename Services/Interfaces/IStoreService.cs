using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Services.Interfaces
{
    public interface IStoreService : IService<Store, StoreRequestDTO, StoreResponseDTO>
    {
        Task<StoreResponseDTO?> GetStoreByUserIdAsync(int userId);
        Task<bool> VerifyStoreAsync(int storeId);
        Task<bool> RejectStoreAsync(int storeId, string reason);
        Task<bool> SetStoreStatusAsync(int storeId, StoreStatus status);
        Task<bool> SetStoreActiveAsync(int storeId, bool isActive);
        Task<IEnumerable<StoreResponseDTO?>> GetRecentlyAddedStoresAsync(int count);
        Task<IEnumerable<StoreResponseDTO?>> GetStoresByVerificationStatusAsync(StoreStatus status);
    }
} 