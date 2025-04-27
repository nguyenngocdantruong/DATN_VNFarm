using System.ComponentModel.DataAnnotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class TransactionRequestDTO: BaseRequestDTO
    {
        #region Thông tin cơ bản
        public string? TransactionCode { get; set; }  = string.Empty;
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Mã người mua là bắt buộc")]
        public int BuyerId { get; set; }
        public string? Details { get; set; } = string.Empty;
        #endregion
        
        #region Thông tin thanh toán
        [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc")]
        public PaymentMethodEnum PaymentMethod { get; set; }
        #endregion
    }
} 