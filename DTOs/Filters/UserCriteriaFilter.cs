using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class UserCriteriaFilter : BaseFilterCriteria
    {
        public UserRole Role { get; set; } = UserRole.All;
        public bool? IsActive { get; set; }
        public bool? EmailVerified { get; set; }
    }
}

