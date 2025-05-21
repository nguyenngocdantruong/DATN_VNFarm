using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VNFarm.Services.External.Interfaces;
using VNPAY.NET;
using VNPAY.NET.Models;

namespace VNFarm.ExternalServices.Payment
{
    public class VnpayPayment : IPaymentService
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;

        public VnpayPayment(IVnpay vnpay, IConfiguration configuration)
        {
            _vnpay = vnpay;
            _configuration = configuration;

            _vnpay.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:ReturnUrl"]);
        }

        public PaymentResult GetPaymentResult(IQueryCollection query)
        {
            return _vnpay.GetPaymentResult(query);
        }

        public async Task<string> GetPaymentUrl(PaymentRequest request)
        {
            return await Task.FromResult(_vnpay.GetPaymentUrl(request));
        }

        public Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResponse> VerifyPaymentAsync(string paymentId)
        {
            throw new NotImplementedException();
        }
    }
}