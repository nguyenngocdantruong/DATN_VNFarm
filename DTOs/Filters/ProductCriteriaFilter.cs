using System.Collections.Generic;
using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class ProductCriteriaFilter : BaseFilterCriteria
    {
        public decimal? MinPrice { get; set; } = 0;
        public decimal? MaxPrice { get; set; } = decimal.MaxValue;
        public int CategoryId { get; set; } = -999;
        public int? StoreId { get; set; }
        public string Origin { get; set; } = "";
        public bool? IsActive { get; set; } = true;
        public bool? IsInStock { get; set; } = true;
        public Unit Unit { get; set; } = Unit.All;
    }
} 