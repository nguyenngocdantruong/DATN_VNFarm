using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class BusinessRegistrationCriteriaFilter : BaseFilterCriteria
    {
        public StoreType BusinessType { get; set; } = StoreType.All;
        public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.All;
    }
}

