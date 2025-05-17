namespace VNFarm.DTOs.Response
{
    public class ShopCartResponseDTO : BaseResponseDTO
    {
        public required int ShopId { get; set; }
        public required int CartId { get; set; }
        public StoreResponseDTO? Shop { get; set; }
        public ICollection<CartItemResponseDTO>? CartItems { get; set; }
    }
} 