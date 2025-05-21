using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.DTOs.Response;
using VNPAY.NET.Models;

namespace VNFarm.Services.External.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendWelcomeEmailAsync(string to, string customerName);
        Task<bool> SendOrderConfirmationEmailAsync(string to, string customerName, string orderNumber, decimal orderTotal);
        Task<bool> SendPasswordResetEmailAsync(string to, string resetLink);
        Task<bool> SendAccountVerificationEmailAsync(string to, string verificationLink);
        Task<bool> SendOrderStatusUpdateEmailAsync(string to, string customerName, string orderNumber, string status);
        Task<bool> SendOrderPaymentSuccessEmailAsync(string to, string customerName, OrderResponseDTO order, PaymentResult paymentResult);
        Task<bool> SendUserActiveEmailAsync(string to, string customerName, bool isActive);
        Task<bool> SendDiscountCreatedEmailAsync(string to, string customerName, string discountCode, string discountDescription, DateTime startDate, DateTime endDate, string discountAmount, int remainingQuantity);
    }
} 