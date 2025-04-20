using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Response;

namespace VNFarm_FinalFinal.ViewModels.Common
{
    public class ShopListViewModel
    {
        public List<StoreResponseDTO> Stores { get; set; } = new();
        public StoreCriteriaFilter StoreCriteriaFilter { get; set; } = new();
    }
}
