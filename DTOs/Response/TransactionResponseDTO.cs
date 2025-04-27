using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class TransactionResponseDTO: BaseResponseDTO
    {
        #region Thông tin cơ bản
        public required string TransactionCode { get; set; }
        public required int OrderId { get; set; }
        public required int BuyerId { get; set; }
        public required decimal Amount { get; set; }
        public required string Details { get; set; }
        #endregion
        
        #region Thông tin thanh toán
        public required PaymentMethodEnum PaymentMethod { get; set; }
        public required TransactionStatus Status { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        #endregion
        
        #region Navigation Properties
        public OrderResponseDTO? Order { get; set; }
        public UserResponseDTO? Buyer { get; set; }
        #endregion
    }
} 