namespace VNFarm.DTOs.Filters
{
    public class CategoryCriteriaFilter : BaseFilterCriteria
    {
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 100000000;

    }
}

