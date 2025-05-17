using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Interfaces.Services;
using VNFarm.Interfaces.Repositories;
using System.Linq;
using System;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ApiBaseController<Discount, DiscountRequestDTO, DiscountResponseDTO>
    {
        private readonly IDiscountService _discountService;
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        private readonly IUserService _userService;

        public DiscountController(
            IDiscountService discountService, 
            ILogger<DiscountController> logger,
            IProductService productService,
            IStoreService storeService,
            IUserService userService,
            IJwtTokenService jwtTokenService) : base(discountService, jwtTokenService, logger)
        {
            _discountService = discountService;
            _productService = productService;
            _storeService = storeService;
            _userService = userService;
        }

        /// <summary>
        /// Lấy mã giảm giá theo mã code
        /// </summary>
        [HttpGet("code/{code}")]
        public async Task<ActionResult<DiscountResponseDTO>> GetDiscountByCode(string code)
        {
            var discount = await _discountService.GetByCodeAsync(code);
            if (discount == null)
                return NotFound();

            return Ok(discount);
        }

        /// <summary>
        /// Lấy danh sách mã giảm giá theo cửa hàng
        /// </summary>
        [HttpGet("store/{storeId}")]
        public async Task<ActionResult<IEnumerable<DiscountResponseDTO>>> GetDiscountsByStore(int storeId)
        {
            var discounts = await _discountService.GetDiscountsByStoreIdAsync(storeId);
            return Ok(discounts);
        }

        /// <summary>
        /// Lấy danh sách mã giảm giá theo trạng thái
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<DiscountResponseDTO>>> GetDiscountsByStatus(DiscountStatus status)
        {
            var discounts = await _discountService.GetDiscountsByStatusAsync(status);
            return Ok(discounts);
        }

        /// <summary>
        /// Lấy danh sách mã giảm giá đã hết hạn
        /// </summary>
        [HttpGet("expired")]
        public async Task<ActionResult<IEnumerable<DiscountResponseDTO>>> GetExpiredDiscounts()
        {
            var discounts = await _discountService.GetExpiredDiscountsAsync();
            return Ok(discounts);
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của mã giảm giá
        /// </summary>
        [HttpGet("validate/{code}")]
        public async Task<ActionResult> ValidateDiscount(string code, [FromQuery] int? userId = null, [FromQuery] int? storeId = null)
        {
            var isValid = await _discountService.IsDiscountValidAsync(code, userId, storeId);
            if (!isValid)
                return BadRequest("Mã giảm giá không hợp lệ");

            return Ok(new { valid = true });
        }

        /// <summary>
        /// Giảm số lượng mã giảm giá sau khi sử dụng
        /// </summary>
        [HttpPut("{id}/decrement")]
        public async Task<ActionResult> DecrementDiscountQuantity(int id)
        {
            var success = await _discountService.DecrementQuantityAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Thay đổi trạng thái mã giảm giá
        /// </summary>
        [HttpPut("{id}/status/{status}")]
        public async Task<ActionResult> ToggleDiscountStatus(int id, DiscountStatus status)
        {
            var success = await _discountService.ToggleStatusAsync(id, status);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Lấy danh sách mã giảm giá theo bộ lọc
        /// </summary>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<DiscountResponseDTO>>> GetDiscountsByFilter([FromBody] DiscountCriteriaFilter filter)
        {
            var discounts = await _discountService.Query(filter);
            var results = await _discountService.ApplyPagingAndSortingAsync(discounts, filter);
            return Ok(results);
        }
        
        /// <summary>
        /// Áp dụng mã giảm giá cho giỏ hàng
        /// </summary>
        [HttpPost("voucher")]
        public async Task<ActionResult<VoucherResponseDTO>> ApplyVoucher([FromBody] VoucherRequestDTO request)
        {
            try
            {
                // Lấy thông tin mã giảm giá
                var discount = await _discountService.GetByCodeAsync(request.Voucher);
                
                // Kiểm tra mã giảm giá có tồn tại không
                if (discount == null)
                {
                    return Ok(new VoucherResponseDTO 
                    { 
                        Success = false, 
                        Message = "Mã giảm giá không tồn tại.",
                        Value = 0,
                        Type = DiscountType.Percentage
                    });
                }
                
                // Tạo danh sách để lưu các ShopCart
                var shopCarts = new List<ShopCart>();
                
                // Kiểm tra giỏ hàng có dữ liệu không
                if (request.Cart.ShopCarts == null || !request.Cart.ShopCarts.Any() || 
                    request.Cart.ShopCarts.All(sc => sc.CartItems == null || !sc.CartItems.Any()))
                {
                    return Ok(new VoucherResponseDTO
                    {
                        Success = false,
                        Message = "Giỏ hàng trống. Vui lòng thêm sản phẩm vào giỏ hàng trước khi áp dụng mã giảm giá.",
                        Value = 0,
                        Type = discount.Type
                    });
                }
                
                // Xử lý từng ShopCart trong request
                if (request.Cart.ShopCarts != null)
                {
                    foreach (var scRequest in request.Cart.ShopCarts)
                    {
                        // Lấy thông tin Store
                        var store = await _storeService.GetByIdAsync(scRequest.ShopId);
                        
                        // Tạo ShopCart mới
                        var shopCart = new ShopCart
                        {
                            Id = 0, // Temporary ID
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            ShopId = scRequest.ShopId,
                            CartId = 0, // Temporary ID
                            CartItems = new List<CartItem>()
                        };
                        
                        // Xử lý từng CartItem trong ShopCart
                        if (scRequest.CartItems != null)
                        {
                            foreach (var ciRequest in scRequest.CartItems)
                            {
                                // Lấy thông tin Product
                                var product = await _productService.GetByIdAsync(ciRequest.ProductId);
                                
                                // Log thông tin sản phẩm để debug
                                if (product == null)
                                {
                                    _logger.LogWarning($"Không tìm thấy thông tin sản phẩm với ID: {ciRequest.ProductId}");
                                }
                                else
                                {
                                    _logger.LogInformation($"Sản phẩm ID: {ciRequest.ProductId}, Tên: {product.Name}, Giá: {product.Price}, Số lượng: {ciRequest.Quantity}");
                                }
                                
                                // Tạo CartItem mới
                                var cartItem = new CartItem
                                {
                                    Id = 0, // Temporary ID
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    ProductId = ciRequest.ProductId,
                                    Quantity = ciRequest.Quantity,
                                    ShopCartId = 0, // Temporary ID
                                };
                                
                                // Thêm CartItem vào ShopCart
                                shopCart.CartItems.Add(cartItem);
                            }
                        }
                        
                        // Thêm ShopCart vào danh sách
                        shopCarts.Add(shopCart);
                    }
                }
                
                // Tính tổng giá trị đơn hàng
                decimal totalPrice = 0;
                bool hasInvalidProducts = false;
                
                foreach (var shopCart in shopCarts)
                {
                    foreach (var item in shopCart.CartItems)
                    {
                        if (item.Product != null)
                        {
                            totalPrice += item.Product.Price * item.Quantity;
                        }
                        else
                        {
                            hasInvalidProducts = true;
                        }
                    }
                }
                
                // Kiểm tra nếu có sản phẩm không hợp lệ
                if (hasInvalidProducts)
                {
                    return Ok(new VoucherResponseDTO
                    {
                        Success = false,
                        Message = "Một số sản phẩm trong giỏ hàng không tồn tại hoặc đã bị xóa. Vui lòng cập nhật giỏ hàng.",
                        Value = 0,
                        Type = discount.Type
                    });
                }
                
                // Kiểm tra nếu tổng giá trị đơn hàng bằng 0
                if (totalPrice <= 0)
                {
                    return Ok(new VoucherResponseDTO
                    {
                        Success = false,
                        Message = "Không thể tính toán giá trị đơn hàng. Vui lòng thử lại sau.",
                        Value = 0,
                        Type = discount.Type
                    });
                }
                
                // Log tổng giá trị đơn hàng
                _logger.LogInformation($"Tổng giá trị đơn hàng: {totalPrice}");
                
                // Kiểm tra trạng thái mã giảm giá
                if (!discount.IsActive)
                {
                    return Ok(new VoucherResponseDTO
                    {
                        Success = false,
                        Message = "Mã giảm giá đã hết hạn hoặc không còn hiệu lực.",
                        Value = 0,
                        Type = discount.Type
                    });
                }
                
                // Kiểm tra giá trị đơn hàng tối thiểu
                if (totalPrice < discount.MinimumOrderAmount)
                {
                    return Ok(new VoucherResponseDTO
                    {
                        Success = false,
                        Message = $"Giá trị đơn hàng tối thiểu để sử dụng mã giảm giá là {discount.MinimumOrderAmount:N0} VNĐ. Đơn hàng của bạn có giá trị là {totalPrice:N0} VNĐ.",
                        Value = 0,
                        Type = discount.Type
                    });
                }
                
                // Kiểm tra mã giảm giá có áp dụng cho cửa hàng không
                if (discount.StoreId.HasValue)
                {
                    bool hasValidStore = shopCarts.Any(sc => sc.ShopId == discount.StoreId.Value);
                    if (!hasValidStore)
                    {
                        return Ok(new VoucherResponseDTO
                        {
                            Success = false,
                            Message = "Mã giảm giá chỉ áp dụng cho cửa hàng cụ thể.",
                            Value = 0,
                            Type = discount.Type
                        });
                    }
                }
                
                // Tính giá trị giảm giá
                decimal discountValue = 0;
                if (discount.Type == DiscountType.Percentage)
                {
                    discountValue = totalPrice * discount.DiscountAmount / 100;
                    // Giới hạn giá trị giảm tối đa
                    if (discount.MaximumDiscountAmount > 0 && discountValue > discount.MaximumDiscountAmount)
                    {
                        discountValue = discount.MaximumDiscountAmount;
                    }
                }
                else // DiscountType.FixedAmount
                {
                    discountValue = discount.DiscountAmount;
                }
                
                // Trả về kết quả
                return Ok(new VoucherResponseDTO
                {
                    Success = true,
                    Message = "Mã giảm giá hợp lệ.",
                    Value = discountValue,
                    Type = discount.Type
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi áp dụng mã giảm giá");
                return StatusCode(500, new VoucherResponseDTO
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xử lý yêu cầu.",
                    Value = 0,
                    Type = DiscountType.Percentage
                });
            }
        }
        protected override async Task<DiscountResponseDTO> IncludeNavigation(DiscountResponseDTO item)
        {
            if(item.StoreId.HasValue){
                item.Store = await _storeService.GetByIdAsync(item.StoreId.Value);
            }
            if(item.UserId.HasValue){
                item.User = await _userService.GetByIdAsync(item.UserId.Value);
            }
            return await base.IncludeNavigation(item);
        }
    }
}
