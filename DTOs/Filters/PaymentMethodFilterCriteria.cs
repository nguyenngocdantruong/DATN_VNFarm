using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Filters
{
    public class PaymentMethodFilterCriteria : BaseFilterCriteria
    {
        public PaymentType PaymentType { get; set; } = PaymentType.All;
        public string AccountHolderName { get; set; } = "";
        public string BankName { get; set; } = "";
    }
}

