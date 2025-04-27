using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class DiscountResponseDTO : BaseResponseDTO
    {
        public required string Code { get; set; }
        public required string Description { get; set; }
        public required int RemainingQuantity { get; set; }
        public required DiscountStatus Status { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required DiscountType Type { get; set; }
        
        public int? StoreId { get; set; }
        
        public int? UserId { get; set; }

        public StoreResponseDTO? Store { get; set; }
        public UserResponseDTO? User { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required decimal MinimumOrderAmount { get; set; }
        public required decimal MaximumDiscountAmount { get; set; }
        
        public bool IsActive => Status == DiscountStatus.Active && DateTime.Now >= StartDate && DateTime.Now <= EndDate && RemainingQuantity > 0;
        public bool IsGlobal => !StoreId.HasValue && !UserId.HasValue;
        public string DiscountValueText => Type == DiscountType.Percentage ? $"{DiscountAmount}%" : $"{DiscountAmount:N0} VNĐ";
    }
}