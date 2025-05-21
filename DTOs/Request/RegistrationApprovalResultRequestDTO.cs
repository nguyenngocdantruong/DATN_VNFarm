using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using VNFarm.Enums;

namespace VNFarm.DTOs.Request
{
    [Obsolete("Don't use this class anymore")]
    public class RegistrationApprovalResultRequestDTO: BaseRequestDTO
    {
        [Required(ErrorMessage = "Mã đăng ký là bắt buộc")]
        public int RegistrationId { get; set; }
        [Required(ErrorMessage = "Kết quả phê duyệt là bắt buộc")]
        [EnumDataType(typeof(ApprovalResult))]
        public ApprovalResult ApprovalResult { get; set; }
        [Required(ErrorMessage = "Ghi chú là bắt buộc")]
        public string Note { get; set; } = "";
    }
}
