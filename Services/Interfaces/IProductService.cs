using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;

namespace VNFarm.Services.Interfaces
{
    public interface IProductService : IService<Product, ProductRequestDTO, ProductResponseDTO>
    {
        Task<IEnumerable<ProductResponseDTO?>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductResponseDTO?>> GetByStoreAsync(int storeId);
        Task<IEnumerable<ProductResponseDTO?>> GetTopSellingProductsAsync(int page, int count);
        Task UpdateStockAsync(int productId, int quantity);
        Task<IEnumerable<ReviewResponseDTO?>> GetReviewsAsync(int productId);
        Task<bool> AddReviewAsync(ReviewRequestDTO reviewRequestDTO);
        Task<bool> UpdateSoldQuantityAsync(int productId, int soldQuantity);
    }
} 