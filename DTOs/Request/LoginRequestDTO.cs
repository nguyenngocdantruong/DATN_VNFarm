using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
