using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;

namespace VNFarm.Interfaces.Services
{
    public interface ICartService : IService<Cart, CartRequestDTO, CartResponseDTO>
    {
        Task<CartResponseDTO?> GetCartByUserIdAsync(int userId);
        Task<CartResponseDTO?> AddItemToCartAsync(int userId, CartItemRequestDTO cartItemDto);
        Task<bool> UpdateCartItemAsync(int userId, CartItemRequestDTO cartItemDto);
        Task<bool> RemoveCartItemAsync(int userId, int cartItemId);
        Task<bool> ClearCartAsync(int userId);
        Task<CheckoutResponseDTO?> CheckoutAsync(int userId, CheckoutRequestDTO checkoutDto);
    }
} 