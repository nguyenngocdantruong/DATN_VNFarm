using System.Collections.Generic;
using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class CheckoutResponseDTO : BaseResponseDTO
    {
        // Thông tin giỏ hàng
        public required List<ShopCartResponseDTO> ShopCarts { get; set; }
        
        // Thông tin địa chỉ giao hàng
        public required string ShippingName { get; set; }
        public required string ShippingPhone { get; set; }
        public required string ShippingAddress { get; set; }
        public required string ShippingProvince { get; set; }
        public required string ShippingDistrict { get; set; }
        public required string ShippingWard { get; set; }
        
        // Thông tin thanh toán
        public required PaymentMethodEnum PaymentMethod { get; set; }
        
        // Thông tin giá cả
        public required decimal SubTotal { get; set; }
        public required decimal ShippingFee { get; set; }
        public required decimal TaxAmount { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required decimal FinalAmount { get; set; }
        
        // Thông tin khác
        public required string Notes { get; set; }
        public string? DiscountCode { get; set; }
        public DiscountResponseDTO? Discount { get; set; }
    }
} 