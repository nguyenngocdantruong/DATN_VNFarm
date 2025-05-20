using VNFarm.DTOs.Request;
using System.ComponentModel.DataAnnotations.Schema;
using VNFarm.DTOs.Response;
namespace VNFarm.Entities
{
    public class CartItem : BaseEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ShopCartId { get; set; }
        public Product? Product { get; set; }
        [NotMapped]
        public ProductResponseDTO? ProductResponse { get; set; }
    }
}
