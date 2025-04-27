using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Admin
{
    public class CategoryListViewModel
    {
        public CategoryCriteriaFilter CategoryCriteriaFilter { get; set; } = new();
        public List<CategoryResponseDTO> Categories { get; set; } = new();
    }
}
