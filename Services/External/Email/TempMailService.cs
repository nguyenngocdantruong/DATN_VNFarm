using VNFarm.DTOs.Response;
using VNFarm.Services.External.Interfaces;
using VNPAY.NET.Models;

namespace VNFarm.ExternalServices.Email
{
    public class TempMailService : IEmailService
    {
        private readonly ILogger<TempMailService> _logger;
        public TempMailService(ILogger<TempMailService> logger)
        {
            _logger = logger;
        }
        public Task<bool> SendAccountVerificationEmailAsync(string to, string verificationLink)
        {
            _logger.LogInformation("Đã gửi email xác thực tài khoản đến {Email} với link xác thực {Link}", to, verificationLink);
            return Task.FromResult(true);
        }

        public Task<bool> SendBulkEmailAsync(List<string> recipients, string subject, string message)
        {
            _logger.LogInformation("Đã gửi email hàng loạt đến {RecipientCount} người nhận với chủ đề: {Subject}", recipients.Count, subject);
            return Task.FromResult(true);
        }

        public Task<bool> SendDiscountCreatedEmailAsync(string to, string customerName, string discountCode, string discountDescription, DateTime startDate, DateTime endDate, string discountAmount, int remainingQuantity)
        {
            var message = $"Mã giảm giá: {discountCode}\n" +    
                          $"Chi tiết: {discountDescription}\n" +
                          $"Ngày bắt đầu: {startDate}\n" +
                          $"Ngày kết thúc: {endDate}\n" +
                          $"Giá trị giảm: {discountAmount}\n" +
                          $"Số lượng còn lại: {remainingQuantity}";
            _logger.LogInformation(message);
            return Task.FromResult(true);
        }

        public Task<bool> SendDisputeNotificationAsync(string to, int disputeId, string disputeTitle)
        {
            _logger.LogInformation("Đã gửi thông báo tranh chấp đến {Email} về tranh chấp {DisputeId}: {DisputeTitle}", to, disputeId, disputeTitle);
            return Task.FromResult(true);
        }

        public Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            _logger.LogInformation("Đã gửi email đến {Email} với chủ đề: {Subject} (Body: {Body})", to, subject, body);
            return Task.FromResult(true);
        }

        public Task<bool> SendNewDiscountNotificationAsync(string to, string discountCode, string discountDetails)
        {
            _logger.LogInformation("Đã gửi thông báo mã giảm giá mới đến {Email} với mã: {DiscountCode}, chi tiết: {Details}", to, discountCode, discountDetails);
            return Task.FromResult(true);
        }

        public Task<bool> SendOrderConfirmationAsync(string to, int orderId, string orderDetails)
        {
            _logger.LogInformation("Đã gửi xác nhận đơn hàng đến {Email} cho đơn hàng {OrderId} với chi tiết: {Details}", to, orderId, orderDetails);
            return Task.FromResult(true);
        }

        public Task<bool> SendOrderConfirmationEmailAsync(string to, string customerName, string orderNumber, decimal orderTotal)
        {
            _logger.LogInformation("Đã gửi email xác nhận đơn hàng đến {Email} cho khách hàng {CustomerName}, đơn hàng số {OrderNumber} với tổng giá trị {OrderTotal}", to, customerName, orderNumber, orderTotal);
            return Task.FromResult(true);
        }

        public Task<bool> SendOrderPaymentSuccessEmailAsync(string to, string customerName, OrderResponseDTO order, PaymentResult paymentResult)
        {
            _logger.LogInformation("Đã gửi email thành công thanh toán đơn hàng đến {Email} cho khách hàng {CustomerName}, đơn hàng số {OrderNumber} với tổng giá trị {OrderTotal}", to, customerName, order.OrderCode, order.TotalAmount);
            return Task.FromResult(true);
        }

        public Task<bool> SendOrderStatusUpdateAsync(string to, int orderId, string newStatus)
        {
            _logger.LogInformation("Đã gửi cập nhật trạng thái đơn hàng đến {Email} cho đơn hàng {OrderId}, trạng thái mới: {Status}", to, orderId, newStatus);
            return Task.FromResult(true);
        }

        public Task<bool> SendOrderStatusUpdateEmailAsync(string to, string customerName, string orderNumber, string status)
        {
            _logger.LogInformation("Đã gửi email cập nhật trạng thái đơn hàng đến {Email} cho khách hàng {CustomerName}, đơn hàng số {OrderNumber}, trạng thái: {Status}", to, customerName, orderNumber, status);
            return Task.FromResult(true);
        }

        public Task<bool> SendPasswordResetAsync(string to, string resetToken)
        {
            _logger.LogInformation("Đã gửi yêu cầu đặt lại mật khẩu đến {Email} với token: {ResetToken}", to, resetToken);
            return Task.FromResult(true);
        }

        public Task<bool> SendPasswordResetEmailAsync(string to, string resetLink)
        {
            _logger.LogInformation("Đã gửi email đặt lại mật khẩu đến {Email} với link đặt lại: {ResetLink}", to, resetLink);
            return Task.FromResult(true);
        }

        public Task<bool> SendStoreApprovalEmailAsync(string to, string storeName)
        {
            _logger.LogInformation("Đã gửi email phê duyệt cửa hàng đến {Email} cho cửa hàng: {StoreName}", to, storeName);
            return Task.FromResult(true);
        }

        public Task<bool> SendStoreRejectionEmailAsync(string to, string storeName, string reason)
        {
            _logger.LogInformation("Đã gửi email từ chối cửa hàng đến {Email} cho cửa hàng: {StoreName}, lý do: {Reason}", to, storeName, reason);
            return Task.FromResult(true);
        }

        public Task<bool> SendStoreStatusChangeAsync(string to, string storeName, bool isActive, string reason)
        {
            _logger.LogInformation("Đã gửi thông báo thay đổi trạng thái cửa hàng đến {Email} cho cửa hàng: {StoreName}, trạng thái hoạt động: {IsActive}, lý do: {Reason}", to, storeName, isActive, reason);
            return Task.FromResult(true);
        }

        public Task<bool> SendStoreVerificationResultAsync(string to, string storeName, bool isApproved, string reason)
        {
            _logger.LogInformation("Đã gửi kết quả xác minh cửa hàng đến {Email} cho cửa hàng: {StoreName}, được phê duyệt: {IsApproved}, lý do: {Reason}", to, storeName, isApproved, reason);
            return Task.FromResult(true);
        }

        public Task<bool> SendUserActiveEmailAsync(string to, string customerName, bool isActive)
        {
            _logger.LogInformation("Đã gửi email kích hoạt tài khoản đến {Email} cho khách hàng: {CustomerName}, trạng thái: {IsActive}", to, customerName, isActive);
            return Task.FromResult(true);
        }

        public Task<bool> SendVerificationEmailAsync(string to, string verificationToken)
        {
            _logger.LogInformation("Đã gửi email xác minh đến {Email} với token xác minh: {VerificationToken}", to, verificationToken);
            return Task.FromResult(true);
        }

        public Task<bool> SendWelcomeEmailAsync(string to, string customerName)
        {
            _logger.LogInformation("Đã gửi email chào mừng đến {Email} cho khách hàng: {CustomerName}", to, customerName);
            return Task.FromResult(true);
        }
    }
}
