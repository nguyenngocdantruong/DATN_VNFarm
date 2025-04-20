using System.ComponentModel.DataAnnotations;

namespace VNFarm_FinalFinal.DTOs.Response
{
    public class ShippingResponseDTO : BaseResponseDTO
    {
        public required int OrderId { get; set; }
        #region Thông tin vận chuyển
        public required string TrackingNumber { get; set; }
        public required string ShippingMethod { get; set; }
        public required string ShippingPartner { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        #endregion
    }
}
