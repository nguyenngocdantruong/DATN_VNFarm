using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.DTOs.Filters;

namespace VNFarm_FinalFinal.ViewModels.Common
{
    public class ProductListViewModel
    {
        public ProductCriteriaFilter ProductCriteriaFilter { get; set; } = new();
        public List<ProductResponseDTO> Products { get; set; } = new();
        public List<CategoryResponseDTO> Categories { get; set; } = new();
    }

}
