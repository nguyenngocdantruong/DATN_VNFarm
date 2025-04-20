using System.Collections.Generic;
using System.Threading.Tasks;

namespace VNFarm_FinalFinal.Interfaces.External
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendWelcomeEmailAsync(string to, string customerName);
        Task<bool> SendOrderConfirmationEmailAsync(string to, string customerName, string orderNumber, decimal orderTotal);
        Task<bool> SendPasswordResetEmailAsync(string to, string resetLink);
        Task<bool> SendAccountVerificationEmailAsync(string to, string verificationLink);
        Task<bool> SendOrderStatusUpdateEmailAsync(string to, string customerName, string orderNumber, string status);
        Task<bool> SendStoreApprovalEmailAsync(string to, string storeName);
        Task<bool> SendStoreRejectionEmailAsync(string to, string storeName, string reason);
        
        // Thêm các phương thức từ VNFarm_FinalFinal.Interfaces.Services.IEmailService
        Task<bool> SendVerificationEmailAsync(string to, string verificationToken);
        Task<bool> SendStoreVerificationResultAsync(string to, string storeName, bool isApproved, string reason);
        Task<bool> SendStoreStatusChangeAsync(string to, string storeName, bool isActive, string reason);
        Task<bool> SendOrderStatusUpdateAsync(string to, int orderId, string newStatus);
        Task<bool> SendBulkEmailAsync(List<string> recipients, string subject, string message);
        Task<bool> SendDisputeNotificationAsync(string to, int disputeId, string disputeTitle);
        Task<bool> SendNewDiscountNotificationAsync(string to, string discountCode, string discountDetails);
        Task<bool> SendOrderConfirmationAsync(string to, int orderId, string orderDetails);
        Task<bool> SendPasswordResetAsync(string to, string resetToken);
    }
} 