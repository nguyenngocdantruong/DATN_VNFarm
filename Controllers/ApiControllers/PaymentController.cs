using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VNFarm.DTOs.Request;
using VNFarm.Enums;
using VNFarm.Services.External.Interfaces;
using VNFarm.Services.Interfaces;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IProductService _productService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public PaymentController(
            ILogger<PaymentController> logger,
            IOrderService orderService,
            IPaymentService paymentService,
            IJwtTokenService jwtTokenService,
            IProductService productService,
            IEmailService emailService,
            IUserService userService)
        {
            _logger = logger;
            _paymentService = paymentService;
            _orderService = orderService;
            _jwtTokenService = jwtTokenService;
            _productService = productService;
            _emailService = emailService;
            _userService = userService;
        }

        protected int? GetCurrentUserId()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return null;

            var token = authHeader.Substring("Bearer ".Length).Trim();
            return _jwtTokenService.GetUserIdFromToken(token);
        }

        [HttpPost("CreatePaymentUrl")]
        public async Task<ActionResult<string>> CreatePaymentUrl([FromBody] CheckoutUrlRequest checkoutUrlRequest)
        {
            try
            {
                var order = (await _orderService.FindAsync(x => x.OrderCode == checkoutUrlRequest.OrderId)).FirstOrDefault();
                if (order == null)
                {
                    return BadRequest(new {
                        message = "Không tìm thấy đơn hàng",
                        success = false
                    });
                }
                // Kiểm tra user có phải là người mua không
                if (order.BuyerId != GetCurrentUserId())
                {
                    return BadRequest(new {
                        message = "Bạn không có quyền thanh toán đơn hàng này",
                        success = false
                    });
                }
                // Kiểm tra trạng thái đơn hàng có thể thanh toán không
                bool validStatus = order.Status != OrderStatus.Cancelled && order.Status != OrderStatus.Refunded && order.Status != OrderStatus.Completed && order.Status != OrderStatus.All;
                bool validPaymentStatus = order.PaymentStatus != PaymentStatus.Paid && order.PaymentStatus != PaymentStatus.Refunded;
                if (!validStatus || !validPaymentStatus)
                {
                    return BadRequest(new {
                        message = "Đơn hàng không thể thanh toán",
                        success = false
                    });
                }

                var ipAddress = NetworkHelper.GetIpAddress(HttpContext);
                long paymentId = DateTime.Now.Ticks;
                var request = new PaymentRequest
                {
                    PaymentId = paymentId,
                    Money = (double)order.TotalAmount,
                    Description = order.OrderCode,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY,
                    CreatedDate = DateTime.Now,
                    Currency = Currency.VND,
                    Language = DisplayLanguage.Vietnamese
                };

                bool? result = await _orderService.SetOrderPaymentIdAsync(order.Id, paymentId);
                if (result == null)
                {
                    return BadRequest(new {
                        message = "Cập nhật id thanh toán thất bại",
                        success = false
                    });
                }
                var paymentUrl = await _paymentService.GetPaymentUrl(request);

                return Ok(new {
                    message = "Tạo url thanh toán thành công",
                    success = true,
                    data = paymentUrl
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*
        // For production
        [HttpGet("IpnAction")]
        public async Task<IActionResult> IpnAction()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _paymentService.GetPaymentResult(Request.Query);
                    if (paymentResult.IsSuccess)
                    {
                        // Thực hiện hành động nếu thanh toán thành công tại đây. Ví dụ: Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu.
                        var order = (await _orderService.FindAsync(x => x.OrderPaymentId == paymentResult.PaymentId)).FirstOrDefault();
                        if (order == null)
                        {
                            return BadRequest("Không tìm thấy đơn hàng");
                        }
                        bool? result = await _orderService.SetOrderPaymentStatusAsync(order.Id, PaymentStatus.Paid);
                        if (result == null)
                        {
                            _logger.LogError("Cập nhật trạng thái thanh toán thất bại");
                        }
                        else{
                            _logger.LogInformation("Cập nhật trạng thái thanh toán thành công");
                        }
                        return Ok("Thanh toán thành công. Bạn có thể đóng trang này !");
                    }

                    // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.
                    _logger.LogError("Thanh toán thất bại");
                    return BadRequest("Thanh toán thất bại");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi xử lý thanh toán");
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
        */

        [HttpGet("Callback")]
        public async Task<IActionResult> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var query = Request.QueryString.ToString();
                    var paymentResult = _paymentService.GetPaymentResult(Request.Query);
                    if (paymentResult.IsSuccess)
                    {
                        // Thực hiện hành động nếu thanh toán thành công tại đây. Ví dụ: Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu.
                        var order = (await _orderService.FindAsync(x => x.OrderPaymentId == paymentResult.PaymentId)).FirstOrDefault();
                        if (order == null)
                        {
                            return BadRequest("Không tìm thấy đơn hàng");
                        }
                        bool? result = await _orderService.SetOrderPaymentStatusAsync(order.Id, PaymentStatus.Paid);
                        if (result == null)
                        {
                            _logger.LogError("Cập nhật trạng thái thanh toán thất bại");
                        }
                        else{
                            _logger.LogInformation("Cập nhật trạng thái thanh toán thành công");
                            
                            // Cập nhật số lượng đã bán của sản phẩm
                            var orderItems = await _orderService.GetOrderItemsAsync(order.Id);
                            foreach (var item in orderItems)
                            {
                                await _productService.UpdateSoldQuantityAsync(item.ProductId, item.Quantity);
                            }

                            // Gửi email xác nhận thanh toán
                            var user = await _userService.GetByIdAsync(order.BuyerId);
                            if (user != null && user.Email != null)
                            {
                                await _emailService.SendOrderPaymentSuccessEmailAsync(user.Email, user.FullName, order, paymentResult);
                            }
                        }
                        
                    }
                    var redirectUrl = $"http://localhost:5011/Home/ResultPayment{query}";
                    return Redirect(redirectUrl);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi xử lý thanh toán");
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}

