using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Response
{
    public class StoreResponseDTO : BaseResponseDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string LogoUrl { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required StoreType BusinessType { get; set; }
        public required StoreStatus VerificationStatus { get; set; }
        public required bool IsActive { get; set; }
        public required decimal AverageRating { get; set; }
        public required int ReviewCount { get; set; }
        
        #region User Information
        public required int OwnerId { get; set; }
        #endregion
        
        #region Navigation Properties
        public UserResponseDTO? Owner { get; set; }
        public ICollection<ProductResponseDTO>? Products { get; set; }
        public ICollection<OrderResponseDTO>? Orders { get; set; }
        public ICollection<DiscountResponseDTO>? Discounts { get; set; }
        #endregion
    }
}