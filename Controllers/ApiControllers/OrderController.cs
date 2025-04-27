using Microsoft.AspNetCore.Mvc;
using System;
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
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ApiBaseController<Order, OrderRequestDTO, OrderResponseDTO>
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IStoreService _storeService;
        private readonly IDiscountService _discountService;

        public OrderController(IOrderService orderService,
            IUserService userService,
            IStoreService storeService,
            IDiscountService discountService,
            ILogger<OrderController> logger) : base(orderService, logger)
        {
            _orderService = orderService;
            _userService = userService;
            _storeService = storeService;
            _discountService = discountService;
        }

        /// <summary>
        /// Lấy đơn hàng theo người dùng
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByUser(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        /// <summary>
        /// Lấy đơn hàng theo cửa hàng
        /// </summary>
        [HttpGet("store/{storeId}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByStore(int storeId)
        {
            var orders = await _orderService.GetOrdersByStoreIdAsync(storeId);
            return Ok(orders);
        }

        /// <summary>
        /// Lấy đơn hàng theo trạng thái
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        /// <summary>
        /// Lấy đơn hàng theo khoảng thời gian
        /// </summary>
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(orders);
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        [HttpPut("{id}/status/{status}")]
        public async Task<ActionResult> UpdateOrderStatus(int id, OrderStatus status)
        {
            var success = await _orderService.UpdateOrderStatusAsync(id, status);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Lấy tổng doanh thu của cửa hàng
        /// </summary>
        [HttpGet("revenue/store/{storeId}")]
        public async Task<ActionResult<decimal>> GetTotalRevenueByStore(int storeId)
        {
            var revenue = await _orderService.GetTotalRevenueByStoreIdAsync(storeId);
            return Ok(new { Revenue = revenue });
        }

        /// <summary>
        /// Lấy tổng doanh thu theo khoảng thời gian
        /// </summary>
        [HttpGet("revenue/date-range")]
        public async Task<ActionResult<decimal>> GetTotalRevenueByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var revenue = await _orderService.GetTotalRevenueByDateRangeAsync(startDate, endDate);
            return Ok(new { Revenue = revenue });
        }

        /// <summary>
        /// Lấy đơn hàng theo bộ lọc
        /// </summary>
        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByFilter([FromBody] OrderCriteriaFilter filter)
        {
            var orders = await _orderService.Query(filter);
            orders = orders.Include(m => m.Buyer);
            var totalCount = orders.Count();
            var results = await _orderService.ApplyPagingAndSortingAsync(orders, filter);
            return Ok(new {
                success = true,
                data = results,
                totalCount = totalCount
            });
        }

        /// <summary>
        /// Lấy lịch sử trạng thái đơn hàng
        /// </summary>
        [HttpGet("{id}/timeline")]
        public async Task<ActionResult<IEnumerable<OrderTimeline>>> GetOrderTimeline(int id)
        {
            var timeline = await _orderService.GetOrderTimelineAsync(id);
            return Ok(timeline);
        }

        /// <summary>
        /// Thêm trạng thái mới vào lịch sử đơn hàng
        /// </summary>
        [HttpPost("{id}/timeline")]
        public async Task<ActionResult<OrderTimeline>> AddOrderTimeline(int id, [FromBody] OrderTimelineRequestDTO orderTimelineRequest)
        {
            if (id != orderTimelineRequest.OrderId)
                return BadRequest("OrderId không khớp");

            var timeline = await _orderService.AddOrderTimelineAsync(id, orderTimelineRequest);
            if (timeline == null)
                return BadRequest("Không thể thêm trạng thái đơn hàng");

            return CreatedAtAction(nameof(GetOrderTimeline), new { id = id }, timeline);
        }

        #region Order Information
        /// <summary>
        /// Cập nhật địa chỉ đơn hàng
        /// </summary>
        [HttpPut("{id}/address")]
        public async Task<ActionResult> UpdateOrderAddress(int id, [FromBody] AddressRequestDTO addressRequest)
        {
            var success = await _orderService.UpdateOrderAddressAsync(id, addressRequest);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Cập nhật thông tin vận chuyển
        /// </summary>
        [HttpPut("{id}/shipping")]
        public async Task<ActionResult> UpdateOrderShipping(int id, [FromBody] ShippingRequestDTO shippingRequest)
        {
            var success = await _orderService.UpdateOrderShippingAsync(id, shippingRequest);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Cập nhật trạng thái thanh toán
        /// </summary>
        [HttpPut("{id}/payment-status/{status}")]
        public async Task<ActionResult> UpdateOrderPaymentStatus(int id, PaymentStatus status)
        {
            var success = await _orderService.UpdateOrderPaymentStatusAsync(id, status);
            if (!success)
                return NotFound();

            return NoContent();
        }
        #endregion

        #region Order Calculation
        /// <summary>
        /// Tính thông số đơn hàng
        /// </summary>
        [HttpGet("stats")]
        public async Task<ActionResult> GetOrderStats()
        {
            var totalOrders = await _orderService.CountAsync();
            var totalCanceled = await _orderService.CountAsync(m => m.Status == OrderStatus.Cancelled);
            var totalShipping = await _orderService.CountAsync(m => m.Status == OrderStatus.Shipping);
            var totalRefunded = await _orderService.CountAsync(m => m.Status == OrderStatus.Refunded);
            var totalPending = await _orderService.CountAsync(m => m.Status == OrderStatus.Pending);
            var totalCompleted = await _orderService.CountAsync(m => m.Status == OrderStatus.Completed);            
            var stats = new
            {
                TotalOrders = totalOrders,
                TotalCanceled = totalCanceled,
                TotalShipping = totalShipping,
                TotalRefunded = totalRefunded,
                TotalPending = totalPending,
                TotalCompleted = totalCompleted
            };
            return Ok(new {
                success = true,
                data = stats
            });
        }
        /// <summary>
        /// Tính tổng tiền đơn hàng
        /// </summary>
        [HttpGet("{id}/total-amount")]
        public async Task<ActionResult<decimal>> GetOrderTotalAmount(int id)
        {
            var amount = await _orderService.CalculateOrderTotalAmountAsync(id);
            return Ok(new { TotalAmount = amount });
        }

        /// <summary>
        /// Tính phí vận chuyển
        /// </summary>
        [HttpGet("{id}/shipping-fee")]
        public async Task<ActionResult<decimal>> GetOrderShippingFee(int id)
        {
            var fee = await _orderService.CalculateOrderShippingFeeAsync(id);
            return Ok(new { ShippingFee = fee });
        }

        /// <summary>
        /// Tính thuế
        /// </summary>
        [HttpGet("{id}/tax-amount")]
        public async Task<ActionResult<decimal>> GetOrderTaxAmount(int id)
        {
            var tax = await _orderService.CalculateOrderTaxAmountAsync(id);
            return Ok(new { TaxAmount = tax });
        }

        /// <summary>
        /// Tính số tiền giảm giá
        /// </summary>
        [HttpGet("{id}/discount-amount")]
        public async Task<ActionResult<decimal>> GetOrderDiscountAmount(int id)
        {
            var discount = await _orderService.CalculateOrderDiscountAmountAsync(id);
            return Ok(new { DiscountAmount = discount });
        }

        /// <summary>
        /// Tính tổng tiền cuối cùng
        /// </summary>
        [HttpGet("{id}/final-amount")]
        public async Task<ActionResult<decimal>> GetOrderFinalAmount(int id)
        {
            var amount = await _orderService.CalculateOrderFinalAmountAsync(id);
            return Ok(new { FinalAmount = amount });
        }
        #endregion

        #region Order Review
        /// <summary>
        /// Thêm đánh giá cho sản phẩm trong đơn hàng
        /// </summary>
        [HttpPost("{id}/review/{productId}")]
        public async Task<ActionResult> AddOrderReview(int id, int productId, [FromBody] ReviewRequestDTO reviewRequest)
        {
            var success = await _orderService.AddOrderReviewAsync(id, productId, reviewRequest);
            if (!success)
                return BadRequest("Không thể thêm đánh giá");

            return Ok();
        }
        #endregion

        #region Discount
        /// <summary>
        /// Áp dụng mã giảm giá cho đơn hàng
        /// </summary>
        [HttpPut("{id}/discount/{code}")]
        public async Task<ActionResult> AddDiscountToOrder(int id, string code)
        {
            var success = await _orderService.AddDiscountToOrderAsync(id, code);
            if (!success)
                return BadRequest("Không thể áp dụng mã giảm giá");

            return NoContent();
        }

        /// <summary>
        /// Xóa mã giảm giá khỏi đơn hàng
        /// </summary>
        [HttpDelete("{id}/discount")]
        public async Task<ActionResult> RemoveDiscountFromOrder(int id)
        {
            var success = await _orderService.RemoveDiscountFromOrderAsync(id);
            if (!success)
                return BadRequest("Không thể xóa mã giảm giá");

            return NoContent();
        }
        #endregion

        #region Order Detail
        /// <summary>
        /// Thêm chi tiết đơn hàng
        /// </summary>
        [HttpPost("{id}/details")]
        public async Task<ActionResult<OrderDetailResponseDTO>> AddOrderDetail(int id, [FromBody] OrderDetailRequestDTO orderDetailRequest)
        {
            var detail = await _orderService.AddOrderDetailAsync(id, orderDetailRequest);
            if (detail == null)
                return BadRequest("Không thể thêm chi tiết đơn hàng");

            return CreatedAtAction(nameof(GetOrderDetail), new { id = id }, detail);
        }

        /// <summary>
        /// Lấy chi tiết đơn hàng
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<OrderDetailResponseDTO>>> GetOrderDetail(int id)
        {
            var details = await _orderService.GetOrderDetailAsync(id);
            return Ok(details);
        }

        /// <summary>
        /// Cập nhật trạng thái chi tiết đơn hàng
        /// </summary>
        [HttpPut("{id}/details/{productId}/status/{status}")]
        public async Task<ActionResult> UpdateOrderDetailStatus(int id, int productId, OrderDetailStatus status)
        {
            var success = await _orderService.UpdateOrderDetailStatusAsync(id, productId, status);
            if (!success)
                return NotFound();

            return NoContent();
        }
        #endregion
        protected async override Task<OrderResponseDTO> IncludeNavigation(OrderResponseDTO item)
        {
            item.Buyer = await _userService.GetByIdAsync(item.BuyerId);
            if(item.StoreId != null)
                item.Store = await _storeService.GetByIdAsync(item.StoreId.Value);
            if(item.DiscountId != null)
                item.Discount = await _discountService.GetByIdAsync(item.DiscountId.Value);
            item.OrderDetails = (await _orderService.GetOrderDetailAsync(item.Id)).ToList();
            item.OrderTimelines = (await _orderService.GetOrderTimelineAsync(item.Id)).ToList();
            return await base.IncludeNavigation(item);
        }
    }
}
