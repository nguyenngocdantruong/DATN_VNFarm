using System.Collections.Generic;
using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class ProductCriteriaFilter : BaseFilterCriteria
    {
        public int? MinPrice { get; set; } = 0;
        public int? MaxPrice { get; set; } = 100000000;
        public int CategoryId { get; set; } = -999;
        public int? StoreId { get; set; }
        public string Origin { get; set; } = "";
        public bool? IsActive { get; set; } = true;
        public bool? IsInStock { get; set; } = true;
        public Unit Unit { get; set; } = Unit.All;
    }
} 