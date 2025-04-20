using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Payment;

namespace VNFarm_FinalFinal.Interfaces.External
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