using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Interfaces.Services;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ApiBaseController<Store, StoreRequestDTO, StoreResponseDTO>
    {
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public StoreController(IStoreService storeService, IProductService productService, IOrderService orderService, ILogger<StoreController> logger) : base(storeService, logger)
        {
            _storeService = storeService;
            _productService = productService;
            _orderService = orderService;
        }

        /// <summary>
        /// Lấy cửa hàng theo người dùng
        /// </summary>
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
                return NotFound();

            return NoContent();
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
        [HttpGet("get-statistics-store/{storeId}")]
        public async Task<IActionResult> GetStatisticsStore(int storeId)
        {
            var totalProduct = await _productService.CountAsync(m => m.IsDeleted == false && m.StoreId == storeId);
            var totalProductEmptyStock = await _productService.CountAsync(m => m.IsDeleted == false && m.StoreId == storeId && m.StockQuantity == 0);
            var totalOrders = await _orderService.CountAsync(m => m.IsDeleted == false && m.StoreId == storeId);
            var totalRenenvue = (await _orderService.FindAsync(m => m.IsDeleted == false && m.StoreId == storeId)).Sum(m => m != null ? m.TotalAmount : 0);
            return Ok(new 
            {
                success = true,
                data = new
                {
                    totalProduct,
                    totalProductEmptyStock,
                    totalOrders,
                    totalRenenvue
                },
                totalCount  = 4,
                storeId = storeId
            });
        }
    }
}
