namespace VNFarm_FinalFinal.DTOs.Filters
{
    public class CategoryCriteriaFilter : BaseFilterCriteria
    {
        public decimal MinPrice { get; set; } = 0;
        public decimal MaxPrice { get; set; } = decimal.MaxValue;

    }
}

