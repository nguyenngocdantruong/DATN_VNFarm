using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class CheckoutUrlRequest
    {
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public string? OrderId { get; set; }
    }
}
