using System.Threading.Tasks;
using VNPAY.NET.Models;

namespace VNFarm.Interfaces.External
{
    public interface IPaymentService
    {
        Task<string> GetPaymentUrl(PaymentRequest request);
        PaymentResult GetPaymentResult(IQueryCollection query);
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        Task<PaymentResponse> VerifyPaymentAsync(string paymentId);
    }
} 