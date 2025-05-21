using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ApiBaseController<Store, StoreRequestDTO, StoreResponseDTO>
    {
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public StoreController(IStoreService storeService, IProductService productService, IOrderService orderService, IJwtTokenService jwtTokenService, ILogger<StoreController> logger) : base(storeService, jwtTokenService, logger)
        {
            _storeService = storeService;
            _productService = productService;
            _orderService = orderService;
        }

        /// <summary>
        /// Lấy cửa hàng theo người dùng
        /// </summary>
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<StoreResponseDTO>> GetStoreByUserId(int userId)
        {
            var store = await _storeService.GetStoreByUserIdAsync(userId);
            if (store == null)
                return NotFound();

            return Ok(store);
        }

        /// <summary>
        /// Lấy danh sách cửa hàng mới thêm gần đây
        /// </summary>
        [HttpGet("recently-added/{count}")]
        public async Task<ActionResult<IEnumerable<StoreResponseDTO>>> GetRecentlyAddedStores(int count = 5)
        {
            var stores = await _storeService.GetRecentlyAddedStoresAsync(count);
            return Ok(stores);
        }

        /// <summary>
        /// Lấy danh sách cửa hàng theo trạng thái xác minh
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<StoreResponseDTO>>> GetStoresByVerificationStatus(StoreStatus status)
        {
            var stores = await _storeService.GetStoresByVerificationStatusAsync(status);
            return Ok(stores);
        }

        /// <summary>
        /// Lấy danh sách cửa hàng theo bộ lọc
        /// </summary>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<StoreResponseDTO>>> GetStoresByFilter([FromBody] StoreCriteriaFilter filter)
        {
            var stores = await _storeService.Query(filter);
            if(stores.Any())
                stores = stores.Include(m => m.User);
            var totalCount = stores.Count();
            var results = await _storeService.ApplyPagingAndSortingAsync(stores, filter);
            return Ok(new {
                success = true,
                data = results,
                totalCount = totalCount
            });
        }

        /// <summary>
        /// Xác minh cửa hàng
        /// </summary>
        [HttpPut("{id}/verify")]
        public async Task<ActionResult> VerifyStore(int id)
        {
            var success = await _storeService.VerifyStoreAsync(id);
            if (!success)
                return BadRequest(new {
                    success = false,
                    message = "Có lỗi xảy ra khi xác minh cửa hàng"
                });

            return Ok(new {
                success = true,
                message = "Phê duyệt cửa hàng thành công"
            });
        }

        /// <summary>
        /// Từ chối cửa hàng
        /// </summary>
        [HttpPut("{id}/reject")]
        public async Task<ActionResult> RejectStore(int id, [FromQuery] string reason)
        {
            var success = await _storeService.RejectStoreAsync(id, reason);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Cập nhật trạng thái cửa hàng
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<ActionResult> SetStoreStatus(int id, [FromQuery] StoreStatus status)
        {
            var success = await _storeService.SetStoreStatusAsync(id, status);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Cập nhật trạng thái hoạt động của cửa hàng
        /// </summary>
        [HttpPut("{id}/active")]
        public async Task<ActionResult> SetStoreActive(int id, [FromQuery] bool isActive)
        {
            var success = await _storeService.SetStoreActiveAsync(id, isActive);
            if (!success)
                return NotFound();

            return NoContent();
        }
        [HttpGet("{storeId}/get-statistics-store")]
        public async Task<IActionResult> GetStatisticsStore(int storeId)
        {
            // Kiểm tra xem có phải là cửa hàng của người dùng hiện tại không
            var currentUserId = GetCurrentUserId();
            if(currentUserId == null){
                return Unauthorized(new {
                    success = false,
                    message = "Bạn cần đăng nhập để xem thông tin cửa hàng"
                });
            }
            var store = await _storeService.GetByIdAsync(storeId);
            if(store == null || 
                (store.OwnerId != currentUserId && !IsCurrentUserAdmin)){
                return Unauthorized(new {
                    success = false,
                    message = "Bạn không có quyền truy cập vào cửa hàng này"
                });
            }

            var totalProduct = await _productService.CountAsync(m => m.IsDeleted == false && m.StoreId == storeId);
            var totalProductEmptyStock = await _productService.CountAsync(m => m.IsDeleted == false && m.StoreId == storeId && m.StockQuantity == 0);
            var totalOrders = await _orderService.CountAsync(m => m.IsDeleted == false && m.OrderItems.Any(item => item.Product != null && item.Product.StoreId == storeId));
            var totalRenenvue = await _orderService.GetTotalRevenueByStoreIdAsync(storeId);
            var listRenenvueInYear = new List<decimal>();
            var currentYear = DateTime.Now.Year;
            
            for(int month = 1; month <= 12; month++) {
                var startDate = new DateTime(currentYear, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                
                // Sử dụng phương thức tính doanh thu theo khoảng thời gian và lọc theo cửa hàng
                var monthlyRevenue = await _orderService.GetMonthlyRevenueByStoreAsync(storeId, startDate, endDate);
                
                listRenenvueInYear.Add(monthlyRevenue);
            }
            return Ok(new 
            {
                success = true,
                data = new
                {
                    totalProduct,
                    totalProductEmptyStock,
                    totalOrders,
                    totalRenenvue,
                    listRenenvueInYear
                },
                storeId = storeId
            });
        }
    }
}
