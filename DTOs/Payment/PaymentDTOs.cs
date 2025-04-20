using System;
using System.Collections.Generic;

namespace VNFarm_FinalFinal.DTOs.Payment
{
    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string PaymentMethod { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; } = new Dictionary<string, string>();
    }

    public class PaymentResponse
    {
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public string RedirectUrl { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public Dictionary<string, string> AdditionalData { get; set; } = new Dictionary<string, string>();
    }

    public class RefundRequest
    {
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }

    public class RefundResponse
    {
        public string RefundId { get; set; }
        public string Status { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class PaymentMethod
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class PaymentMethodResponse
    {
        public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class CheckoutRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string Description { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }

    public class CheckoutUrlResponse
    {
        public string CheckoutUrl { get; set; }
        public string CheckoutId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
} 