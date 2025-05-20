using System;
using System.Collections.Generic;
using VNFarm.DTOs.Response;
using VNFarm.Enums;

namespace VNFarm.ViewModels.Admin
{
    public class DiscountViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";
        public int RemainingQuantity { get; set; }
        public DiscountStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DiscountType Type { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public decimal MaximumDiscountAmount { get; set; }
        public int? StoreId { get; set; }
        public StoreResponseDTO? Store { get; set; }
    }

    public class DiscountListViewModel
    {
        public List<DiscountResponseDTO> Discounts { get; set; } = new List<DiscountResponseDTO>();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);
    }
} 