using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class CartRequestDTO : BaseRequestDTO
    {
        [Required]
        public int UserId { get; set; }
        
        public List<ShopCartRequestDTO>? ShopCarts { get; set; }
    }
} 