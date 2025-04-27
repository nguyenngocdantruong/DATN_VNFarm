using System.Threading.Tasks;
using VNFarm.DTOs.Payment;

namespace VNFarm.Interfaces.External
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        Task<PaymentResponse> VerifyPaymentAsync(string paymentId);
        Task<RefundResponse> ProcessRefundAsync(RefundRequest request);
        Task<PaymentMethodResponse> GetPaymentMethodsAsync();
        Task<CheckoutUrlResponse> CreateCheckoutUrlAsync(CheckoutRequest request);
    }
} 