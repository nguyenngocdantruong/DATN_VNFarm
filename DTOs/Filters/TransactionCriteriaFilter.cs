using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Filters
{
    public class TransactionCriteriaFilter : BaseFilterCriteria
    {
        public decimal? MinAmount { get; set; } = 0;
        public decimal? MaxAmount { get; set; } = decimal.MaxValue;
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.All;
        public TransactionStatus Status { get; set; } = TransactionStatus.All;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

