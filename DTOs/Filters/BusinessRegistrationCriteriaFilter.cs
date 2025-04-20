using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Filters
{
    public class BusinessRegistrationCriteriaFilter : BaseFilterCriteria
    {
        public StoreType BusinessType { get; set; } = StoreType.All;
        public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.All;
    }
}

