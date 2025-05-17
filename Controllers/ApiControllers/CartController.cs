using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Repositories;
using VNFarm.Interfaces.Services;
using VNFarm.Mappers;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ApiBaseController<Cart, CartRequestDTO, CartResponseDTO>
    {
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;

        public CartController(
            ICartService cartService,
            IJwtTokenService jwtTokenService,
            IProductRepository productRepository,
            IStoreRepository storeRepository,
            ILogger<CartController> logger) : base(cartService, jwtTokenService, logger)
        {
            _cartService = cartService;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng." });

                var cart = await _cartService.GetCartByUserIdAsync(userId.Value);
                return Ok(new { success = true, data = cart });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin giỏ hàng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpPost("preview")]
        public async Task<IActionResult> PreviewCart([FromBody] CartRequestDTO request)
        {
            try
            {
                // Tạo danh sách ShopCart từ request
                var shopCarts = await CreateShopCartsFromRequestAsync(request);
                
                // Tạo Cart
                var cart = new Cart
                {
                    Id = 0, // Temporary ID
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = request.UserId,
                    ShopCarts = shopCarts
                };

                // Chuyển đổi sang CartResponseDTO
                var cartResponseDTO = cart.ToResponseDTO();
                
                // Tính phí vận chuyển: mỗi shop 50000 đồng
                decimal shippingFee = shopCarts.Count * 50000;
                
                // Tính tổng giá trị đơn hàng
                decimal totalPrice = CalculateTotalPrice(cartResponseDTO);
                
                return Ok(new { 
                    success = true, 
                    data = cartResponseDTO,
                    shippingFee = shippingFee,
                    totalPrice = totalPrice,
                    total = totalPrice + shippingFee
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo bản xem trước giỏ hàng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemRequestDTO request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng." });

                var cart = await _cartService.AddItemToCartAsync(userId.Value, request);
                if (cart == null)
                    return BadRequest(new { success = false, message = "Không thể thêm sản phẩm vào giỏ hàng." });

                return Ok(new { success = true, data = cart });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sản phẩm vào giỏ hàng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItemRequestDTO request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng." });

                request.Id = id;
                var result = await _cartService.UpdateCartItemAsync(userId.Value, request);
                if (!result)
                    return BadRequest(new { success = false, message = "Không thể cập nhật sản phẩm trong giỏ hàng." });

                var cart = await _cartService.GetCartByUserIdAsync(userId.Value);
                return Ok(new { success = true, message = "Cập nhật giỏ hàng thành công.", data = cart });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật sản phẩm trong giỏ hàng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng." });

                var result = await _cartService.RemoveCartItemAsync(userId.Value, id);
                if (!result)
                    return BadRequest(new { success = false, message = "Không thể xóa sản phẩm khỏi giỏ hàng." });

                var cart = await _cartService.GetCartByUserIdAsync(userId.Value);
                return Ok(new { success = true, message = "Xóa sản phẩm khỏi giỏ hàng thành công.", data = cart });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa sản phẩm khỏi giỏ hàng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng." });

                var result = await _cartService.ClearCartAsync(userId.Value);
                if (!result)
                    return BadRequest(new { success = false, message = "Không thể xóa giỏ hàng." });

                return Ok(new { success = true, message = "Xóa giỏ hàng thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa giỏ hàng");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDTO request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng." });

                var checkoutResult = await _cartService.CheckoutAsync(userId.Value, request);
                if (checkoutResult == null)
                    return BadRequest(new { success = false, message = "Không thể tạo đơn hàng." });

                return Ok(new { success = true, data = checkoutResult });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo đơn hàng");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        
        // Phương thức tạo danh sách ShopCart từ request
        private async Task<List<ShopCart>> CreateShopCartsFromRequestAsync(CartRequestDTO request)
        {
            // Tạo danh sách để lưu các ShopCart
            var shopCarts = new List<ShopCart>();
            
            // Xử lý từng ShopCart trong request
            if (request.ShopCarts != null)
            {
                foreach (var scRequest in request.ShopCarts)
                {
                    // Lấy thông tin Store
                    var store = await _storeRepository.GetByIdAsync(scRequest.ShopId);
                    
                    // Tạo ShopCart mới
                    var shopCart = new ShopCart
                    {
                        Id = 0, // Temporary ID
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        ShopId = scRequest.ShopId,
                        CartId = 0, // Temporary ID
                        Shop = store, // Gán thông tin Store
                        CartItems = new List<CartItem>()
                    };
                    
                    // Xử lý từng CartItem trong ShopCart
                    if (scRequest.CartItems != null)
                    {
                        foreach (var ciRequest in scRequest.CartItems)
                        {
                            // Lấy thông tin Product
                            var product = await _productRepository.GetByIdAsync(ciRequest.ProductId);
                            
                            // Tạo CartItem mới
                            var cartItem = new CartItem
                            {
                                Id = 0, // Temporary ID
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now,
                                ProductId = ciRequest.ProductId,
                                Quantity = ciRequest.Quantity,
                                ShopCartId = 0, // Temporary ID
                                Product = product // Gán thông tin Product
                            };
                            
                            // Thêm CartItem vào ShopCart
                            shopCart.CartItems.Add(cartItem);
                        }
                    }
                    
                    // Thêm ShopCart vào danh sách
                    shopCarts.Add(shopCart);
                }
            }
            
            return shopCarts;
        }
        
        // Phương thức tính tổng giá trị đơn hàng
        private decimal CalculateTotalPrice(CartResponseDTO cartResponseDTO)
        {
            decimal totalPrice = 0;
            if (cartResponseDTO.ShopCarts != null)
            {
                foreach (var shopCart in cartResponseDTO.ShopCarts)
                {
                    if (shopCart.CartItems != null)
                    {
                        foreach (var item in shopCart.CartItems)
                        {
                            if (item.Product != null)
                            {
                                totalPrice += item.Product.Price * item.Quantity;
                            }
                        }
                    }
                }
            }
            return totalPrice;
        }
    }
}