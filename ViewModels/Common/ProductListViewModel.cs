using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Common
{
    public class ProductListViewModel
    {
        public ProductCriteriaFilter ProductCriteriaFilter { get; set; } = new();
        public List<ProductResponseDTO> Products { get; set; } = new();
        public List<CategoryResponseDTO> Categories { get; set; } = new();
    }

}
