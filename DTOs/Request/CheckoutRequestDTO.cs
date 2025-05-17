using System.ComponentModel.DataAnnotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    public class CheckoutRequestDTO : BaseRequestDTO
    {
        // Thông tin giỏ hàng - Danh sách ID của các CartItem được chọn
        public required List<int> CartItemIds { get; set; }
        
        // Thông tin địa chỉ giao hàng
        public required AddressRequestDTO Address { get; set; }
        
        // Thông tin thanh toán
        [EnumDataType(typeof(PaymentMethodEnum))]
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.BankTransfer;
        
        // Thông tin khác
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string Notes { get; set; } = "";
        
        // Mã giảm giá (nếu có)
        public string? DiscountCode { get; set; }
    }
} 