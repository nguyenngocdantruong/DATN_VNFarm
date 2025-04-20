using VNFarm_FinalFinal.Enums;
namespace VNFarm_FinalFinal.DTOs.Response
{
    public class PaymentMethodResponseDTO : BaseResponseDTO
    {
        #region Thông tin cơ bản
        public required string CardName { get; set; }
        public required PaymentType PaymentType { get; set; }
        public required string AccountNumber { get; set; }
        public required string AccountHolderName { get; set; }
        public required string BankName { get; set; }
        public UserResponseDTO? User { get; set; }
        #endregion
    }
} 