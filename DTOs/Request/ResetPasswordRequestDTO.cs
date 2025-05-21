using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Request
{
    public class ResetPasswordRequestDTO
    {
        [Required(ErrorMessage = "OTP không được để trống")]
        public int OTP { get; set; } = -1;
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string NewPassword { get; set; } = "";
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";
    }
}
