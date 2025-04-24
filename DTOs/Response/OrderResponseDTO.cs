using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace VNFarm_FinalFinal.DTOs.Response
{
    public class OrderResponseDTO : BaseResponseDTO
    {
        #region Thông tin chung
        public required string OrderCode { get; set; }
        public required OrderStatus Status { get; set; }
        public required string Notes { get; set; }
        public required AddressResponseDTO Address { get; set; }        
        public required ShippingResponseDTO Shipping { get; set; }
        #endregion
        
        #region Thông tin giá cả
        public required decimal TotalAmount { get; set; }
        public required decimal ShippingFee { get; set; }
        public required decimal TaxAmount { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required decimal FinalAmount { get; set; }
        #endregion
        
        #region Thông tin thanh toán
        public required PaymentStatus PaymentStatus { get; set; }
        public required PaymentMethodEnum PaymentMethod { get; set; }
        public DateTime? PaidAt { get; set; }
        #endregion
        
        #region Foreign keys
        public int BuyerId { get; set; }
        public int? StoreId { get; set; }
        public int? DiscountId { get; set; }
        #endregion
        
        #region Related Properties
        public UserResponseDTO? Buyer { get; set; }
        public StoreResponseDTO? Store { get; set; }
        public DiscountResponseDTO? Discount { get; set; }
        public List<OrderDetailResponseDTO> OrderDetails { get; set; } = [];
        public List<OrderTimelineResponseDTO?> OrderTimelines { get; set; } = [];
        #endregion
    }
}