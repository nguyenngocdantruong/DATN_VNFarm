using System;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Filters
{
    public class OrderCriteriaFilter : BaseFilterCriteria
    {
        public OrderStatus? Status { get; set; } = OrderStatus.All;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.All;
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.All;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinTotal { get; set; } = 0;
        public decimal? MaxTotal { get; set; } = decimal.MaxValue;
        public int? StoreId {get;set;}
    }
} 