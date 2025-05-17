using Microsoft.EntityFrameworkCore;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Interfaces.Repositories;

namespace VNFarm.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(VNFarmContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(c => c.ShopCarts)
                    .ThenInclude(sc => sc.CartItems)
                        .ThenInclude(ci => ci.Product)
                .Include(c => c.ShopCarts)
                    .ThenInclude(sc => sc.Shop)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);
        }

        public async Task<Cart?> GetCartWithItemsAsync(int cartId)
        {
            return await _dbSet
                .Include(c => c.ShopCarts)
                    .ThenInclude(sc => sc.CartItems)
                        .ThenInclude(ci => ci.Product)
                .Include(c => c.ShopCarts)
                    .ThenInclude(sc => sc.Shop)
                .FirstOrDefaultAsync(c => c.Id == cartId && !c.IsDeleted);
        }
    }
} 