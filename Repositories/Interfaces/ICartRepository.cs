using VNFarm.Entities;

namespace VNFarm.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> GetCartWithItemsAsync(int cartId);
    }
} 