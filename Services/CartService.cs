using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Mappers;
using VNFarm.Repositories;
using VNFarm.Repositories.Interfaces;
using VNFarm.Services.Interfaces;

namespace VNFarm.Services
{
    public class CartService : BaseService<Cart, CartRequestDTO, CartResponseDTO>, ICartService
    {
        #region Fields & Constructor
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ILogger<CartService> _logger;
        private readonly IDiscountRepository _discountRepository;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IStoreRepository storeRepository,
            ILogger<CartService> logger,
            IDiscountRepository discountRepository) : base(cartRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _logger = logger;
            _discountRepository = discountRepository;
        }
        #endregion

        #region Base Service Implementation
        protected override CartResponseDTO? MapToDTO(Cart? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override Cart? MapToEntity(CartRequestDTO dto)
        {
            return dto.ToEntity();
        }

        public override async Task<bool> UpdateAsync(CartRequestDTO dto)
        {
            var entity = await _cartRepository.GetByIdAsync(dto.Id);
            if (entity == null) return false;
            
            entity.UserId = dto.UserId;
            return await _cartRepository.UpdateAsync(entity);
        }

        public override async Task<IQueryable<Cart>> Query(IFilterCriteria filter)
        {
            var query = await _cartRepository.GetQueryableAsync();
            return query;
        }

        public override async Task<IEnumerable<CartResponseDTO?>> QueryAsync(string query)
        {
            var carts = await _cartRepository.GetAllAsync();
            return carts.Select(MapToDTO);
        }

        public override async Task<IEnumerable<CartResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Cart> query, IFilterCriteria filter)
        {
            var paged = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);
                
