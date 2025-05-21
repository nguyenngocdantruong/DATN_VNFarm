using System;
using VNFarm.Enums;

namespace VNFarm.DTOs.Filters
{
    public class OrderCriteriaFilter : BaseFilterCriteria
    {
        public OrderStatus? Status { get; set; } = OrderStatus.All;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.All;
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.All;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MinTotal { get; set; } = 0;
        public int? MaxTotal { get; set; } = 100000000;
        public int? StoreId {get;set;}
        public int? UserId {get;set;}
    }
} 