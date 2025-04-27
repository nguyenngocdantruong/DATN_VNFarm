using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Admin
{
    public class RegisterShopListViewModel
    {
        public int TotalRegisterShops { get; set; } = 0;
        public int TotalVerifiedShops { get; set; } = 0;
        public int TotalPendingShops { get; set; } = 0;
        public int TotalRejectedShops { get; set; } = 0;
        public StoreCriteriaFilter StoreCriteriaFilter { get; set; } = new();
        public List<StoreResponseDTO> Stores { get; set; } = new();
    }
}
