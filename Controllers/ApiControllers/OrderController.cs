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
using Microsoft.Extensions.DependencyInjection;
using VNFarm.Interfaces.External;
using VNFarm.Helpers;

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
        private readonly IEmailService _emailService;
        public OrderController(IOrderService orderService,
            IUserService userService,
            IStoreService storeService,
            IDiscountService discountService,
            IJwtTokenService jwtTokenService,
            IEmailService emailService,
            ILogger<OrderController> logger) : base(orderService, jwtTokenService, logger)
        {
            _orderService = orderService;
            _userService = userService;
            _storeService = storeService;
            _discountService = discountService;
            _emailService = emailService;
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
            if (filter.UserId.HasValue)
            {
                var currentUserId = GetCurrentUserId();
                var isValid = User.IsInRole("Admin") || (currentUserId.HasValue && currentUserId.Value == filter.UserId.Value);
                if (!isValid)
                {
                    return Unauthorized(new { success = false, message = "Bạn không có quyền xem đơn hàng của người dùng khác" });
                }
            }
            var orders = await _orderService.Query(filter);
            var totalCount = orders.Count();
            var results = await _orderService.ApplyPagingAndSortingAsync(orders, filter);
            results = await Task.WhenAll(results.Select(async m => await IncludeNavigation(m)));
            return Ok(new
            {
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
            timeline = timeline.OrderByDescending(m => m.Id).ToList();
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
            var order = await _orderService.GetByIdAsync(id);
            // Kiểm tra nếu đơn hàng đã hoàn thành thì không thể thêm trạng thái mới
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

            if (orderTimelineRequest.EventType == OrderEventType.OrderCreated)
            {
                order = await _orderService.GetByIdAsync(id);
                if (order == null)
                    return BadRequest("Không tìm thấy đơn hàng");
            }

            if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Cancelled)
            {
                return BadRequest(new { success = false, message = "Không thể thêm trạng thái mới cho đơn hàng đã hoàn thành hoặc đã hủy" });
            }
            // Kiểm tra xem trạng thái có hợp lệ không
            if (!Enum.IsDefined(typeof(OrderTimelineStatus), orderTimelineRequest.Status))
            {
                return BadRequest(new { success = false, message = "Trạng thái không hợp lệ" });
            }
            var user = await _userService.GetByIdAsync(order.BuyerId);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                // Gửi email thông báo cập nhật trạng thái đơn hàng nếu có thay đổi trạng thái
                try
                {
                    var orderContents = OrderUtils.GetOrderEventTypeForForm();
                    var content = orderContents.TryGetValue((int)orderTimelineRequest.EventType, out var value) ? value.ToString() : "Không thể xác định trạng thái";
                    await _emailService.SendOrderStatusUpdateAsync(user.Email, id, content ?? "Không thể xác định trạng thái");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gửi email thông báo cập nhật trạng thái đơn hàng {OrderId}", id);
                }
            }

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
            return Ok(new
            {
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
        [HttpPost("{id}/items")]
        public async Task<ActionResult<OrderItemResponseDTO>> AddOrderItem(int id, [FromBody] OrderItemRequestDTO orderItemRequest)
        {
            var item = await _orderService.AddOrderItemAsync(id, orderItemRequest);
            if (item == null)
                return BadRequest(new { success = false, message = "Không thể thêm chi tiết đơn hàng." });

            return CreatedAtAction(nameof(GetOrderItems), new { id = id }, item);
        }

        /// <summary>
        /// Lấy chi tiết đơn hàng
        /// </summary>
        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<OrderItemResponseDTO>>> GetOrderItems(int id)
        {
            var items = await _orderService.GetOrderItemsAsync(id);
            return Ok(new { success = true, data = items });
        }

        /// <summary>
        /// Cập nhật trạng thái chi tiết đơn hàng
        /// </summary>
        [HttpPut("{id}/items/{productId}/status/{status}")]
        public async Task<ActionResult> UpdateOrderItemStatus(int id, int productId, OrderItemStatus status)
        {
            var success = await _orderService.UpdateOrderItemStatusAsync(id, productId, status);
            if (!success)
                return NotFound(new { success = false, message = "Không tìm thấy chi tiết đơn hàng." });

            return Ok(new { success = true, message = "Cập nhật trạng thái thành công." });
        }
        #endregion

        #region Create Order
        /// <summary>
        /// Tạo đơn hàng mới từ thông tin checkout
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult> CreateOrder([FromBody] CheckoutRequestDTO checkoutRequest)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng." });

                var order = await _orderService.CreateOrderFromCheckoutAsync(userId.Value, checkoutRequest);
                if (order == null)
                    return BadRequest(new { success = false, message = "Không thể tạo đơn hàng." });

                // Thêm thông tin navigation
                order = await IncludeNavigation(order);

                return Ok(new
                {
                    success = true,
                    message = "Đặt hàng thành công!",
                    data = order,
                    orderCode = order.OrderCode,
                    orderId = order.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo đơn hàng");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        #endregion

        /// <summary>
        /// Cập nhật trạng thái đơn hàng và thanh toán (Admin)
        /// </summary>
        [HttpPut("{id}/admin-update")]
        public async Task<ActionResult> AdminUpdateOrderStatus(int id, [FromBody] OrderAdminUpdateDTO updateRequest)
        {
            try
            {
                // Kiểm tra quyền admin
                if (!User.IsInRole("Admin"))
                {
                    return Unauthorized(new { success = false, message = "Bạn không có quyền thực hiện thao tác này" });
                }

                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                    return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

                // Lưu trạng thái cũ để so sánh
                var oldStatus = order.Status;
                var oldPaymentStatus = order.PaymentStatus;

                // Cập nhật trạng thái thanh toán
                if (updateRequest.PaymentStatus.HasValue && updateRequest.PaymentStatus.Value != oldPaymentStatus)
                {
                    await _orderService.UpdateOrderPaymentStatusAsync(id, updateRequest.PaymentStatus.Value);
                    
                    // Thêm timeline cho trạng thái thanh toán
                    var paymentTimelineRequest = new OrderTimelineRequestDTO
                    {
                        OrderId = id,
                        EventType = OrderEventType.OrderPaymentReceived,
                        Status = OrderTimelineStatus.Completed,
                        Description = $"Trạng thái thanh toán được cập nhật từ [{GetPaymentStatusName(oldPaymentStatus)}] thành [{GetPaymentStatusName(updateRequest.PaymentStatus.Value)}] bởi Admin"
                    };
                    
                    await _orderService.AddOrderTimelineAsync(id, paymentTimelineRequest);
                    
                    // Gửi email thông báo cho khách hàng
                    var buyer = await _userService.GetByIdAsync(order.BuyerId);
                    if (buyer != null && !string.IsNullOrEmpty(buyer.Email))
                    {
                        await _emailService.SendOrderStatusUpdateAsync(buyer.Email, id, $"Trạng thái thanh toán đơn hàng của bạn đã được cập nhật thành: {GetPaymentStatusName(updateRequest.PaymentStatus.Value)}");
                    }
                }

                // Cập nhật trạng thái đơn hàng
                if (updateRequest.Status.HasValue && updateRequest.Status.Value != oldStatus)
                {
                    await _orderService.UpdateOrderStatusAsync(id, updateRequest.Status.Value);
                    
                    // Thêm timeline cho trạng thái đơn hàng
                    var orderTimelineRequest = new OrderTimelineRequestDTO
                    {
                        OrderId = id,
                        EventType = GetOrderEventTypeFromStatus(updateRequest.Status.Value),
                        Status = OrderTimelineStatus.Completed,
                        Description = $"Trạng thái đơn hàng được cập nhật từ [{GetStatusName(oldStatus)}] thành [{GetStatusName(updateRequest.Status.Value)}] bởi Admin"
                    };
                    
                    await _orderService.AddOrderTimelineAsync(id, orderTimelineRequest);
                    
                    // Gửi email thông báo cho khách hàng
                    var buyer = await _userService.GetByIdAsync(order.BuyerId);
                    if (buyer != null && !string.IsNullOrEmpty(buyer.Email))
                    {
                        await _emailService.SendOrderStatusUpdateAsync(buyer.Email, id, $"Đơn hàng của bạn đã được cập nhật sang trạng thái: {GetStatusName(updateRequest.Status.Value)}");
                    }
                    
                    // Gửi email thông báo cho các cửa hàng liên quan
                    var orderItems = await _orderService.GetOrderItemsAsync(id);
                    var storeIds = orderItems.Select(item => item.ShopId).Distinct().ToList();
                    
                    foreach (var storeId in storeIds)
                    {
                        var store = await _storeService.GetByIdAsync(storeId);
                        if (store != null && !string.IsNullOrEmpty(store.Email))
                        {
                            await _emailService.SendOrderStatusUpdateAsync(store.Email, id, $"Đơn hàng #{order.OrderCode} đã được cập nhật sang trạng thái: {GetStatusName(updateRequest.Status.Value)}");
                        }
                    }
                }

                // Lấy đơn hàng đã cập nhật với đầy đủ thông tin
                var updatedOrder = await _orderService.GetByIdAsync(id);
                updatedOrder = await IncludeNavigation(updatedOrder);

                return Ok(new { success = true, message = "Cập nhật thành công", data = updatedOrder });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái đơn hàng {OrderId}", id);
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi cập nhật đơn hàng" });
            }
        }

        /// <summary>
        /// Cập nhật thông tin vận chuyển và địa chỉ đơn hàng (Admin)
        /// </summary>
        [HttpPut("{id}/admin-shipping-update")]
        public async Task<ActionResult> AdminUpdateOrderShipping(int id, [FromBody] OrderAdminShippingUpdateDTO updateRequest)
        {
            try
            {
                // Kiểm tra quyền admin
                if (!User.IsInRole("Admin"))
                {
                    return Unauthorized(new { success = false, message = "Bạn không có quyền thực hiện thao tác này" });
                }

                _logger.LogInformation("Cập nhật thông tin vận chuyển đơn hàng {OrderId} - {OrderCode}", id, updateRequest.Shipping.OrderId);

                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                    return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });

                // Cập nhật thông tin vận chuyển
                if (updateRequest.Shipping != null)
                {
                    var shippingRequest = new ShippingRequestDTO
                    {
                        OrderId = updateRequest.Shipping.OrderId,
                        TrackingNumber = updateRequest.Shipping.TrackingNumber ?? "",
                        ShippingMethod = updateRequest.Shipping.ShippingMethod ?? "",
                        ShippingPartner = updateRequest.Shipping.ShippingPartner ?? "",
                        ShippedAt = updateRequest.Shipping.ShippedAt,
                        DeliveredAt = updateRequest.Shipping.DeliveredAt
                    };
                    
                    await _orderService.UpdateOrderShippingAsync(id, shippingRequest);
                    
                    // Thêm timeline cho cập nhật thông tin vận chuyển
                    var shippingTimelineRequest = new OrderTimelineRequestDTO
                    {
                        OrderId = id,
                        EventType = OrderEventType.OrderShippingUpdated,
                        Status = OrderTimelineStatus.Completed,
                        Description = "Thông tin vận chuyển đã được cập nhật bởi Admin"
                    };
                    
                    await _orderService.AddOrderTimelineAsync(id, shippingTimelineRequest);
                    
                    // Gửi email thông báo cho khách hàng
                    var buyer = await _userService.GetByIdAsync(order.BuyerId);
                    if (buyer != null && !string.IsNullOrEmpty(buyer.Email))
                    {
                        await _emailService.SendOrderStatusUpdateAsync(buyer.Email, id, "Thông tin vận chuyển đơn hàng của bạn đã được cập nhật");
                    }
                }

                // Cập nhật thông tin địa chỉ
                if (updateRequest.Address != null)
                {
                    var addressRequest = new AddressRequestDTO
                    {
                        ShippingName = updateRequest.Address.ShippingName,
                        ShippingPhone = updateRequest.Address.ShippingPhone,
                        ShippingAddress = updateRequest.Address.ShippingAddress,
                        ShippingProvince = updateRequest.Address.ShippingProvince,
                        ShippingDistrict = updateRequest.Address.ShippingDistrict,
                        ShippingWard = updateRequest.Address.ShippingWard
                    };
                    
                    await _orderService.UpdateOrderAddressAsync(id, addressRequest);
                    
                    // Thêm timeline cho cập nhật địa chỉ
                    var addressTimelineRequest = new OrderTimelineRequestDTO
                    {
                        OrderId = id,
                        EventType = OrderEventType.OrderAddressUpdated,
                        Status = OrderTimelineStatus.Completed,
                        Description = "Địa chỉ giao hàng đã được cập nhật bởi Admin"
                    };
                    
                    await _orderService.AddOrderTimelineAsync(id, addressTimelineRequest);
                }

                // Lấy đơn hàng đã cập nhật với đầy đủ thông tin
                var updatedOrder = await _orderService.GetByIdAsync(id);
                updatedOrder = await IncludeNavigation(updatedOrder);

                return Ok(new { success = true, message = "Cập nhật thông tin vận chuyển thành công", data = updatedOrder });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin vận chuyển đơn hàng {OrderId}", id);
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi cập nhật thông tin vận chuyển" });
            }
        }

        // Phương thức hỗ trợ chuyển đổi OrderStatus sang OrderEventType
        private OrderEventType GetOrderEventTypeFromStatus(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => OrderEventType.OrderCreated,
                OrderStatus.Processing => OrderEventType.OrderAcceptedBySeller,
                OrderStatus.Shipping => OrderEventType.OrderShipped,
                OrderStatus.Delivered => OrderEventType.OrderCompleted,
                OrderStatus.Completed => OrderEventType.OrderCompleted,
                OrderStatus.Cancelled => OrderEventType.OrderCancelled,
                OrderStatus.Refunded => OrderEventType.OrderRefunded,
                _ => OrderEventType.OrderReadyToShip,
            };
        }

        // Phương thức lấy tên trạng thái đơn hàng
        private string GetStatusName(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Chờ xác nhận",
                OrderStatus.Processing => "Đang xử lý",
                OrderStatus.Packaged => "Đã đóng gói",
                OrderStatus.Shipping => "Đang vận chuyển",
                OrderStatus.Delivered => "Đã giao hàng",
                OrderStatus.Completed => "Đã hoàn thành",
                OrderStatus.Cancelled => "Đã hủy",
                OrderStatus.Refunded => "Đã hoàn tiền",
                _ => "Không xác định",
            };
        }

        // Phương thức lấy tên trạng thái thanh toán
        private string GetPaymentStatusName(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Unpaid => "Chưa thanh toán",
                PaymentStatus.PartiallyPaid => "Thanh toán một phần",
                PaymentStatus.Paid => "Đã thanh toán",
                PaymentStatus.Refunded => "Đã hoàn tiền",
                PaymentStatus.Failed => "Thanh toán thất bại",
                _ => "Không xác định",
            };
        }

        protected async override Task<OrderResponseDTO> IncludeNavigation(OrderResponseDTO item)
        {
            // Thông tin người mua
            item.Buyer = await _userService.GetByIdAsync(item.BuyerId);

            // Thông tin giảm giá
            if (item.DiscountId != null)
                item.Discount = await _discountService.GetByIdAsync(item.DiscountId.Value);

            // Thông tin chi tiết đơn hàng
            item.OrderItems = (await _orderService.GetOrderItemsAsync(item.Id)).ToList();

            // Thông tin lịch sử đơn hàng
            item.OrderTimelines = (await _orderService.GetOrderTimelineAsync(item.Id)).OrderByDescending(m => m.Id).ToList();

            return await base.IncludeNavigation(item);
        }

        /// <summary>
        /// Lấy đơn hàng theo ID với đầy đủ thông tin liên quan
        /// </summary>
        [HttpGet("{orderCode}/full")]
        public async Task<ActionResult<OrderResponseDTO>> Get(string orderCode)
        {
            var order = (await _orderService.FindAsync(m => m.OrderCode == orderCode)).FirstOrDefault();
            if (order == null)
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng." });
            // Thêm thông tin navigation
            order = await IncludeNavigation(order);

            return Ok(new { success = true, data = order });
        }

        /// <summary>
        /// Lấy đơn hàng cho seller với thông tin phù hợp
        /// </summary>
        [HttpPost("seller")]
        public async Task<ActionResult<OrderForSellerResponseDTO>> GetOrdersForSeller([FromBody] OrderCriteriaFilter filter)
        {
            try
            {
                // Kiểm tra người dùng đã đăng nhập và có quyền seller
                if (!IsCurrentUserSeller)
                {
                    return Unauthorized(new { success = false, message = "Bạn không có quyền truy cập thông tin này" });
                }

                // Lấy ID của người đang đăng nhập (seller)
                var sellerId = GetCurrentUserId();
                if (sellerId == null)
                {
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng" });
                }

                // Lấy thông tin store của seller
                var store = await _storeService.GetStoreByUserIdAsync(sellerId.Value);
                if (store == null)
                {
                    return BadRequest(new { success = false, message = "Không tìm thấy thông tin cửa hàng của bạn" });
                }

                // Thêm storeId vào filter để chỉ lấy đơn hàng thuộc về store này
                filter.StoreId = store.Id;

                // Thực hiện query lấy đơn hàng
                var orders = await _orderService.Query(filter);
                var totalCount = orders.Count();
                
                // Áp dụng phân trang và sắp xếp
                var orderResponses = await _orderService.ApplyPagingAndSortingAsync(orders, filter);
                
                // Chuyển đổi OrderResponseDTO sang OrderForSellerResponseDTO
                var sellerOrderResponses = new List<OrderForSellerResponseDTO>();
                
                foreach (var order in orderResponses)
                {
                    // Lấy thông tin chi tiết của đơn hàng
                    var orderItems = await _orderService.GetOrderItemsAsync(order.Id);
                    var orderTimelines = await _orderService.GetOrderTimelineAsync(order.Id);
                    
                    // Lọc các orderItems thuộc về store của seller
                    var storeOrderItems = orderItems.Where(item => item.ShopId == store.Id).ToList();
                    
                    // Chỉ trả về thông tin nếu đơn hàng có sản phẩm thuộc store này
                    if (storeOrderItems.Any())
                    {
                        var sellerOrder = new OrderForSellerResponseDTO
                        {
                            Id = order.Id,
                            OrderCode = order.OrderCode,
                            Status = order.Status,
                            Notes = order.Notes ?? "",
                            Address = new AddressResponseDTO
                            {
                                OrderId = order.Id,
                                ShippingName = order.Address.ShippingName,
                                ShippingPhone = order.Address.ShippingPhone,
                                ShippingAddress = order.Address.ShippingAddress,
                                ShippingProvince = order.Address.ShippingProvince,
                                ShippingDistrict = order.Address.ShippingDistrict,
                                ShippingWard = order.Address.ShippingWard
                            },
                            Shipping = new ShippingResponseDTO
                            {
                                OrderId = order.Id,
                                TrackingNumber = order.Shipping.TrackingNumber ?? "",
                                ShippingMethod = order.Shipping.ShippingMethod ?? "",
                                ShippingPartner = order.Shipping.ShippingPartner ?? "",
                                ShippedAt = order.Shipping.ShippedAt,
                                DeliveredAt = order.Shipping.DeliveredAt
                            },
                            PaymentStatus = order.PaymentStatus,
                            BuyerId = order.BuyerId,
                            StoreId = store.Id,
                            OrderItems = storeOrderItems,
                            OrderTimelines = orderTimelines.OrderByDescending(m => m.Id).ToList(),
                            CreatedAt = order.CreatedAt,
                            UpdatedAt = order.UpdatedAt
                        };
                        
                        sellerOrderResponses.Add(sellerOrder);
                    }
                }

                return Ok(new
                {
                    success = true,
                    data = sellerOrderResponses,
                    totalCount = totalCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy đơn hàng cho seller");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết đơn hàng cho seller theo ID
        /// </summary>
        [HttpGet("seller/{id}")]
        public async Task<ActionResult<OrderForSellerResponseDTO>> GetOrderForSellerById(int id)
        {
            try
            {
                // Kiểm tra người dùng đã đăng nhập và có quyền seller
                if (!IsCurrentUserSeller)
                {
                    return Unauthorized(new { success = false, message = "Bạn không có quyền truy cập thông tin này" });
                }

                // Lấy ID của người đang đăng nhập (seller)
                var sellerId = GetCurrentUserId();
                if (sellerId == null)
                {
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng" });
                }

                // Lấy thông tin store của seller
                var store = await _storeService.GetStoreByUserIdAsync(sellerId.Value);
                if (store == null)
                {
                    return BadRequest(new { success = false, message = "Không tìm thấy thông tin cửa hàng của bạn" });
                }

                // Lấy thông tin đơn hàng
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                // Lấy thông tin chi tiết của đơn hàng
                var orderItems = await _orderService.GetOrderItemsAsync(id);
                var orderTimelines = await _orderService.GetOrderTimelineAsync(id);
                
                // Lọc các orderItems thuộc về store của seller
                var storeOrderItems = orderItems.Where(item => item.ShopId == store.Id).ToList();
                
                // Kiểm tra xem đơn hàng có sản phẩm thuộc store này không
                if (!storeOrderItems.Any())
                {
                    return NotFound(new { success = false, message = "Đơn hàng này không thuộc về cửa hàng của bạn" });
                }

                // Tạo response
                var sellerOrder = new OrderForSellerResponseDTO
                {
                    Id = order.Id,
                    OrderCode = order.OrderCode,
                    Status = order.Status,
                    Notes = order.Notes ?? "",
                    Address = new AddressResponseDTO
                    {
                        OrderId = order.Id,
                        ShippingName = order.Address.ShippingName,
                        ShippingPhone = order.Address.ShippingPhone,
                        ShippingAddress = order.Address.ShippingAddress,
                        ShippingProvince = order.Address.ShippingProvince,
                        ShippingDistrict = order.Address.ShippingDistrict,
                        ShippingWard = order.Address.ShippingWard
                    },
                    Shipping = new ShippingResponseDTO
                    {
                        OrderId = order.Id,
                        TrackingNumber = order.Shipping.TrackingNumber ?? "",
                        ShippingMethod = order.Shipping.ShippingMethod ?? "",
                        ShippingPartner = order.Shipping.ShippingPartner ?? "",
                        ShippedAt = order.Shipping.ShippedAt,
                        DeliveredAt = order.Shipping.DeliveredAt
                    },
                    PaymentStatus = order.PaymentStatus,
                    BuyerId = order.BuyerId,
                    StoreId = store.Id,
                    OrderItems = storeOrderItems,
                    OrderTimelines = orderTimelines.OrderByDescending(m => m.Id).ToList(),
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt
                };

                return Ok(new { success = true, data = sellerOrder });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin chi tiết đơn hàng cho seller");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái chi tiết đơn hàng cho seller
        /// </summary>
        [HttpPut("seller/item/{orderId}/{orderItemId}/status")]
        public async Task<ActionResult> UpdateOrderItemStatusForSeller(int orderId, int orderItemId, [FromBody] OrderItemStatusUpdateDTO updateRequest)
        {
            try
            {
                // Kiểm tra người dùng đã đăng nhập và có quyền seller
                if (!IsCurrentUserSeller)
                {
                    return Unauthorized(new { success = false, message = "Bạn không có quyền truy cập thông tin này" });
                }

                // Lấy ID của người đang đăng nhập (seller)
                var sellerId = GetCurrentUserId();
                if (sellerId == null)
                {
                    return Unauthorized(new { success = false, message = "Không thể xác thực người dùng" });
                }

                // Lấy thông tin store của seller
                var store = await _storeService.GetStoreByUserIdAsync(sellerId.Value);
                if (store == null)
                {
                    return BadRequest(new { success = false, message = "Không tìm thấy thông tin cửa hàng của bạn" });
                }

                // Lấy thông tin đơn hàng
                var order = await _orderService.GetByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });
                }

                // Lấy thông tin chi tiết của đơn hàng
                var orderItems = await _orderService.GetOrderItemsAsync(orderId);
                
                // Lọc các orderItems thuộc về store của seller
                var storeOrderItems = orderItems.Where(item => item.ShopId == store.Id).ToList();
                
                // Kiểm tra xem đơn hàng có sản phẩm thuộc store này không
                if (!storeOrderItems.Any())
                {
                    return NotFound(new { success = false, message = "Đơn hàng này không thuộc về cửa hàng của bạn" });
                }

                // Tìm OrderItem cụ thể cần cập nhật
                var orderItem = storeOrderItems.FirstOrDefault(item => item.Id == orderItemId);
                if (orderItem == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy sản phẩm này trong đơn hàng của bạn" });
                }

                // Kiểm tra trạng thái đơn hàng, chỉ cho phép cập nhật nếu đơn hàng chưa hoàn thành hoặc hủy
                if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Cancelled)
                {
                    return BadRequest(new { success = false, message = "Không thể cập nhật sản phẩm trong đơn hàng đã hoàn thành hoặc đã hủy" });
                }

                // Cập nhật trạng thái OrderItem
                var success = await _orderService.UpdateOrderItemStatusAsync(orderId, orderItem.ProductId, updateRequest.Status);
                if (!success)
                {
                    return BadRequest(new { success = false, message = "Không thể cập nhật trạng thái sản phẩm" });
                }

                // Nếu tất cả sản phẩm trong đơn hàng đều đã đóng gói xong (Packed), cập nhật toàn bộ đơn hàng sang trạng thái Packaged
                if (updateRequest.Status == OrderItemStatus.Packed)
                {
                    // Lấy lại thông tin cập nhật mới nhất sau khi cập nhật 1 item
                    var updatedItems = await _orderService.GetOrderItemsByOrderIdAndStoreIdAsync(orderId, store.Id);
                    
                    // Kiểm tra xem tất cả sản phẩm của cửa hàng này đã packed chưa
                    bool allItemsPacked = updatedItems.All(item => item.PackagingStatus == OrderItemStatus.Packed);
                    
                    if (allItemsPacked)
                    {
                        // Tự động cập nhật trạng thái đơn hàng nếu tất cả sản phẩm đã được đóng gói
                        if (order.Status < OrderStatus.Packaged)
                        {
                            await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Packaged);
                            
                            // Thêm timeline cho cập nhật trạng thái đơn hàng
                            var packagedTimelineRequest = new OrderTimelineRequestDTO
                            {
                                OrderId = orderId,
                                EventType = OrderEventType.OrderReadyToShip,
                                Status = OrderTimelineStatus.Completed,
                                Description = "Tất cả sản phẩm đã được đóng gói và sẵn sàng giao hàng"
                            };
                            
                            await _orderService.AddOrderTimelineAsync(orderId, packagedTimelineRequest);
                        }
                    }
                }

                return Ok(new { success = true, message = "Cập nhật trạng thái sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật trạng thái sản phẩm {OrderId}, {OrderItemId}", orderId, orderItemId);
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        // Class DTO cho cập nhật trạng thái OrderItem
        public class OrderItemStatusUpdateDTO
        {
            public required OrderItemStatus Status { get; set; }
        }

        // Phương thức hỗ trợ chuyển đổi OrderItemStatus sang OrderEventType
        private OrderEventType GetOrderEventTypeFromOrderItemStatus(OrderItemStatus status)
        {
            return status switch
            {
                OrderItemStatus.Processing => OrderEventType.OrderAcceptedBySeller,
                OrderItemStatus.Packed => OrderEventType.OrderReadyToShip,
                OrderItemStatus.Shipped => OrderEventType.OrderShipped,
                OrderItemStatus.Delivered => OrderEventType.OrderCompleted,
                OrderItemStatus.Cancelled => OrderEventType.OrderCancelled,
                OrderItemStatus.Returned => OrderEventType.OrderRefunded,
                _ => OrderEventType.OrderReadyToShip,
            };
        }

        // Phương thức lấy tên trạng thái OrderItem
        private string GetOrderItemStatusName(OrderItemStatus status)
        {
            return status switch
            {
                OrderItemStatus.Pending => "Chờ xử lý",
                OrderItemStatus.Processing => "Đang xử lý",
                OrderItemStatus.Packed => "Đã đóng gói",
                OrderItemStatus.Shipped => "Đã giao cho đơn vị vận chuyển",
                OrderItemStatus.Delivered => "Đã giao hàng",
                OrderItemStatus.Cancelled => "Đã hủy",
                OrderItemStatus.Returned => "Đã trả hàng",
                _ => "Không xác định",
            };
        }
    }
}
