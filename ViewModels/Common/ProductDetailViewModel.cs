using VNFarm_FinalFinal.DTOs.Response;

namespace VNFarm_FinalFinal.ViewModels.Common
{
    public class ProductDetailViewModel
    {
        public ProductResponseDTO? Product { get; set; } 
        public List<ProductResponseDTO> RelatedProducts { get; set; } = new();
        public List<ReviewResponseDTO> Reviews { get; set; } = new();
        public List<CategoryResponseDTO> Categories { get; set; } = new();
        public StoreResponseDTO? Store { get; set; } 
    }
}
