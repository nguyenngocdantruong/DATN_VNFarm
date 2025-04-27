using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Admin
{
    public class ActiveUserViewModel
    {
        public int TotalRecords { get; set; }
        public List<UserResponseDTO> Users { get; set; } = new();
        public List<StoreResponseDTO> Stores { get; set; } = new();
        public UserCriteriaFilter UserCriteriaFilter { get; set; } = new();
        public StoreCriteriaFilter StoreCriteriaFilter { get; set; } = new();
    }
}
