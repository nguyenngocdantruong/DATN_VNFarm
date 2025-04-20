using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Services;
using System.Collections.Generic;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ApiBaseController<Store, StoreRequestDTO, StoreResponseDTO>
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService, ILogger<StoreController> logger) : base(storeService, logger)
        {
            _storeService = storeService;
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
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<StoreResponseDTO>>> GetStoresByFilter([FromQuery] StoreCriteriaFilter filter)
        {
            var stores = await _storeService.Query(filter);
            var results = await _storeService.ApplyPagingAndSortingAsync(stores, filter);
            return Ok(results);
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
    }
}
