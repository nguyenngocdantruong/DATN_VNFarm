using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Interfaces.Services
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