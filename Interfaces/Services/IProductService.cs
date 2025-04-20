using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
namespace VNFarm_FinalFinal.Interfaces.Services
{
    public interface IProductService : IService<Product, ProductRequestDTO, ProductResponseDTO>
    {
        Task<IEnumerable<ProductResponseDTO?>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductResponseDTO?>> GetByStoreAsync(int storeId);
        Task<IEnumerable<ProductResponseDTO?>> GetTopSellingProductsAsync(int page, int count);
        Task UpdateStockAsync(int productId, int quantity);
        Task<IEnumerable<ReviewResponseDTO?>> GetReviewsAsync(int productId);
    }
} 