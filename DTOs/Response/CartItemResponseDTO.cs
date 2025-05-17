namespace VNFarm.DTOs.Response
{
    public class CartItemResponseDTO : BaseResponseDTO
    {
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required int ShopCartId { get; set; }
        public ProductResponseDTO? Product { get; set; }
    }
} 