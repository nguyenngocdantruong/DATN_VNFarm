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
using VNFarm_FinalFinal.DTOs.Payment;
using VNFarm_FinalFinal.Interfaces.External;

namespace VNFarm.Infrastructure.External.Payment
{
    public class VNPayService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<VNPayService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _vnpUrl;
        private readonly string _vnpReturnUrl;
        private readonly string _vnpTmnCode;
        private readonly string _vnpHashSecret;
        private readonly string _vnpApiUrl;
        private readonly string _vnpApiVersion;

        public VNPayService(
            IConfiguration configuration,
            ILogger<VNPayService> logger,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;

            _vnpUrl = _configuration["VNPay:PaymentUrl"];
            _vnpReturnUrl = _configuration["VNPay:ReturnUrl"];
            _vnpTmnCode = _configuration["VNPay:TmnCode"];
            _vnpHashSecret = _configuration["VNPay:HashSecret"];
            _vnpApiUrl = _configuration["VNPay:ApiUrl"];
            _vnpApiVersion = _configuration["VNPay:Version"];
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                var vnpParams = new Dictionary<string, string>
                {
                    ["vnp_Version"] = _vnpApiVersion,
                    ["vnp_Command"] = "pay",
                    ["vnp_TmnCode"] = _vnpTmnCode,
                    ["vnp_Amount"] = (request.Amount * 100).ToString(CultureInfo.InvariantCulture),
                    ["vnp_CreateDate"] = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ["vnp_CurrCode"] = request.Currency,
                    ["vnp_IpAddr"] = "127.0.0.1",
                    ["vnp_Locale"] = "vn",
                    ["vnp_OrderInfo"] = $"Thanh toan don hang #{request.OrderId}",
                    ["vnp_OrderType"] = "other",
                    ["vnp_ReturnUrl"] = request.ReturnUrl ?? _vnpReturnUrl,
                    ["vnp_TxnRef"] = request.OrderId.ToString()
                };

                vnpParams = SortDictionary(vnpParams);
                var signData = string.Join("&", Array.ConvertAll(vnpParams.Keys.ToArray(), key => $"{key}={vnpParams[key]}"));
                var signature = ComputeHmacSha512(signData, _vnpHashSecret);
                vnpParams["vnp_SecureHash"] = signature;

                var queryString = string.Join("&", Array.ConvertAll(vnpParams.Keys.ToArray(), key => $"{key}={HttpUtility.UrlEncode(vnpParams[key])}"));
                var paymentUrl = $"{_vnpUrl}?{queryString}";

                return new PaymentResponse
                {
                    PaymentId = request.OrderId.ToString(),
                    Status = "Pending",
                    RedirectUrl = paymentUrl,
                    Success = true,
                    Message = "Đã tạo URL thanh toán thành công"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi xử lý thanh toán VNPay: {ex.Message}");
                return new PaymentResponse
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xử lý thanh toán"
                };
            }
        }

        public async Task<PaymentResponse> VerifyPaymentAsync(string paymentId)
        {
            try
            {
                var vnpParams = new Dictionary<string, string>
                {
                    ["vnp_Version"] = _vnpApiVersion,
                    ["vnp_Command"] = "querydr",
                    ["vnp_TmnCode"] = _vnpTmnCode,
                    ["vnp_TxnRef"] = paymentId,
                    ["vnp_OrderInfo"] = $"Kiểm tra trạng thái giao dịch #{paymentId}",
                    ["vnp_CreateDate"] = DateTime.Now.ToString("yyyyMMddHHmmss")
                };

                vnpParams = SortDictionary(vnpParams);
                var signData = string.Join("&", Array.ConvertAll(vnpParams.Keys.ToArray(), key => $"{key}={vnpParams[key]}"));
                var signature = ComputeHmacSha512(signData, _vnpHashSecret);
                vnpParams["vnp_SecureHash"] = signature;

                var queryString = string.Join("&", Array.ConvertAll(vnpParams.Keys.ToArray(), key => $"{key}={HttpUtility.UrlEncode(vnpParams[key])}"));
                var requestUrl = $"{_vnpApiUrl}?{queryString}";
                
                var response = await _httpClient.GetAsync(requestUrl);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Giả lập kết quả từ VNPay vì đây là môi trường phát triển
                return new PaymentResponse
                {
                    PaymentId = paymentId,
                    Status = "Success",
                    Success = true,
                    Message = "Giao dịch thành công",
                    AdditionalData = new Dictionary<string, string>
                    {
                        ["TransactionDate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi xác minh thanh toán VNPay: {ex.Message}");
                return new PaymentResponse
                {
                    PaymentId = paymentId,
                    Status = "Error",
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xác minh thanh toán"
                };
            }
        }

        public async Task<RefundResponse> ProcessRefundAsync(RefundRequest request)
        {
            try
            {
                var vnpParams = new Dictionary<string, string>
                {
                    ["vnp_Version"] = _vnpApiVersion,
                    ["vnp_Command"] = "refund",
                    ["vnp_TmnCode"] = _vnpTmnCode,
                    ["vnp_TransactionType"] = "02",
                    ["vnp_TxnRef"] = request.PaymentId,
                    ["vnp_Amount"] = (request.Amount * 100).ToString(CultureInfo.InvariantCulture),
                    ["vnp_OrderInfo"] = request.Reason,
                    ["vnp_TransactionNo"] = request.PaymentId,
                    ["vnp_TransactionDate"] = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ["vnp_CreateBy"] = "System"
                };

                vnpParams = SortDictionary(vnpParams);
                var signData = string.Join("&", Array.ConvertAll(vnpParams.Keys.ToArray(), key => $"{key}={vnpParams[key]}"));
                var signature = ComputeHmacSha512(signData, _vnpHashSecret);
                vnpParams["vnp_SecureHash"] = signature;

                var content = new FormUrlEncodedContent(vnpParams);
                var response = await _httpClient.PostAsync(_vnpApiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Giả lập kết quả từ VNPay vì đây là môi trường phát triển
                return new RefundResponse
                {
                    RefundId = Guid.NewGuid().ToString(),
                    Status = "Success",
                    Success = true,
                    Message = "Hoàn tiền thành công"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi xử lý hoàn tiền VNPay: {ex.Message}");
                return new RefundResponse
                {
                    Status = "Error",
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xử lý hoàn tiền"
                };
            }
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodsAsync()
        {
            try
            {
                // Giả lập danh sách phương thức thanh toán VNPay
                var methods = new List<PaymentMethod>
                {
                    new PaymentMethod
                    {
                        Id = "VNPAYQR",
                        Name = "Thanh toán qua VNPay QR",
                        Description = "Quét mã QR để thanh toán",
                        ImageUrl = "/images/payment/vnpay-qr.png",
                        IsActive = true
                    },
                    new PaymentMethod
                    {
                        Id = "VNBANK",
                        Name = "Thanh toán qua thẻ ATM/Internet Banking",
                        Description = "Thanh toán qua ngân hàng liên kết với VNPay",
                        ImageUrl = "/images/payment/vnpay-bank.png",
                        IsActive = true
                    },
                    new PaymentMethod
                    {
                        Id = "INTCARD",
                        Name = "Thanh toán qua thẻ quốc tế",
                        Description = "Thanh toán qua thẻ Visa, MasterCard, JCB",
                        ImageUrl = "/images/payment/vnpay-card.png",
                        IsActive = true
                    }
                };

                return new PaymentMethodResponse
                {
                    PaymentMethods = methods,
                    Success = true,
                    Message = "Lấy danh sách phương thức thanh toán thành công"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi lấy danh sách phương thức thanh toán: {ex.Message}");
                return new PaymentMethodResponse
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách phương thức thanh toán"
                };
            }
        }

        public async Task<CheckoutUrlResponse> CreateCheckoutUrlAsync(CheckoutRequest request)
        {
            try
            {
                var vnpParams = new Dictionary<string, string>
                {
                    ["vnp_Version"] = _vnpApiVersion,
                    ["vnp_Command"] = "pay",
                    ["vnp_TmnCode"] = _vnpTmnCode,
                    ["vnp_Amount"] = (request.Amount * 100).ToString(CultureInfo.InvariantCulture),
                    ["vnp_BankCode"] = "",
                    ["vnp_CreateDate"] = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ["vnp_CurrCode"] = request.Currency,
                    ["vnp_IpAddr"] = "127.0.0.1",
                    ["vnp_Locale"] = "vn",
                    ["vnp_OrderInfo"] = request.Description,
                    ["vnp_OrderType"] = "other",
                    ["vnp_ReturnUrl"] = request.ReturnUrl ?? _vnpReturnUrl,
                    ["vnp_TxnRef"] = request.OrderId.ToString()
                };

                vnpParams = SortDictionary(vnpParams);
                var signData = string.Join("&", Array.ConvertAll(vnpParams.Keys.ToArray(), key => $"{key}={vnpParams[key]}"));
                var signature = ComputeHmacSha512(signData, _vnpHashSecret);
                vnpParams["vnp_SecureHash"] = signature;

                var queryString = string.Join("&", Array.ConvertAll(vnpParams.Keys.ToArray(), key => $"{key}={HttpUtility.UrlEncode(vnpParams[key])}"));
                var paymentUrl = $"{_vnpUrl}?{queryString}";

                return new CheckoutUrlResponse
                {
                    CheckoutUrl = paymentUrl,
                    CheckoutId = Guid.NewGuid().ToString(),
                    Success = true,
                    Message = "Tạo URL thanh toán thành công",
                    ExpiryTime = DateTime.Now.AddHours(24)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tạo URL thanh toán VNPay: {ex.Message}");
                return new CheckoutUrlResponse
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo URL thanh toán"
                };
            }
        }

        private string ComputeHmacSha512(string data, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private Dictionary<string, string> SortDictionary(Dictionary<string, string> dictionary)
        {
            return dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }
    }
} 