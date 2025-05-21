using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Services.Interfaces
{
    public interface IDiscountService : IService<Discount, DiscountRequestDTO, DiscountResponseDTO>
    {
        Task<IEnumerable<DiscountResponseDTO?>> GetDiscountsByStoreIdAsync(int storeId);
        Task<IEnumerable<DiscountResponseDTO?>> GetDiscountsByStatusAsync(DiscountStatus status);
        Task<IEnumerable<DiscountResponseDTO?>> GetExpiredDiscountsAsync();
        Task<bool> IsDiscountValidAsync(string code, int? userId, int? storeId);
        Task<DiscountResponseDTO?> GetByCodeAsync(string code);
        Task<bool> DecrementQuantityAsync(int discountId);
        Task<bool> ToggleStatusAsync(int discountId, DiscountStatus status);
    }
} 