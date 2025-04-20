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
    public class DiscountController : ApiBaseController<Discount, DiscountRequestDTO, DiscountResponseDTO>
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger) : base(discountService, logger)
        {
            _discountService = discountService;
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
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<DiscountResponseDTO>>> GetDiscountsByFilter([FromQuery] DiscountCriteriaFilter filter)
        {
            var discounts = await _discountService.Query(filter);
            var results = await _discountService.ApplyPagingAndSortingAsync(discounts, filter);
            return Ok(results);
        }
    }
}

