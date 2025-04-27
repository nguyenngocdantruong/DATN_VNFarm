using System;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class PaymentMethodRequestDTO : BaseRequestDTO
    {
        #region Thông tin cơ bản
        [Required(ErrorMessage = "Tên thẻ là bắt buộc")]
        public string CardName { get; set; } = "";
        [EnumDataType(typeof(PaymentType))]
        public PaymentType PaymentType { get; set; } = PaymentType.Bank;
        [Required(ErrorMessage = "Số tài khoản là bắt buộc")]
        public string AccountNumber { get; set; } = "";
        [Required(ErrorMessage = "Tên chủ tài khoản là bắt buộc")]
        public string AccountHolderName { get; set; } = "";
        [Required(ErrorMessage = "Tên ngân hàng là bắt buộc")]
        public string BankName { get; set; } = "";
        #endregion
    }
} 