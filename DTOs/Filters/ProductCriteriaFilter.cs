using System.Collections.Generic;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Filters
{
    public class ProductCriteriaFilter : BaseFilterCriteria
    {
        public decimal? MinPrice { get; set; } = 0;
        public decimal? MaxPrice { get; set; } = decimal.MaxValue;
        public int CategoryId { get; set; } = -999;
        public string Origin { get; set; } = "";
        public bool? IsActive { get; set; } = true;
        public bool? IsInStock { get; set; } = true;
        public Unit Unit { get; set; } = Unit.All;
    }
} 