using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.DTOs.Filters;

namespace VNFarm_FinalFinal.ViewModels.Admin
{
    public class CategoryListViewModel
    {
        public CategoryCriteriaFilter CategoryCriteriaFilter { get; set; } = new();
        public List<CategoryResponseDTO> Categories { get; set; } = new();
    }
}
