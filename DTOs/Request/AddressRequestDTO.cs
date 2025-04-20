using System.ComponentModel.DataAnnotations;

namespace VNFarm_FinalFinal.DTOs.Request
{
    public class AddressRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int OrderId { get; set; }
        #region Thông tin địa chỉ giao hàng
        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        [StringLength(100, ErrorMessage = "Tên người nhận không được vượt quá 100 ký tự")]
        public string ShippingName { get; set; } = "";
        
        [Required(ErrorMessage = "Số điện thoại người nhận không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string ShippingPhone { get; set; } = "";
        
        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        [StringLength(255, ErrorMessage = "Địa chỉ giao hàng không được vượt quá 255 ký tự")]
        public string ShippingAddress { get; set; } = "";
        
        [Required(ErrorMessage = "Tỉnh/Thành phố không được để trống")]
        [StringLength(100, ErrorMessage = "Tỉnh/Thành phố không được vượt quá 100 ký tự")]
        public string ShippingProvince { get; set; } = "";
        
        [Required(ErrorMessage = "Quận/Huyện không được để trống")]
        [Display(Name = "Quận/Huyện")]
        [StringLength(100, ErrorMessage = "Quận/Huyện không được vượt quá 100 ký tự")]
        public string ShippingDistrict { get; set; } = "";
        
        [Required(ErrorMessage = "Phường/Xã không được để trống")]
        [Display(Name = "Phường/Xã")]
        [StringLength(100, ErrorMessage = "Phường/Xã không được vượt quá 100 ký tự")]
        public string ShippingWard { get; set; } = "";
        #endregion
    }
}
