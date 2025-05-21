using System;
using System.Collections.Generic;
using VNFarm.Enums;

namespace VNFarm.Entities
{
    // Entity người dùng
    // Quản lý thông tin tài khoản người dùng trong hệ thống
    public class User : BaseEntity
    {
        // Họ và tên người dùng
        public string FullName { get; set; } = "";
        
        // Email đăng nhập
        public string Email { get; set; } = "";
        
        // Mật khẩu đã được mã hóa
        public string PasswordHash { get; set; } = "";
        
        // Số điện thoại liên hệ
        public string PhoneNumber { get; set; } = "";
        
        // Địa chỉ cư trú
        public string Address { get; set; } = "";
        
        // URL ảnh đại diện
        public string ImageUrl { get; set; } = "";
        
        // Vai trò người dùng (Admin, User, Seller)
        public UserRole Role { get; set; } = UserRole.User;
        
        // Trạng thái hoạt động của tài khoản
        public bool IsActive { get; set; } = true;
        
        // Trạng thái xác thực email
        public bool EmailVerified { get; set; } = false;

        // Cài đặt thông báo
        public bool EmailNotificationsEnabled { get; set; } = true;        // Thông báo qua email
        public bool OrderStatusNotificationsEnabled { get; set; } = true;  // Thông báo trạng thái đơn hàng
        public bool DiscountNotificationsEnabled { get; set; } = true;     // Thông báo giảm giá
        public bool AdminNotificationsEnabled { get; set; } = true;        // Thông báo từ admin

        // Navigation properties - Các thuộc tính liên kết
        public Store? Store { get; set; }                                    // Cửa hàng (nếu là người bán)
        public ICollection<Order>? Orders { get; set; }                     // Danh sách đơn hàng
        public ICollection<Review>? Reviews { get; set; }                   // Danh sách đánh giá
        public ICollection<Chat>? SentMessages { get; set; }               // Tin nhắn đã gửi
        public ICollection<Chat>? ReceivedMessages { get; set; }           // Tin nhắn đã nhận
        public ICollection<Notification>? Notifications { get; set; }       // Thông báo
        public Cart? Cart { get; set; }
    }
} 