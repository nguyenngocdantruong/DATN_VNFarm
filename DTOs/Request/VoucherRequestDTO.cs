using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class VoucherRequestDTO
    {
        [Required(ErrorMessage = "Mã voucher là bắt buộc")]
        public string Voucher { get; set; } = "";
        
        [Required(ErrorMessage = "Thông tin giỏ hàng là bắt buộc")]
        public CartRequestDTO Cart { get; set; } = new CartRequestDTO();
    }
} 