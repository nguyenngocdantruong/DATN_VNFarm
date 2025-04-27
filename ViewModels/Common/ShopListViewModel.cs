using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Common
{
    public class ShopListViewModel
    {
        public List<StoreResponseDTO> Stores { get; set; } = new();
        public StoreCriteriaFilter StoreCriteriaFilter { get; set; } = new();
    }
}
