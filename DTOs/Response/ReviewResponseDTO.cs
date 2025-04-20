
namespace VNFarm_FinalFinal.DTOs.Response
{
    public class ReviewResponseDTO : BaseResponseDTO
    {
        public required int UserId { get; set; }
        public required int ProductId { get; set; }
        public required int Rating { get; set; }
        public string? Content { get; set; }
        public string? ShopResponse { get; set; }
        public string? ImageUrl { get; set; }
        public UserResponseDTO? User { get; set; }
        public ProductResponseDTO? Product { get; set; }
    }
}