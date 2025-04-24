using System.Transactions;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Response
{
    public class UserResponseDTO : BaseResponseDTO
    {   
        #region Thông tin cơ bản
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        #endregion
        
        #region Thông tin địa chỉ
        public required string Address { get; set; }
        
        #endregion
        
        #region Thông tin tài khoản
        public required string ImageUrl { get; set; }
        public required UserRole Role { get; set; }
        public required bool IsActive { get; set; }
        public required bool EmailVerified { get; set; }
        #endregion
        
        #region Cài đặt thông báo
        public required bool EmailNotificationsEnabled { get; set; }
        public required bool OrderStatusNotificationsEnabled { get; set; }
        public required bool DiscountNotificationsEnabled { get; set; }
        public required bool AdminNotificationsEnabled { get; set; }
        #endregion
        #region Navigation Properties
        public StoreResponseDTO? Store { get; set; }   
        public IEnumerable<PaymentMethodResponseDTO>? PaymentMethods { get; set; }  
        public IEnumerable<TransactionResponseDTO>? Transactions { get; set; }
        #endregion
    }
}