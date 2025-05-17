using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class ShopCartRequestDTO : BaseRequestDTO
    {
        [Required]
        public int ShopId { get; set; }
        
        [Required]
        public int CartId { get; set; }
        
        public List<CartItemRequestDTO>? CartItems { get; set; }
    }
} 