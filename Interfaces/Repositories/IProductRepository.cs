using VNFarm.Entities;

namespace VNFarm.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsByStoreAsync(int storeId);
        Task<IEnumerable<Product>> GetTopSellingProductsAsync(int page, int count);
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        Task<Review?> AddReviewAsync(Review review);
        Task UpdateStockAsync(int productId, int quantity);
    }
} 