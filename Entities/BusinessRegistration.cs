using System;
using System.Collections.Generic;
using VNFarm.Helpers;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity đăng ký kinh doanh
    // Quản lý thông tin đăng ký kinh doanh của người dùng
    public class BusinessRegistration : BaseEntity
    {
        // Loại hình kinh doanh
        public StoreType BusinessType { get; set; } = StoreType.Farmer;
        // Địa chỉ
        public string Address { get; set; } = string.Empty;
        // Mã số thuế
        public string TaxCode { get; set; } = string.Empty;
        // Tên cửa hàng
        public string BusinessName { get; set; } = string.Empty;
        // URL giấy phép kinh doanh
        public string BusinessLicenseUrl { get; set; } = string.Empty;
        // Ghi chú
        public string Notes { get; set; } = string.Empty;
        // Trạng thái phê duyệt
        public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Pending;
        // Foreign key
        public int UserId { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<RegistrationApprovalResult>? ApprovalResults { get; set; }
    }
} 