using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class ShippingRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int OrderId { get; set; }
        #region Thông tin vận chuyển
        [StringLength(50, ErrorMessage = "Mã vận đơn không được vượt quá 50 ký tự")]
        public string TrackingNumber { get; set; } = "";
        
        [StringLength(100, ErrorMessage = "Phương thức vận chuyển không được vượt quá 100 ký tự")]
        public string ShippingMethod { get; set; } = "";
        
        [StringLength(100, ErrorMessage = "Đơn vị vận chuyển không được vượt quá 100 ký tự")]
        public string ShippingPartner { get; set; } = "";
        
        [DataType(DataType.DateTime)]
        public DateTime? ShippedAt { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? DeliveredAt { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? CancelledAt { get; set; }
        #endregion
    }
}
