using VNFarm.Entities;

namespace VNFarm.Interfaces.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> GetCartWithItemsAsync(int cartId);
    }
} 