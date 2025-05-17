using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.Interfaces.External;

namespace VNFarm.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            var result = await _emailService.SendEmailAsync(request.To, request.Subject, request.Body, request.IsHtml);
            if (result)
                return Ok(new { success = true, message = "Email đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email" });
        }

        [HttpPost("welcome")]
        public async Task<IActionResult> SendWelcomeEmail([FromBody] WelcomeEmailRequest request)
        {
            var result = await _emailService.SendWelcomeEmailAsync(request.To, request.CustomerName);
            if (result)
                return Ok(new { success = true, message = "Email chào mừng đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email chào mừng" });
        }

        [HttpPost("order-confirmation")]
        public async Task<IActionResult> SendOrderConfirmation([FromBody] OrderConfirmationRequest request)
        {
            var result = await _emailService.SendOrderConfirmationEmailAsync(
                request.To, 
                request.CustomerName, 
                request.OrderNumber, 
                request.OrderTotal);
                
            if (result)
                return Ok(new { success = true, message = "Email xác nhận đơn hàng đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email xác nhận đơn hàng" });
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> SendPasswordReset([FromBody] PasswordResetRequest request)
        {
            var result = await _emailService.SendPasswordResetAsync(request.To, request.ResetToken);
            if (result)
                return Ok(new { success = true, message = "Email đặt lại mật khẩu đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email đặt lại mật khẩu" });
        }

        [HttpPost("verification")]
        public async Task<IActionResult> SendVerification([FromBody] VerificationRequest request)
        {
            var result = await _emailService.SendVerificationEmailAsync(request.To, request.VerificationToken);
            if (result)
                return Ok(new { success = true, message = "Email xác thực đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email xác thực" });
        }

        [HttpPost("store-status")]
        public async Task<IActionResult> SendStoreStatus([FromBody] StoreStatusRequest request)
        {
            var result = await _emailService.SendStoreStatusChangeAsync(
                request.To, 
                request.StoreName, 
                request.IsActive, 
                request.Reason);
                
            if (result)
                return Ok(new { success = true, message = "Email thông báo trạng thái cửa hàng đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email thông báo trạng thái cửa hàng" });
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> SendBulkEmail([FromBody] BulkEmailRequest request)
        {
            var result = await _emailService.SendBulkEmailAsync(request.Recipients, request.Subject, request.Message);
            if (result)
                return Ok(new { success = true, message = "Email hàng loạt đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email hàng loạt" });
        }

        [HttpPost("dispute")]
        public async Task<IActionResult> SendDisputeNotification([FromBody] DisputeRequest request)
        {
            var result = await _emailService.SendDisputeNotificationAsync(
                request.To, 
                request.DisputeId, 
                request.DisputeTitle);
                
            if (result)
                return Ok(new { success = true, message = "Email thông báo tranh chấp đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email thông báo tranh chấp" });
        }

        [HttpPost("discount")]
        public async Task<IActionResult> SendDiscountNotification([FromBody] DiscountRequest request)
        {
            var result = await _emailService.SendNewDiscountNotificationAsync(
                request.To, 
                request.DiscountCode, 
                request.DiscountDetails);
                
            if (result)
                return Ok(new { success = true, message = "Email thông báo mã giảm giá đã được gửi thành công" });
            
            return BadRequest(new { success = false, message = "Không thể gửi email thông báo mã giảm giá" });
        }
    }

    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; } = true;
    }

    public class WelcomeEmailRequest
    {
        public string To { get; set; }
        public string CustomerName { get; set; }
    }

    public class OrderConfirmationRequest
    {
        public string To { get; set; }
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
        public decimal OrderTotal { get; set; }
    }

    public class PasswordResetRequest
    {
        public string To { get; set; }
        public string ResetToken { get; set; }
    }

    public class VerificationRequest
    {
        public string To { get; set; }
        public string VerificationToken { get; set; }
    }

    public class StoreStatusRequest
    {
        public string To { get; set; }
        public string StoreName { get; set; }
        public bool IsActive { get; set; }
        public string Reason { get; set; }
    }

    public class BulkEmailRequest
    {
        public List<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class DisputeRequest
    {
        public string To { get; set; }
        public int DisputeId { get; set; }
        public string DisputeTitle { get; set; }
    }

    public class DiscountRequest
    {
        public string To { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountDetails { get; set; }
    }
}
