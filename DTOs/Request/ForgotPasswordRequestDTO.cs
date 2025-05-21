using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class ForgotPasswordRequestDTO
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";
    }
}
