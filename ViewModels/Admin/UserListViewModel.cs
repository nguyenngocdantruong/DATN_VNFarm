using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Response;

namespace VNFarm.ViewModels.Admin
{
    public class UserListViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalVerifiedUsers { get; set; }
        public int TotalPendingUsers { get; set; }
        public int TotalRejectedUsers { get; set; }
        public UserCriteriaFilter UserCriteriaFilter { get; set; } = new();
        public List<UserResponseDTO> Users { get; set; } = new();
    }
}
