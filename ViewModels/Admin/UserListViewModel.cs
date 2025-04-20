using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.DTOs.Filters;

namespace VNFarm_FinalFinal.ViewModels.Admin
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
