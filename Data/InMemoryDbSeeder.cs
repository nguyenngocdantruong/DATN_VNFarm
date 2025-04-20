using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;
using VNFarm.Infrastructure.Persistence.Context;

namespace VNFarm.Infrastructure.Data
{
    public static class InMemoryDbSeeder
    {
        public static async Task SeedDatabaseAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<VNFarmContext>>();
            var context = services.GetRequiredService<VNFarmContext>();

            try
            {
                logger.LogInformation("Bắt đầu khởi tạo dữ liệu mẫu cho InMemory Database");
                
                // Thêm dữ liệu mẫu cho từng entity
                await SeedUsersAsync(context);
                await SeedCategoriesAsync(context);
                await SeedStoresAsync(context);
                await SeedProductsAsync(context);
                await SeedOrdersAsync(context);
                await SeedOrderDetailsAsync(context);
                await SeedReviewsAsync(context);
                await SeedTransactionsAsync(context);
                await SeedChatRoomsAsync(context);
                await SeedChatsAsync(context);
                await SeedNotificationsAsync(context);
                await SeedPaymentMethodsAsync(context);
                await SeedBusinessRegistrationsAsync(context);
                
                logger.LogInformation("Đã khởi tạo dữ liệu mẫu thành công");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Lỗi khi khởi tạo dữ liệu mẫu cho InMemory Database");
            }
        }

