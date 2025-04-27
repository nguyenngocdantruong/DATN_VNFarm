using System;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity kết quả duyệt đăng ký
    // Quản lý thông tin kết quả duyệt đăng ký kinh doanh
    public class RegistrationApprovalResult : BaseEntity
    {
        // ID đăng ký kinh doanh
        public int RegistrationId { get; set; }
        
        // ID admin duyệt
        public int AdminId { get; set; }
        
        // Kết quả duyệt (Đồng ý/Từ chối/Yêu cầu bổ sung/cập nhật/Đang chờ duyệt)
        public ApprovalResult ApprovalResult { get; set; }
        
        // Ghi chú
        public string Note { get; set; } = string.Empty;
        
        // Navigation properties - Các thuộc tính liên kết
        public User? Admin { get; set; }                        // Thông tin admin
    }
} 