# VNFarm - Đồ án tốt nghiệp đề tài "Nông sản Việt Nam"

## Giới thiệu 

VNFarm là nền tảng thương mại điện tử chuyên về nông sản Việt Nam, kết nối người nông dân trực tiếp với người tiêu dùng. Dự án được xây dựng bằng ASP.NET Core, cung cấp API cho ứng dụng web và di động, với mục tiêu tạo ra một hệ sinh thái mua bán nông sản minh bạch, an toàn và hiệu quả.

## Tính năng chính 

- **Quản lý sản phẩm**: Đăng bán, tìm kiếm, phân loại sản phẩm nông nghiệp
- **Quản lý cửa hàng**: Hệ thống xác thực và quản lý cửa hàng cho người bán
- **Giỏ hàng & Thanh toán**: Tích hợp VNPay và các phương thức thanh toán khác
- **Đơn hàng & Vận chuyển**: Theo dõi trạng thái đơn hàng và quản lý vận chuyển
- **Đánh giá & Bình luận**: Hệ thống đánh giá sản phẩm và người bán
- **Chat trực tuyến**: Kênh liên lạc giữa người mua và người bán
- **Thông báo**: Hệ thống thông báo qua email và push notification
- **Mã giảm giá**: Quản lý và áp dụng các chương trình khuyến mãi

## Công nghệ sử dụng 

- **Backend**: ASP.NET Core 9.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Token
- **Realtime Communication**: Pusher (third-party service)
- **Payment Gateway**: VNPay (third-party service)
- **Email Service**: SMTP (third-party service)
- **Logging**: Serilog
- **Documentation**: Swagger
- **Deployment**: Local

## Cấu trúc dự án
```
VNFarm/
├── Controllers/ # API Controllers
├── Data/ # Database context và migrations
├── DTOs/ # Data Transfer Objects
├── Entities/ # Domain models
├── Enums/ # Enumerations
├── Helpers/ # Utility classes
├── Mappers/ # Object mappers
├── Middlewares/ # Custom middlewares
├── Repositories/ # Data access layer
├── Services/ # Business logic layer
│ ├── External/ # External services (Email, Payment)
│ └── Interfaces/ # Service interfaces
├── wwwroot/ # Static files
└── Program.cs # Application entry point
```

## Cài đặt và chạy dự án 🔧

### Yêu cầu hệ thống

- .NET 9.0 SDK
- SQL Server 2022 (hoặc bất kỳ cơ sở dữ liệu nào hỗ trợ EF Core)
- Visual Studio 2022 hoặc bất kỳ IDE nào hỗ trợ .NET

### Các bước cài đặt

1. Clone repository:
```bash
git clone https://github.com/nguyenngocdantruong/DATN_VNFarm.git
cd DATN_VNFarm
```

2. Khôi phục các packages:
```bash
dotnet restore
```

3. Đổi tên `appsettings.template` thành `appsettings.json` và cấu hình các thông tin sau trong `appsettings.json`:
```json
"ConnectionStrings": {
  "SqlServer": "Server=YourServerName;Database=YourDatabaseName;Trusted_Connection=True;TrustServerCertificate=True;"
},
"Jwt": {
  "Key": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "Issuer": "VNFarm",
  "Audience": "VNFarmUsers"
},
"Vnpay": {
  "TmnCode": "XXXXXXXX",
  "HashSecret": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "BaseUrl": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
  "ReturnUrl": "http://localhost:5172/api/Payment/Callback"
},
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SmtpUsername": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "SmtpPassword": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "SenderEmail": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "SenderName": "VNFarm"
},
"Pusher": {
  "AppId": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "Key": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "Secret": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
  "Cluster": "ap1"
}
```

Có thể tham khảo schema của database tại file `data.sql`

4. Chạy ứng dụng:
```bash
dotnet run
```

Ứng dụng sẽ chạy tại `http://localhost:5172` và Swagger UI có thể truy cập tại `http://localhost:5172/swagger/index.html`.

## Đóng góp

Tôi rất hoan nghênh mọi đóng góp cho dự án VNFarm! Vui lòng làm theo các bước sau:

1. Fork repository
2. Tạo branch mới (`git checkout -b feature/amazing-feature`)
3. Commit thay đổi (`git commit -m 'Add some amazing feature'`)
4. Push lên branch (`git push origin feature/amazing-feature`)
5. Tạo Pull Request


## Liên hệ 📧

- **Email**: dantruong2023@gmail.com
- **GitHub**: https://github.com/nguyenngocdantruong

---
