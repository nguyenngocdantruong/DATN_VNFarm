
namespace VNFarm.DTOs.Response
{
    public class CategoryResponseDTO : BaseResponseDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string IconUrl { get; set; }
        public required decimal MinPrice { get; set; }
        public required decimal MaxPrice { get; set; }
        public required int ProductCount { get; set; }
        public ICollection<ProductResponseDTO> Products { get; set; } = [];
    }
}