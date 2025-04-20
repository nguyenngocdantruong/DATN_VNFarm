
namespace VNFarm_FinalFinal.DTOs.Response
{
    public class AddressResponseDTO : BaseResponseDTO
    {
        public required int OrderId { get; set; }
        #region Thông tin địa chỉ giao hàng
        public required string ShippingName { get; set; }
        public required string ShippingPhone { get; set; }
        public required string ShippingAddress { get; set; }
        public required string ShippingProvince { get; set; }
        public required string ShippingDistrict { get; set; }
        public required string ShippingWard { get; set; }
        #endregion
    }
}
