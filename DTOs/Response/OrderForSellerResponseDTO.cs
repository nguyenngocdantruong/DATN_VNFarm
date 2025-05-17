
using VNFarm.Enums;

namespace VNFarm.DTOs.Response
{
    public class OrderForSellerResponseDTO : BaseResponseDTO
    {
         #region Thông tin chung
        public required string OrderCode { get; set; }
        public required OrderStatus Status { get; set; }
        public required string Notes { get; set; }
        public required AddressResponseDTO Address { get; set; }        
        public required ShippingResponseDTO Shipping { get; set; }
        #endregion
        
        #region Thông tin thanh toán
        public required PaymentStatus PaymentStatus { get; set; }
        #endregion
        
        #region Foreign keys
        public int BuyerId { get; set; }
        public int? StoreId { get; set; }
        #endregion
        
        #region Related Properties
        public List<OrderItemResponseDTO> OrderItems { get; set; } = [];
        public List<OrderTimelineResponseDTO?> OrderTimelines { get; set; } = [];
        #endregion

    }
}