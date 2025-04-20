using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Request
{
    public class OrderRequestDTO : BaseRequestDTO
    {
        #region Thông tin chung
        public string OrderCode { get; set; } = "";
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string Notes { get; set; } = "";
        public string? DiscountCode { get; set; }
        #endregion

        #region Thông tin thanh toán
        [EnumDataType(typeof(PaymentMethodEnum))]
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.BankTransfer;
        #endregion
        
        #region Foreign keys
        public int BuyerId { get; set; }
        public int StoreId { get; set; }
        #endregion
    }
}