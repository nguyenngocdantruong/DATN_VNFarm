using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Interfaces.Services
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