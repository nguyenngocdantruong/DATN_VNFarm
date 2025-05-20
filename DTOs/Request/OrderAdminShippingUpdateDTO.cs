using System;
using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class OrderAdminShippingUpdateDTO
    {
        public ShippingUpdateDTO Shipping { get; set; }
        public AddressUpdateDTO Address { get; set; }
    }

    public class ShippingUpdateDTO
    {
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int OrderId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShippingMethod { get; set; }
        public string? ShippingPartner { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

    public class AddressUpdateDTO
    {
        [Required(ErrorMessage = "Tên người nhận là bắt buộc")]
        public string ShippingName { get; set; }
        [Required(ErrorMessage = "Số điện thoại người nhận là bắt buộc")]
        public string ShippingPhone { get; set; }
        [Required(ErrorMessage = "Địa chỉ người nhận là bắt buộc")]
        public string ShippingAddress { get; set; }
        [Required(ErrorMessage = "Tỉnh/Thành phố là bắt buộc")]
        public string ShippingProvince { get; set; }
        [Required(ErrorMessage = "Quận/Huyện là bắt buộc")]
        public string ShippingDistrict { get; set; }
        [Required(ErrorMessage = "Phường/Xã là bắt buộc")]
        public string ShippingWard { get; set; }
    }
} 