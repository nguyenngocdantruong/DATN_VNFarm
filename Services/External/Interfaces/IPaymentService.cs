using System.Threading.Tasks;
using VNPAY.NET.Models;

namespace VNFarm.Services.External.Interfaces
{
    public interface IPaymentService
    {
        Task<string> GetPaymentUrl(PaymentRequest request);
        PaymentResult GetPaymentResult(IQueryCollection query);
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        Task<PaymentResponse> VerifyPaymentAsync(string paymentId);
    }
} 