        private static async Task SeedUsersAsync(VNFarmContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    // Admin users
                    new User { Id = 1, FullName = "Admin User", Email = "admin@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654321", Address = "Hà Nội", ImageUrl = "/images/avatars/admin.png", Role = UserRole.Admin, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 2, FullName = "System Admin", Email = "sysadmin@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654322", Address = "TP.HCM", ImageUrl = "/images/avatars/sysadmin.png", Role = UserRole.Admin, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    
                    // Seller users
                    new User { Id = 3, FullName = "Organic Farm", Email = "organic@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654323", Address = "Đà Lạt", ImageUrl = "/images/avatars/organic.png", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 4, FullName = "Fruit Paradise", Email = "fruit@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654324", Address = "Tiền Giang", ImageUrl = "/images/avatars/fruit.png", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 5, FullName = "Rice King", Email = "rice@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654325", Address = "Cần Thơ", ImageUrl = "/images/avatars/rice.png", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    
                    // Buyer users
                    new User { Id = 6, FullName = "Nguyễn Văn A", Email = "buyer1@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654326", Address = "Hà Nội", ImageUrl = "/images/avatars/buyer1.png", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 7, FullName = "Trần Thị B", Email = "buyer2@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654327", Address = "TP.HCM", ImageUrl = "/images/avatars/buyer2.png", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 8, FullName = "Lê Văn C", Email = "buyer3@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654328", Address = "Đà Nẵng", ImageUrl = "/images/avatars/buyer3.png", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 9, FullName = "Phạm Thị D", Email = "buyer4@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654329", Address = "Cần Thơ", ImageUrl = "/images/avatars/buyer4.png", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 10, FullName = "Hoàng Văn E", Email = "buyer5@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654330", Address = "Hải Phòng", ImageUrl = "/images/avatars/buyer5.png", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 11, FullName = "Vũ Thị F", Email = "buyer6@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654331", Address = "Nha Trang", ImageUrl = "/images/avatars/buyer6.png", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 12, FullName = "Đặng Văn G", Email = "buyer7@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654332", Address = "Vũng Tàu", ImageUrl = "/images/avatars/buyer7.png", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedCategoriesAsync(VNFarmContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Id = 1, Name = "Rau củ", Description = "Các loại rau củ tươi ngon", IconUrl = "/images/categories/vegetables.png", MinPrice = 10000, MaxPrice = 100000, CreatedAt = DateTime.Now },
                    new Category { Id = 2, Name = "Trái cây", Description = "Các loại trái cây tươi ngon", IconUrl = "/images/categories/fruits.png", MinPrice = 15000, MaxPrice = 200000, CreatedAt = DateTime.Now },
                    new Category { Id = 3, Name = "Thực phẩm khô", Description = "Các loại thực phẩm khô", IconUrl = "/images/categories/dry-foods.png", MinPrice = 20000, MaxPrice = 300000, CreatedAt = DateTime.Now },
                    new Category { Id = 4, Name = "Thịt cá", Description = "Các loại thịt cá tươi sống", IconUrl = "/images/categories/meat.png", MinPrice = 50000, MaxPrice = 500000, CreatedAt = DateTime.Now },
                    new Category { Id = 5, Name = "Gia vị", Description = "Các loại gia vị", IconUrl = "/images/categories/spices.png", MinPrice = 5000, MaxPrice = 50000, CreatedAt = DateTime.Now },
                    new Category { Id = 6, Name = "Đồ uống", Description = "Các loại đồ uống", IconUrl = "/images/categories/drinks.png", MinPrice = 10000, MaxPrice = 100000, CreatedAt = DateTime.Now },
                    new Category { Id = 7, Name = "Bánh kẹo", Description = "Các loại bánh kẹo", IconUrl = "/images/categories/sweets.png", MinPrice = 15000, MaxPrice = 150000, CreatedAt = DateTime.Now },
                    new Category { Id = 8, Name = "Đồ hộp", Description = "Các loại đồ hộp", IconUrl = "/images/categories/canned.png", MinPrice = 20000, MaxPrice = 200000, CreatedAt = DateTime.Now },
                    new Category { Id = 9, Name = "Đồ đông lạnh", Description = "Các loại đồ đông lạnh", IconUrl = "/images/categories/frozen.png", MinPrice = 30000, MaxPrice = 300000, CreatedAt = DateTime.Now },
                    new Category { Id = 10, Name = "Đồ khô", Description = "Các loại đồ khô", IconUrl = "/images/categories/dried.png", MinPrice = 25000, MaxPrice = 250000, CreatedAt = DateTime.Now },
                    new Category { Id = 11, Name = "Đồ chay", Description = "Các loại đồ chay", IconUrl = "/images/categories/vegetarian.png", MinPrice = 20000, MaxPrice = 200000, CreatedAt = DateTime.Now },
                    new Category { Id = 12, Name = "Đồ ăn nhanh", Description = "Các loại đồ ăn nhanh", IconUrl = "/images/categories/fast-food.png", MinPrice = 30000, MaxPrice = 300000, CreatedAt = DateTime.Now }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedStoresAsync(VNFarmContext context)
        {
            if (!context.Stores.Any())
            {
                var stores = new List<Store>
                {
                    new Store { Id = 1, Name = "Organic Farm", Description = "Chuyên cung cấp rau củ organic", Address = "Đà Lạt", PhoneNumber = "0987654323", Email = "organic@vnfarm.com", LogoUrl = "/images/stores/organic.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.8m, ReviewCount = 150, UserId = 3, CreatedAt = DateTime.Now },
                    new Store { Id = 2, Name = "Fruit Paradise", Description = "Chuyên cung cấp trái cây tươi", Address = "Tiền Giang", PhoneNumber = "0987654324", Email = "fruit@vnfarm.com", LogoUrl = "/images/stores/fruit.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.7m, ReviewCount = 120, UserId = 4, CreatedAt = DateTime.Now },
                    new Store { Id = 3, Name = "Rice King", Description = "Chuyên cung cấp gạo chất lượng cao", Address = "Cần Thơ", PhoneNumber = "0987654325", Email = "rice@vnfarm.com", LogoUrl = "/images/stores/rice.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.9m, ReviewCount = 200, UserId = 5, CreatedAt = DateTime.Now },
                    new Store { Id = 4, Name = "Meat Master", Description = "Chuyên cung cấp thịt cá tươi sống", Address = "Hà Nội", PhoneNumber = "0987654333", Email = "meat@vnfarm.com", LogoUrl = "/images/stores/meat.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.6m, ReviewCount = 100, UserId = 13, CreatedAt = DateTime.Now },
                    new Store { Id = 5, Name = "Spice World", Description = "Chuyên cung cấp gia vị", Address = "TP.HCM", PhoneNumber = "0987654334", Email = "spice@vnfarm.com", LogoUrl = "/images/stores/spice.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.5m, ReviewCount = 80, UserId = 14, CreatedAt = DateTime.Now },
                    new Store { Id = 6, Name = "Drink Heaven", Description = "Chuyên cung cấp đồ uống", Address = "Đà Nẵng", PhoneNumber = "0987654335", Email = "drink@vnfarm.com", LogoUrl = "/images/stores/drink.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.4m, ReviewCount = 90, UserId = 15, CreatedAt = DateTime.Now },
                    new Store { Id = 7, Name = "Sweet Dreams", Description = "Chuyên cung cấp bánh kẹo", Address = "Hải Phòng", PhoneNumber = "0987654336", Email = "sweet@vnfarm.com", LogoUrl = "/images/stores/sweet.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.7m, ReviewCount = 110, UserId = 16, CreatedAt = DateTime.Now },
                    new Store { Id = 8, Name = "Canned Food", Description = "Chuyên cung cấp đồ hộp", Address = "Nha Trang", PhoneNumber = "0987654337", Email = "canned@vnfarm.com", LogoUrl = "/images/stores/canned.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.3m, ReviewCount = 70, UserId = 17, CreatedAt = DateTime.Now },
                    new Store { Id = 9, Name = "Frozen Food", Description = "Chuyên cung cấp đồ đông lạnh", Address = "Vũng Tàu", PhoneNumber = "0987654338", Email = "frozen@vnfarm.com", LogoUrl = "/images/stores/frozen.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.6m, ReviewCount = 95, UserId = 18, CreatedAt = DateTime.Now },
                    new Store { Id = 10, Name = "Dried Food", Description = "Chuyên cung cấp đồ khô", Address = "Cần Thơ", PhoneNumber = "0987654339", Email = "dried@vnfarm.com", LogoUrl = "/images/stores/dried.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.5m, ReviewCount = 85, UserId = 19, CreatedAt = DateTime.Now },
                    new Store { Id = 11, Name = "Vegetarian Food", Description = "Chuyên cung cấp đồ chay", Address = "TP.HCM", PhoneNumber = "0987654340", Email = "vegetarian@vnfarm.com", LogoUrl = "/images/stores/vegetarian.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.8m, ReviewCount = 130, UserId = 20, CreatedAt = DateTime.Now },
                    new Store { Id = 12, Name = "Fast Food", Description = "Chuyên cung cấp đồ ăn nhanh", Address = "Hà Nội", PhoneNumber = "0987654341", Email = "fastfood@vnfarm.com", LogoUrl = "/images/stores/fast-food.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.4m, ReviewCount = 75, UserId = 21, CreatedAt = DateTime.Now }
                };

                await context.Stores.AddRangeAsync(stores);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedProductsAsync(VNFarmContext context)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    // Rau củ
                    new Product { Id = 1, Name = "Cà chua", Description = "Cà chua organic tươi ngon", Price = 25000, ImageUrl = "/images/products/tomato.png", StockQuantity = 100, Unit = Unit.Kg, SoldQuantity = 50, StoreId = 1, CategoryId = 1, IsActive = true, Origin = "Đà Lạt", AverageRating = 4.5m, TotalSoldQuantity = 50, ReviewCount = 10, CreatedAt = DateTime.Now },
                    new Product { Id = 2, Name = "Rau muống", Description = "Rau muống sạch", Price = 15000, ImageUrl = "/images/products/spinach.png", StockQuantity = 200, Unit = Unit.Kg, SoldQuantity = 100, StoreId = 1, CategoryId = 1, IsActive = true, Origin = "Hà Nội", AverageRating = 4.3m, TotalSoldQuantity = 100, ReviewCount = 15, CreatedAt = DateTime.Now },
                    new Product { Id = 3, Name = "Bắp cải", Description = "Bắp cải tươi", Price = 20000, ImageUrl = "/images/products/cabbage.png", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 75, StoreId = 1, CategoryId = 1, IsActive = true, Origin = "Đà Lạt", AverageRating = 4.4m, TotalSoldQuantity = 75, ReviewCount = 12, CreatedAt = DateTime.Now },
                    
                    // Trái cây
                    new Product { Id = 4, Name = "Táo", Description = "Táo Mỹ nhập khẩu", Price = 80000, ImageUrl = "/images/products/apple.png", StockQuantity = 80, Unit = Unit.Kg, SoldQuantity = 30, StoreId = 2, CategoryId = 2, IsActive = true, Origin = "Mỹ", AverageRating = 4.8m, TotalSoldQuantity = 30, ReviewCount = 8, CreatedAt = DateTime.Now },
                    new Product { Id = 5, Name = "Cam", Description = "Cam sành", Price = 45000, ImageUrl = "/images/products/orange.png", StockQuantity = 120, Unit = Unit.Kg, SoldQuantity = 60, StoreId = 2, CategoryId = 2, IsActive = true, Origin = "Vĩnh Long", AverageRating = 4.6m, TotalSoldQuantity = 60, ReviewCount = 20, CreatedAt = DateTime.Now },
                    new Product { Id = 6, Name = "Xoài", Description = "Xoài cát Hòa Lộc", Price = 55000, ImageUrl = "/images/products/mango.png", StockQuantity = 90, Unit = Unit.Kg, SoldQuantity = 45, StoreId = 2, CategoryId = 2, IsActive = true, Origin = "Tiền Giang", AverageRating = 4.7m, TotalSoldQuantity = 45, ReviewCount = 15, CreatedAt = DateTime.Now },
                    
                    // Thực phẩm khô
                    new Product { Id = 7, Name = "Gạo ST25", Description = "Gạo ST25 ngon nhất thế giới", Price = 35000, ImageUrl = "/images/products/rice.png", StockQuantity = 200, Unit = Unit.Kg, SoldQuantity = 100, StoreId = 3, CategoryId = 3, IsActive = true, Origin = "Sóc Trăng", AverageRating = 5.0m, TotalSoldQuantity = 100, ReviewCount = 25, CreatedAt = DateTime.Now },
                    new Product { Id = 8, Name = "Đậu xanh", Description = "Đậu xanh chất lượng cao", Price = 25000, ImageUrl = "/images/products/green-bean.png", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 75, StoreId = 3, CategoryId = 3, IsActive = true, Origin = "Đồng Tháp", AverageRating = 4.5m, TotalSoldQuantity = 75, ReviewCount = 18, CreatedAt = DateTime.Now },
                    new Product { Id = 9, Name = "Đậu đen", Description = "Đậu đen hạt to", Price = 30000, ImageUrl = "/images/products/black-bean.png", StockQuantity = 180, Unit = Unit.Kg, SoldQuantity = 90, StoreId = 3, CategoryId = 3, IsActive = true, Origin = "An Giang", AverageRating = 4.6m, TotalSoldQuantity = 90, ReviewCount = 22, CreatedAt = DateTime.Now },
                    
                    // Thịt cá
                    new Product { Id = 10, Name = "Thịt heo", Description = "Thịt heo tươi", Price = 120000, ImageUrl = "/images/products/pork.png", StockQuantity = 50, Unit = Unit.Kg, SoldQuantity = 25, StoreId = 4, CategoryId = 4, IsActive = true, Origin = "Đồng Nai", AverageRating = 4.7m, TotalSoldQuantity = 25, ReviewCount = 12, CreatedAt = DateTime.Now },
                    new Product { Id = 11, Name = "Cá basa", Description = "Cá basa tươi", Price = 90000, ImageUrl = "/images/products/basa.png", StockQuantity = 40, Unit = Unit.Kg, SoldQuantity = 20, StoreId = 4, CategoryId = 4, IsActive = true, Origin = "An Giang", AverageRating = 4.8m, TotalSoldQuantity = 20, ReviewCount = 15, CreatedAt = DateTime.Now },
                    new Product { Id = 12, Name = "Tôm sú", Description = "Tôm sú tươi", Price = 250000, ImageUrl = "/images/products/shrimp.png", StockQuantity = 30, Unit = Unit.Kg, SoldQuantity = 15, StoreId = 4, CategoryId = 4, IsActive = true, Origin = "Cà Mau", AverageRating = 4.9m, TotalSoldQuantity = 15, ReviewCount = 20, CreatedAt = DateTime.Now }
                };

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedOrdersAsync(VNFarmContext context)
        {
            if (!context.Orders.Any())
            {
                var orders = new List<Order>
                {
                    new Order { 
                        Id = 1, 
                        OrderCode = "ORD001",
                        BuyerId = 6, 
                        StoreId = 1, 
                        TotalAmount = 150000,
                        ShippingFee = 30000,
                        TaxAmount = 15000,
                        DiscountAmount = 0,
                        FinalAmount = 195000,
                        Status = OrderStatus.Completed,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.PaymentAfter,
                        ShippingName = "Nguyễn Văn A",
                        ShippingPhone = "0987654326",
                        ShippingAddress = "123 Đường ABC",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 1",
                        ShippingWard = "Phường 1",
                        CreatedAt = DateTime.Now.AddDays(-5),
                        PaidAt = DateTime.Now.AddDays(-5)
                    },
                    new Order { 
                        Id = 2, 
                        OrderCode = "ORD002",
                        BuyerId = 7, 
                        StoreId = 2, 
                        TotalAmount = 250000,
                        ShippingFee = 30000,
                        TaxAmount = 25000,
                        DiscountAmount = 0,
                        FinalAmount = 305000,
                        Status = OrderStatus.Pending,
                        PaymentStatus = PaymentStatus.Unpaid,
                        PaymentMethod = PaymentMethodEnum.BankTransfer,
                        ShippingName = "Trần Thị B",
                        ShippingPhone = "0987654327",
                        ShippingAddress = "456 Đường XYZ",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 2",
                        ShippingWard = "Phường 2",
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    new Order { 
                        Id = 3, 
                        OrderCode = "ORD003",
                        BuyerId = 8, 
                        StoreId = 3, 
                        TotalAmount = 350000,
                        ShippingFee = 30000,
                        TaxAmount = 35000,
                        DiscountAmount = 0,
                        FinalAmount = 415000,
                        Status = OrderStatus.Processing,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.VNPay,
                        ShippingName = "Lê Văn C",
                        ShippingPhone = "0987654328",
                        ShippingAddress = "789 Đường DEF",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 3",
                        ShippingWard = "Phường 3",
                        CreatedAt = DateTime.Now.AddDays(-1),
                        PaidAt = DateTime.Now.AddDays(-1)
                    }
                };

                await context.Orders.AddRangeAsync(orders);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedOrderDetailsAsync(VNFarmContext context)
        {
            if (!context.OrderDetails.Any())
            {
                var orderDetails = new List<OrderDetail>
                {
                    new OrderDetail { 
                        Id = 1, 
                        OrderId = 1, 
                        ProductId = 1, 
                        Quantity = 2, 
                        UnitPrice = 75000,
                        Subtotal = 150000,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    new OrderDetail { 
                        Id = 2, 
                        OrderId = 2, 
                        ProductId = 2, 
                        Quantity = 1, 
                        UnitPrice = 250000,
                        Subtotal = 250000,
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    new OrderDetail { 
                        Id = 3, 
                        OrderId = 3, 
                        ProductId = 3, 
                        Quantity = 1, 
                        UnitPrice = 350000,
                        Subtotal = 350000,
                        CreatedAt = DateTime.Now.AddDays(-1)
                    }
                };

                await context.OrderDetails.AddRangeAsync(orderDetails);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedReviewsAsync(VNFarmContext context)
        {
            if (!context.Reviews.Any())
            {
                var reviews = new List<Review>
                {
                    new Review { Id = 1, UserId = 6, ProductId = 1, Rating = 5, Content = "Sản phẩm rất tươi ngon", ImageUrl = "/images/reviews/review1.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-5) },
                    new Review { Id = 2, UserId = 7, ProductId = 2, Rating = 4, Content = "Rau sạch, giá hợp lý", ImageUrl = "/images/reviews/review2.jpg", ShopResponse = "Chúng tôi sẽ cố gắng hơn nữa", CreatedAt = DateTime.Now.AddDays(-4) },
                    new Review { Id = 3, UserId = 8, ProductId = 3, Rating = 5, Content = "Bắp cải tươi, ngon", ImageUrl = "/images/reviews/review3.jpg", ShopResponse = "Cảm ơn bạn", CreatedAt = DateTime.Now.AddDays(-3) },
                    new Review { Id = 4, UserId = 9, ProductId = 4, Rating = 4, Content = "Táo ngon, giòn", ImageUrl = "/images/reviews/review4.jpg", ShopResponse = "Rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-2) },
                    new Review { Id = 5, UserId = 10, ProductId = 5, Rating = 5, Content = "Cam ngọt, nhiều nước", ImageUrl = "/images/reviews/review5.jpg", ShopResponse = "Cảm ơn bạn đã tin tưởng", CreatedAt = DateTime.Now.AddDays(-1) },
                    new Review { Id = 6, UserId = 11, ProductId = 6, Rating = 4, Content = "Xoài chín đều, ngọt", ImageUrl = "/images/reviews/review6.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-6) },
                    new Review { Id = 7, UserId = 12, ProductId = 7, Rating = 5, Content = "Gạo ngon, dẻo", ImageUrl = "/images/reviews/review7.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-7) },
                    new Review { Id = 8, UserId = 6, ProductId = 8, Rating = 4, Content = "Đậu xanh chất lượng", ImageUrl = "/images/reviews/review8.jpg", ShopResponse = "Chúng tôi sẽ tiếp tục cải thiện", CreatedAt = DateTime.Now.AddDays(-8) },
                    new Review { Id = 9, UserId = 7, ProductId = 9, Rating = 5, Content = "Đậu đen hạt to, ngon", ImageUrl = "/images/reviews/review9.jpg", ShopResponse = "Cảm ơn bạn", CreatedAt = DateTime.Now.AddDays(-9) },
                    new Review { Id = 10, UserId = 8, ProductId = 10, Rating = 4, Content = "Thịt heo tươi, ngon", ImageUrl = "/images/reviews/review10.jpg", ShopResponse = "Rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-10) }
                };

                await context.Reviews.AddRangeAsync(reviews);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedTransactionsAsync(VNFarmContext context)
        {
            if (!context.Transactions.Any())
            {
                var transactions = new List<Transaction>
                {
                    new Transaction { Id = 1, OrderId = 1, Amount = 150000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-5) },
                    new Transaction { Id = 2, OrderId = 2, Amount = 250000, Status = TransactionStatus.Pending, PaymentMethod = PaymentMethodEnum.BankTransfer, CreatedAt = DateTime.Now.AddDays(-3) },
                    new Transaction { Id = 3, OrderId = 3, Amount = 350000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.VNPay, CreatedAt = DateTime.Now.AddDays(-1) },
                    new Transaction { Id = 4, OrderId = 4, Amount = 200000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-2) },
                    new Transaction { Id = 5, OrderId = 5, Amount = 180000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.BankTransfer, CreatedAt = DateTime.Now.AddDays(-4) },
                    new Transaction { Id = 6, OrderId = 6, Amount = 300000, Status = TransactionStatus.Cancelled, PaymentMethod = PaymentMethodEnum.VNPay, CreatedAt = DateTime.Now.AddDays(-6) },
                    new Transaction { Id = 7, OrderId = 7, Amount = 220000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-7) },
                    new Transaction { Id = 8, OrderId = 8, Amount = 280000, Status = TransactionStatus.Pending, PaymentMethod = PaymentMethodEnum.BankTransfer, CreatedAt = DateTime.Now.AddDays(-8) },
                    new Transaction { Id = 9, OrderId = 9, Amount = 190000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.VNPay, CreatedAt = DateTime.Now.AddDays(-9) },
                    new Transaction { Id = 10, OrderId = 10, Amount = 240000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-10) }
                };

                await context.Transactions.AddRangeAsync(transactions);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedChatRoomsAsync(VNFarmContext context)
        {
            if (!context.ChatRooms.Any())
            {
                var chatRooms = new List<ChatRoom>
                {
                    new ChatRoom { Id = 1, NameRoom = "Chat Room 1", Description = "Chat về đơn hàng #1", BuyerId = 6, SellerId = 3, OrderId = 1, Type = ChatRoomType.ChatNormal, Status = ChatRoomStatus.InProgress, LastMessage = "Xin chào", LastMessageTime = DateTime.Now.AddDays(-5), IsActive = true },
                    new ChatRoom { Id = 2, NameRoom = "Chat Room 2", Description = "Chat về đơn hàng #2", BuyerId = 7, SellerId = 4, OrderId = 2, Type = ChatRoomType.ChatNormal, Status = ChatRoomStatus.InProgress, LastMessage = "Cảm ơn", LastMessageTime = DateTime.Now.AddDays(-3), IsActive = true },
                    new ChatRoom { Id = 3, NameRoom = "Chat Room 3", Description = "Chat về đơn hàng #3", BuyerId = 8, SellerId = 5, OrderId = 3, Type = ChatRoomType.ChatNormal, Status = ChatRoomStatus.InProgress, LastMessage = "Tạm biệt", LastMessageTime = DateTime.Now.AddDays(-1), IsActive = true },
                    new ChatRoom { Id = 4, NameRoom = "Chat Room 4", Description = "Chat về đơn hàng #4", BuyerId = 9, SellerId = 1, OrderId = 4, Type = ChatRoomType.ChatNormal, Status = ChatRoomStatus.InProgress, LastMessage = "Hẹn gặp lại", LastMessageTime = DateTime.Now.AddDays(-2), IsActive = true },
                    new ChatRoom { Id = 5, NameRoom = "Chat Room 5", Description = "Chat về đơn hàng #5", BuyerId = 10, SellerId = 2, OrderId = 5, Type = ChatRoomType.ChatNormal, Status = ChatRoomStatus.InProgress, LastMessage = "Chào buổi sáng", LastMessageTime = DateTime.Now.AddDays(-4), IsActive = true }
                };

                await context.ChatRooms.AddRangeAsync(chatRooms);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedChatsAsync(VNFarmContext context)
        {
            if (!context.Chats.Any())
            {
                var chats = new List<Chat>
                {
                    new Chat { Id = 1, SenderId = 1, ChatRoomId = 1, Content = "Xin chào", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 2, SenderId = 2, ChatRoomId = 1, Content = "Chào bạn", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 3, SenderId = 1, ChatRoomId = 2, Content = "Cảm ơn", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 4, SenderId = 3, ChatRoomId = 2, Content = "Không có gì", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 5, SenderId = 2, ChatRoomId = 3, Content = "Tạm biệt", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 6, SenderId = 1, ChatRoomId = 3, Content = "Hẹn gặp lại", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 7, SenderId = 3, ChatRoomId = 4, Content = "Chào buổi sáng", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 8, SenderId = 2, ChatRoomId = 4, Content = "Chào buổi sáng", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 9, SenderId = 1, ChatRoomId = 5, Content = "Bạn có khoẻ không?", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 10, SenderId = 3, ChatRoomId = 5, Content = "Mình khoẻ, cảm ơn bạn", ImageUrl = "", Type = ChatMessageType.Text }
                };

                await context.Chats.AddRangeAsync(chats);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedNotificationsAsync(VNFarmContext context)
        {
            if (!context.Notifications.Any())
            {
                var notifications = new List<Notification>
                {
                    new Notification
                    {
                        Id = 1,
                        UserId = 1,
                        Content = "Đơn hàng #1234 của bạn đã được xác nhận",
                        LinkUrl = "/orders/1234",
                        Type = NotificationType.Order,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Notification
                    {
                        Id = 2,
                        UserId = 1,
                        Content = "Thanh toán đơn hàng #1234 thành công",
                        LinkUrl = "/orders/1234",
                        Type = NotificationType.Payment,
                        IsRead = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new Notification
                    {
                        Id = 3,
                        UserId = 2,
                        Content = "Bạn có mã giảm giá mới: SALE20",
                        LinkUrl = "/promotions",
                        Type = NotificationType.Discount,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new Notification
                    {
                        Id = 4,
                        UserId = 2,
                        Content = "Tài khoản của bạn đã được xác minh",
                        LinkUrl = "/profile",
                        Type = NotificationType.Account,
                        IsRead = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-3)
                    },
                    new Notification
                    {
                        Id = 5,
                        UserId = 3,
                        Content = "Cửa hàng của bạn đã được duyệt",
                        LinkUrl = "/store",
                        Type = NotificationType.Store,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-4)
                    },
                    new Notification
                    {
                        Id = 6,
                        UserId = 3,
                        Content = "Sản phẩm của bạn đã hết hàng",
                        LinkUrl = "/products/1",
                        Type = NotificationType.Product,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-5)
                    },
                    new Notification
                    {
                        Id = 7,
                        UserId = 1,
                        Content = "Bạn có thông báo mới từ quản trị viên",
                        LinkUrl = "/admin/notifications",
                        Type = NotificationType.Admin,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-6)
                    },
                    new Notification
                    {
                        Id = 8,
                        UserId = 2,
                        Content = "Hệ thống đang bảo trì vào lúc 2h sáng",
                        LinkUrl = "/maintenance",
                        Type = NotificationType.System,
                        IsRead = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-7)
                    },
                    new Notification
                    {
                        Id = 9,
                        UserId = 3,
                        Content = "Đơn hàng #5678 của bạn đã được giao thành công",
                        LinkUrl = "/orders/5678",
                        Type = NotificationType.Order,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-8)
                    },
                    new Notification
                    {
                        Id = 10,
                        UserId = 1,
                        Content = "Bạn có mã giảm giá sinh nhật",
                        LinkUrl = "/promotions",
                        Type = NotificationType.Discount,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-9)
                    }
                };

                await context.Notifications.AddRangeAsync(notifications);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedPaymentMethodsAsync(VNFarmContext context)
        {
            if (!context.PaymentMethods.Any())
            {
                var paymentMethods = new List<PaymentMethod>
                {
                    new PaymentMethod
                    {
                        Id = 1,
                        CardName = "Vietcombank",
                        PaymentType = PaymentType.Bank,
                        AccountNumber = "1234567890",
                        AccountHolderName = "Nguyen Van A",
                        BankName = "Vietcombank",
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow
                    },
                    new PaymentMethod
                    {
                        Id = 2,
                        CardName = "Techcombank",
                        PaymentType = PaymentType.Bank,
                        AccountNumber = "0987654321",
                        AccountHolderName = "Tran Thi B",
                        BankName = "Techcombank",
                        UserId = 2,
                        CreatedAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new PaymentMethod
                    {
                        Id = 3,
                        CardName = "Momo",
                        PaymentType = PaymentType.Momo,
                        AccountNumber = "0123456789",
                        AccountHolderName = "Le Van C",
                        BankName = "Momo",
                        UserId = 3,
                        CreatedAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new PaymentMethod
                    {
                        Id = 4,
                        CardName = "ZaloPay",
                        PaymentType = PaymentType.ZaloPay,
                        AccountNumber = "9876543210",
                        AccountHolderName = "Pham Thi D",
                        BankName = "ZaloPay",
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow.AddDays(-3)
                    },
                    new PaymentMethod
                    {
                        Id = 5,
                        CardName = "Vietinbank",
                        PaymentType = PaymentType.Bank,
                        AccountNumber = "4567890123",
                        AccountHolderName = "Hoang Van E",
                        BankName = "Vietinbank",
                        UserId = 2,
                        CreatedAt = DateTime.UtcNow.AddDays(-4)
                    },
                    new PaymentMethod
                    {
                        Id = 6,
                        CardName = "BIDV",
                        PaymentType = PaymentType.Bank,
                        AccountNumber = "7890123456",
                        AccountHolderName = "Vu Thi F",
                        BankName = "BIDV",
                        UserId = 3,
                        CreatedAt = DateTime.UtcNow.AddDays(-5)
                    },
                    new PaymentMethod
                    {
                        Id = 7,
                        CardName = "Visa",
                        PaymentType = PaymentType.Visa,
                        AccountNumber = "2345678901",
                        AccountHolderName = "Do Van G",
                        BankName = "Visa",
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow.AddDays(-6)
                    },
                    new PaymentMethod
                    {
                        Id = 8,
                        CardName = "Mastercard",
                        PaymentType = PaymentType.Mastercard,
                        AccountNumber = "5678901234",
                        AccountHolderName = "Nguyen Thi H",
                        BankName = "Mastercard",
                        UserId = 2,
                        CreatedAt = DateTime.UtcNow.AddDays(-7)
                    },
                    new PaymentMethod
                    {
                        Id = 9,
                        CardName = "Sacombank",
                        PaymentType = PaymentType.Bank,
                        AccountNumber = "8901234567",
                        AccountHolderName = "Tran Van I",
                        BankName = "Sacombank",
                        UserId = 3,
                        CreatedAt = DateTime.UtcNow.AddDays(-8)
                    },
                    new PaymentMethod
                    {
                        Id = 10,
                        CardName = "MB Bank",
                        PaymentType = PaymentType.Bank,
                        AccountNumber = "3456789012",
                        AccountHolderName = "Le Thi K",
                        BankName = "MB Bank",
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow.AddDays(-9)
                    }
                };

                await context.PaymentMethods.AddRangeAsync(paymentMethods);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedBusinessRegistrationsAsync(VNFarmContext context)
        {
            if (!context.BusinessRegistrations.Any())
            {
                var businessRegistrations = new List<BusinessRegistration>
                {
                    new BusinessRegistration
                    {
                        Id = 1,
                        BusinessType = StoreType.Farmer,
                        Address = "123 Đường Lê Lợi, Quận 1, TP.HCM",
                        TaxCode = "0123456789",
                        BusinessName = "Nông Trại Xanh",
                        BusinessLicenseUrl = "https://example.com/licenses/1.pdf",
                        Notes = "Nông trại trồng rau sạch",
                        RegistrationStatus = RegistrationStatus.Approved,
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow
                    },
                    new BusinessRegistration
                    {
                        Id = 2,
                        BusinessType = StoreType.Company,
                        Address = "456 Đường Nguyễn Huệ, Quận 1, TP.HCM",
                        TaxCode = "9876543210",
                        BusinessName = "Phân Phối Nông Sản Việt",
                        BusinessLicenseUrl = "https://example.com/licenses/2.pdf",
                        Notes = "Phân phối nông sản toàn quốc",
                        RegistrationStatus = RegistrationStatus.Pending,
                        UserId = 2,
                        CreatedAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new BusinessRegistration
                    {
                        Id = 3,
                        BusinessType = StoreType.Company,
                        Address = "789 Đường Lý Tự Trọng, Quận 1, TP.HCM",
                        TaxCode = "5678901234",
                        BusinessName = "Cửa Hàng Nông Sản Sạch",
                        BusinessLicenseUrl = "https://example.com/licenses/3.pdf",
                        Notes = "Bán lẻ nông sản sạch",
                        RegistrationStatus = RegistrationStatus.Rejected,
                        UserId = 3,
                        CreatedAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new BusinessRegistration
                    {
                        Id = 4,
                        BusinessType = StoreType.Farmer,
                        Address = "321 Đường Võ Văn Tần, Quận 3, TP.HCM",
                        TaxCode = "1357924680",
                        BusinessName = "Trang Trại Hữu Cơ",
                        BusinessLicenseUrl = "https://example.com/licenses/4.pdf",
                        Notes = "Trang trại trồng rau hữu cơ",
                        RegistrationStatus = RegistrationStatus.Pending,
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow.AddDays(-3)
                    },
                    new BusinessRegistration
                    {
                        Id = 5,
                        BusinessType = StoreType.Company,
                        Address = "654 Đường Điện Biên Phủ, Quận 3, TP.HCM",
                        TaxCode = "2468013579",
                        BusinessName = "Thực Phẩm Sạch Việt Nam",
                        BusinessLicenseUrl = "https://example.com/licenses/5.pdf",
                        Notes = "Phân phối thực phẩm sạch",
                        RegistrationStatus = RegistrationStatus.Approved,
                        UserId = 2,
                        CreatedAt = DateTime.UtcNow.AddDays(-4)
                    },
                    new BusinessRegistration
                    {
                        Id = 6,
                        BusinessType = StoreType.Company,
                        Address = "987 Đường Nam Kỳ Khởi Nghĩa, Quận 3, TP.HCM",
                        TaxCode = "3692581470",
                        BusinessName = "Siêu Thị Nông Sản",
                        BusinessLicenseUrl = "https://example.com/licenses/6.pdf",
                        Notes = "Siêu thị chuyên bán nông sản",
                        RegistrationStatus = RegistrationStatus.Pending,
                        UserId = 3,
                        CreatedAt = DateTime.UtcNow.AddDays(-5)
                    },
                    new BusinessRegistration
                    {
                        Id = 7,
                        BusinessType = StoreType.Farmer,
                        Address = "147 Đường Cách Mạng Tháng 8, Quận 3, TP.HCM",
                        TaxCode = "7418529630",
                        BusinessName = "Nông Trại Sinh Thái",
                        BusinessLicenseUrl = "https://example.com/licenses/7.pdf",
                        Notes = "Nông trại trồng rau sinh thái",
                        RegistrationStatus = RegistrationStatus.Approved,
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow.AddDays(-6)
                    },
                    new BusinessRegistration
                    {
                        Id = 8,
                        BusinessType = StoreType.Company,
                        Address = "258 Đường Hai Bà Trưng, Quận 1, TP.HCM",
                        TaxCode = "8529637410",
                        BusinessName = "Phân Phối Rau Củ Sạch",
                        BusinessLicenseUrl = "https://example.com/licenses/8.pdf",
                        Notes = "Phân phối rau củ sạch",
                        RegistrationStatus = RegistrationStatus.Rejected,
                        UserId = 2,
                        CreatedAt = DateTime.UtcNow.AddDays(-7)
                    },
                    new BusinessRegistration
                    {
                        Id = 9,
                        BusinessType = StoreType.Company,
                        Address = "369 Đường Trần Hưng Đạo, Quận 1, TP.HCM",
                        TaxCode = "9630741852",
                        BusinessName = "Cửa Hàng Thực Phẩm Hữu Cơ",
                        BusinessLicenseUrl = "https://example.com/licenses/9.pdf",
                        Notes = "Bán lẻ thực phẩm hữu cơ",
                        RegistrationStatus = RegistrationStatus.Pending,
                        UserId = 3,
                        CreatedAt = DateTime.UtcNow.AddDays(-8)
                    },
                    new BusinessRegistration
                    {
                        Id = 10,
                        BusinessType = StoreType.Farmer,
                        Address = "741 Đường Phạm Ngũ Lão, Quận 1, TP.HCM",
                        TaxCode = "0741852963",
                        BusinessName = "Trang Trại Rau Sạch",
                        BusinessLicenseUrl = "https://example.com/licenses/10.pdf",
                        Notes = "Trang trại trồng rau sạch",
                        RegistrationStatus = RegistrationStatus.Approved,
                        UserId = 1,
                        CreatedAt = DateTime.UtcNow.AddDays(-9)
                    }
                };

                await context.BusinessRegistrations.AddRangeAsync(businessRegistrations);
                await context.SaveChangesAsync();
            }
        }
    }
} 