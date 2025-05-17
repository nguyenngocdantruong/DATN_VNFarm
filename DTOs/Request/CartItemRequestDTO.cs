using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class CartItemRequestDTO : BaseRequestDTO
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
        
        public int ShopCartId { get; set; }
    }
} 