using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Helpers
{
    public static class BusinessRegisterUtils
    {
        public static string GetEnumLabel(ApprovalResult approvalResult)
        {
            return approvalResult switch
            {
                ApprovalResult.Approved => "Đã duyệt",
                ApprovalResult.Rejected => "Đã từ chối",
                ApprovalResult.Warning => "Yêu cầu bổ sung / cập nhật",
                ApprovalResult.Pending => "Đang chờ duyệt",
                _ => "Không xác định"
            };
        }
        public static Dictionary<int, string> GetEnumValues()
        {
            return Enum.GetValues(typeof(ApprovalResult))
                .Cast<ApprovalResult>()
                .ToDictionary(x => (int)x, x => GetEnumLabel(x));
        }
    }
}