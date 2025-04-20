using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Request
{
    public class OrderTimelineRequestDTO: BaseRequestDTO
    {
        #region Thông tin cơ bản
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int OrderId { get; set; }
        [EnumDataType(typeof(OrderEventType))]
        public OrderEventType EventType { get; set; } = OrderEventType.OrderCreated;
        [EnumDataType(typeof(OrderTimelineStatus))]
        public OrderTimelineStatus Status { get; set; } = OrderTimelineStatus.Pending;
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; } = "";
        #endregion
    }
}
