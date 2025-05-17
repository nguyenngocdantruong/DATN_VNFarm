namespace VNFarm.DTOs.Response
{
    public class CartResponseDTO : BaseResponseDTO
    {
        public required int UserId { get; set; }
        public UserResponseDTO? User { get; set; }
        public ICollection<ShopCartResponseDTO>? ShopCarts { get; set; }
    }
} 