            var items = await paged.ToListAsync();
            return items.Select(MapToDTO);
        }
        #endregion

        #region Cart Operations
        public async Task<CartResponseDTO?> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                // Tạo giỏ hàng mới nếu chưa có
                cart = new Cart { UserId = userId };
                await _cartRepository.AddAsync(cart);
            }
            
            return MapToDTO(cart);
        }

        public async Task<CartResponseDTO?> AddItemToCartAsync(int userId, CartItemRequestDTO cartItemDto)
        {
            try
            {
                // Kiểm tra sản phẩm tồn tại
                var product = await _productRepository.GetByIdAsync(cartItemDto.ProductId);
                if (product == null) return null;

                // Lấy giỏ hàng hiện tại
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    // Tạo giỏ hàng mới nếu chưa có
                    cart = new Cart { UserId = userId };
                    await _cartRepository.AddAsync(cart);
                }

                // Lấy thông tin cửa hàng của sản phẩm
                var shopId = product.StoreId;

                // Kiểm tra xem đã có ShopCart cho cửa hàng này chưa
                var shopCart = cart.ShopCarts.FirstOrDefault(sc => sc.ShopId == shopId);
                if (shopCart == null)
                {
                    // Tạo ShopCart mới nếu chưa có
                    shopCart = new ShopCart
                    {
                        CartId = cart.Id,
                        ShopId = shopId
                    };
                    cart.ShopCarts.Add(shopCart);
                }

                // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
                var existingItem = shopCart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemDto.ProductId);
                if (existingItem != null)
                {
                    // Cập nhật số lượng nếu sản phẩm đã tồn tại
                    existingItem.Quantity += cartItemDto.Quantity;
                    await _repository.SaveChangesAsync();
                }
                else
                {
                    // Thêm sản phẩm mới vào giỏ hàng
                    var cartItem = new CartItem
                    {
                        ProductId = cartItemDto.ProductId,
                        Quantity = cartItemDto.Quantity,
                        ShopCartId = shopCart.Id
                    };
                    shopCart.CartItems.Add(cartItem);
                    await _repository.SaveChangesAsync();
                }

                return MapToDTO(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sản phẩm vào giỏ hàng");
                return null;
            }
        }

        public async Task<bool> UpdateCartItemAsync(int userId, CartItemRequestDTO cartItemDto)
        {
            try
            {
                // Lấy giỏ hàng hiện tại
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null) return false;

                // Tìm item cần cập nhật
                var shopCart = cart.ShopCarts.FirstOrDefault(sc => 
                    sc.CartItems.Any(ci => ci.Id == cartItemDto.Id));
                    
                if (shopCart == null) return false;
                
                var cartItem = shopCart.CartItems.FirstOrDefault(ci => ci.Id == cartItemDto.Id);
                if (cartItem == null) return false;

                // Cập nhật số lượng
                cartItem.Quantity = cartItemDto.Quantity;
                
                // Xóa item nếu số lượng = 0
                if (cartItem.Quantity <= 0)
                {
                    shopCart.CartItems.Remove(cartItem);
                    
                    // Xóa shopCart nếu không còn item nào
                    if (!shopCart.CartItems.Any())
                    {
                        cart.ShopCarts.Remove(shopCart);
                    }
                }
                
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật sản phẩm trong giỏ hàng");
                return false;
            }
        }

        public async Task<bool> RemoveCartItemAsync(int userId, int cartItemId)
        {
            try
            {
                // Lấy giỏ hàng hiện tại
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null) return false;

                // Tìm item cần xóa
                var shopCart = cart.ShopCarts.FirstOrDefault(sc => 
                    sc.CartItems.Any(ci => ci.Id == cartItemId));
                    
                if (shopCart == null) return false;
                
                var cartItem = shopCart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
                if (cartItem == null) return false;

                // Xóa item
                shopCart.CartItems.Remove(cartItem);
                
                // Xóa shopCart nếu không còn item nào
                if (!shopCart.CartItems.Any())
                {
                    cart.ShopCarts.Remove(shopCart);
                }
                
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa sản phẩm khỏi giỏ hàng");
                return false;
            }
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            try
            {
                // Lấy giỏ hàng hiện tại
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null) return true; // Không có giỏ hàng cũng coi như đã xóa

                // Xóa tất cả ShopCart
                cart.ShopCarts.Clear();
                
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa giỏ hàng");
                return false;
            }
        }

        public async Task<CheckoutResponseDTO?> CheckoutAsync(int userId, CheckoutRequestDTO checkoutDto)
        {
                // Lấy giỏ hàng hiện tại
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null){
                    throw new Exception("Không tìm thấy giỏ hàng");
                }

                // Lấy danh sách CartItem được chọn
                var selectedCartItems = new List<CartItem>();
                var shopCarts = new Dictionary<int, ShopCart>();

                foreach (var shopCart in cart.ShopCarts)
                {
                    foreach (var cartItem in shopCart.CartItems)
                    {
                            selectedCartItems.Add(cartItem);
                            
                            // Thêm ShopCart vào danh sách nếu chưa có
                            if (!shopCarts.ContainsKey(shopCart.Id))
                            {
                                shopCarts[shopCart.Id] = new ShopCart
                                {
                                    Id = shopCart.Id,
                                    ShopId = shopCart.ShopId,
                                    CartId = shopCart.CartId,
                                    Shop = shopCart.Shop,
                                    CartItems = new List<CartItem>()
                                };
                            }
                            
                            // Thêm CartItem vào ShopCart tương ứng
                            shopCarts[shopCart.Id].CartItems.Add(cartItem);
                    }
                }

                if (!selectedCartItems.Any())
                {
                    throw new Exception("Không có sản phẩm nào được chọn để thanh toán");
                }

                // Tính toán giá tiền
                decimal subTotal = 0;
                decimal shippingFee = 0;
                decimal taxAmount = 0;
                decimal discountAmount = 0;

                // Tính tổng tiền hàng và thuế
                foreach (var item in selectedCartItems)
                {
                    if (item.Product != null)
                    {
                        decimal itemPrice = item.Product.Price * item.Quantity;
                        subTotal += itemPrice;
                        taxAmount += itemPrice * 0.1m; // Thuế 10%
                    }
                }

                // Phí vận chuyển cho mỗi cửa hàng
                shippingFee = shopCarts.Count * 50000; // 50,000 VND cho mỗi cửa hàng

                // Xử lý mã giảm giá nếu có
                
                // Tổng tiền cuối cùng
                decimal finalAmount = subTotal + shippingFee + taxAmount - discountAmount;

                // Tạo đối tượng checkout response
                var checkoutResponse = new CheckoutResponseDTO
                {
                    Id = 0, // Temporary ID
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ShopCarts = shopCarts.Values.Select(sc => sc.ToResponseDTO()).ToList(),
                    ShippingName = checkoutDto.Address.ShippingName,
                    ShippingPhone = checkoutDto.Address.ShippingPhone,
                    ShippingAddress = checkoutDto.Address.ShippingAddress,
                    ShippingProvince = checkoutDto.Address.ShippingProvince,
                    ShippingDistrict = checkoutDto.Address.ShippingDistrict,
                    ShippingWard = checkoutDto.Address.ShippingWard,
                    PaymentMethod = checkoutDto.PaymentMethod,
                    SubTotal = subTotal,
                    ShippingFee = shippingFee,
                    TaxAmount = taxAmount,
                    DiscountAmount = discountAmount,
                    FinalAmount = finalAmount,
                    Notes = checkoutDto.Notes,
                    DiscountCode = checkoutDto.DiscountCode
                };

                return checkoutResponse;
            
        }
        #endregion
    }
} 