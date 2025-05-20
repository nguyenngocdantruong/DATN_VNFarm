using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;

namespace VNFarm.Data.Data
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
                await SeedOrderTimelinesAsync(context);
                await SeedDiscountsAsync(context);


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
                    // Admin user
                    new User { Id = 1, FullName = "Admin User", Email = "admin@vnfarm.com", PasswordHash = "ICy5YqxZB1uWSwcVLSNLcA==", PhoneNumber = "0987654321", Address = "Hà Nội", ImageUrl = "avatar-1.jpg", Role = UserRole.Admin, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    
                    // Seller user
                    new User { Id = 2, FullName = "Fruit Paradise", Email = "fruit@vnfarm.com", PasswordHash = "ICy5YqxZB1uWSwcVLSNLcA==", PhoneNumber = "0987654324", Address = "Tiền Giang", ImageUrl = "avatar-4.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    
                    // Buyer user
                    new User { Id = 3, FullName = "Nguyễn Văn A", Email = "buyer1@vnfarm.com", PasswordHash = "ICy5YqxZB1uWSwcVLSNLcA==", PhoneNumber = "0987654326", Address = "Hà Nội", ImageUrl = "avatar-6.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now }
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
                    new Category { Id = 1, Name = "Rau củ", Description = "Các loại rau củ tươi ngon", IconUrl = "category-1.jpg", MinPrice = 10000, MaxPrice = 100000, CreatedAt = DateTime.Now },
                    new Category { Id = 2, Name = "Trái cây", Description = "Các loại trái cây tươi ngon", IconUrl = "category-2.jpg", MinPrice = 15000, MaxPrice = 200000, CreatedAt = DateTime.Now },
                    new Category { Id = 3, Name = "Thực phẩm khô", Description = "Các loại thực phẩm khô", IconUrl = "category-3.jpg", MinPrice = 20000, MaxPrice = 300000, CreatedAt = DateTime.Now },
                    new Category { Id = 4, Name = "Thịt cá", Description = "Các loại thịt cá tươi sống", IconUrl = "category-4.jpg", MinPrice = 50000, MaxPrice = 500000, CreatedAt = DateTime.Now },
                    new Category { Id = 5, Name = "Gia vị", Description = "Các loại gia vị", IconUrl = "category-5.jpg", MinPrice = 5000, MaxPrice = 50000, CreatedAt = DateTime.Now },
                    new Category { Id = 6, Name = "Đồ uống", Description = "Các loại đồ uống", IconUrl = "category-6.jpg", MinPrice = 10000, MaxPrice = 100000, CreatedAt = DateTime.Now },
                    new Category { Id = 7, Name = "Bánh kẹo", Description = "Các loại bánh kẹo", IconUrl = "category-7.jpg", MinPrice = 15000, MaxPrice = 150000, CreatedAt = DateTime.Now },
                    new Category { Id = 8, Name = "Đồ hộp", Description = "Các loại đồ hộp", IconUrl = "category-8.jpg", MinPrice = 20000, MaxPrice = 200000, CreatedAt = DateTime.Now },
                    new Category { Id = 9, Name = "Đồ đông lạnh", Description = "Các loại đồ đông lạnh", IconUrl = "category-9.jpg", MinPrice = 30000, MaxPrice = 300000, CreatedAt = DateTime.Now },
                    new Category { Id = 10, Name = "Đồ khô", Description = "Các loại đồ khô", IconUrl = "category-10.jpg", MinPrice = 25000, MaxPrice = 250000, CreatedAt = DateTime.Now },
                    new Category { Id = 11, Name = "Đồ chay", Description = "Các loại đồ chay", IconUrl = "category-11.jpg", MinPrice = 20000, MaxPrice = 200000, CreatedAt = DateTime.Now },
                    new Category { Id = 12, Name = "Đồ ăn nhanh", Description = "Các loại đồ ăn nhanh", IconUrl = "category-12.jpg", MinPrice = 30000, MaxPrice = 300000, CreatedAt = DateTime.Now }
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
                    new Store { Id = 1, Name = "Fruit Paradise", Description = "Chuyên cung cấp trái cây tươi", Address = "Tiền Giang", PhoneNumber = "0987654324", Email = "fruit@vnfarm.com", LogoUrl = "/images/stores/fruit.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.7m, ReviewCount = 120, UserId = 2, CreatedAt = DateTime.Now },
                    new Store { Id = 2, Name = "Admin Store", Description = "Cửa hàng quản trị viên", Address = "Hà Nội", PhoneNumber = "0987654321", Email = "admin@vnfarm.com", LogoUrl = "/images/stores/admin.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.8m, ReviewCount = 150, UserId = 1, CreatedAt = DateTime.Now }
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
                    // Sản phẩm của cửa hàng Fruit Paradise (StoreId = 1, UserId = 2)
                    new Product { Id = 1, Name = "Việt quất", Description = "Việt quất tươi ngon, giàu chất chống oxy hóa", Price = 25000, ImageUrl = "products1-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 50, StoreId = 1, CategoryId = 2, IsActive = false, Origin = "Đà Lạt", AverageRating = 4.5m, TotalSoldQuantity = 50, ReviewCount = 10, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 3, ReviewStar5Count = 6, CreatedAt = DateTime.Now },
                    new Product { Id = 2, Name = "Sữa tươi", Description = "Sữa tươi thanh trùng nguyên chất", Price = 15000, ImageUrl = "products2-min.jpg", StockQuantity = 200, Unit = Unit.Bag, SoldQuantity = 100, StoreId = 1, CategoryId = 6, IsActive = true, Origin = "Việt Nam", AverageRating = 4.3m, TotalSoldQuantity = 100, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 2, ReviewStar4Count = 7, ReviewStar5Count = 5, CreatedAt = DateTime.Now },
                    new Product { Id = 3, Name = "Sữa hạnh nhân", Description = "Sữa hạnh nhân hữu cơ, không đường", Price = 20000, ImageUrl = "products3-min.jpg", StockQuantity = 150, Unit = Unit.Bag, SoldQuantity = 75, StoreId = 1, CategoryId = 6, IsActive = true, Origin = "Mỹ", AverageRating = 4.4m, TotalSoldQuantity = 75, ReviewCount = 12, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 2, ReviewStar4Count = 5, ReviewStar5Count = 5, CreatedAt = DateTime.Now },
                    
                    // Sản phẩm của cửa hàng Admin Store (StoreId = 2, UserId = 1)
                    new Product { Id = 4, Name = "Súp lơ", Description = "Súp lơ xanh tươi ngon", Price = 80000, ImageUrl = "products4-min.jpg", StockQuantity = 80, Unit = Unit.Kg, SoldQuantity = 30, StoreId = 2, CategoryId = 1, IsActive = false, Origin = "Đà Lạt", AverageRating = 4.8m, TotalSoldQuantity = 30, ReviewCount = 8, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 6, CreatedAt = DateTime.Now },
                    new Product { Id = 5, Name = "Súp lơ", Description = "Súp lơ tươi hữu cơ", Price = 45000, ImageUrl = "products5-min.jpg", StockQuantity = 120, Unit = Unit.Kg, SoldQuantity = 60, StoreId = 2, CategoryId = 1, IsActive = true, Origin = "Đà Lạt", AverageRating = 4.6m, TotalSoldQuantity = 60, ReviewCount = 20, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 5, ReviewStar5Count = 13, CreatedAt = DateTime.Now },
                    
                    // Thêm các sản phẩm của Fruit Paradise
                    new Product { Id = 6, Name = "Thanh long", Description = "Thanh long ruột đỏ Bình Thuận", Price = 90000, ImageUrl = "products14-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 55, StoreId = 1, CategoryId = 2, IsActive = true, Origin = "Bình Thuận", AverageRating = 4.9m, TotalSoldQuantity = 55, ReviewCount = 22, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 20, CreatedAt = DateTime.Now },
                    new Product { Id = 7, Name = "Nho", Description = "Nho xanh không hạt", Price = 35000, ImageUrl = "products7-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 100, StoreId = 1, CategoryId = 2, IsActive = false, Origin = "Ninh Thuận", AverageRating = 5.0m, TotalSoldQuantity = 100, ReviewCount = 25, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 0, ReviewStar5Count = 25, CreatedAt = DateTime.Now },
                    new Product { Id = 8, Name = "Vải thiều", Description = "Vải thiều tươi ngon từ Bắc Giang", Price = 25000, ImageUrl = "products8-min.jpg", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 75, StoreId = 1, CategoryId = 2, IsActive = true, Origin = "Bắc Giang", AverageRating = 4.5m, TotalSoldQuantity = 75, ReviewCount = 18, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 7, ReviewStar5Count = 9, CreatedAt = DateTime.Now },
                    
                    // Thêm các sản phẩm của Admin Store
                    new Product { Id = 9, Name = "Thịt bò", Description = "Thịt bò tươi ngon, thăn ngoại", Price = 30000, ImageUrl = "products9-min.jpg", StockQuantity = 180, Unit = Unit.Kg, SoldQuantity = 90, StoreId = 2, CategoryId = 4, IsActive = false, Origin = "Việt Nam", AverageRating = 4.6m, TotalSoldQuantity = 90, ReviewCount = 22, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 6, ReviewStar5Count = 14, CreatedAt = DateTime.Now },
                    new Product { Id = 10, Name = "Sườn bò", Description = "Sườn bò tươi ngon", Price = 120000, ImageUrl = "products10-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 25, StoreId = 2, CategoryId = 4, IsActive = true, Origin = "Việt Nam", AverageRating = 4.7m, TotalSoldQuantity = 25, ReviewCount = 12, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 2, ReviewStar5Count = 9, CreatedAt = DateTime.Now }
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
                        BuyerId = 3,
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
                        BuyerId = 3,
                        TotalAmount = 250000,
                        ShippingFee = 30000,
                        TaxAmount = 25000,
                        DiscountAmount = 0,
                        FinalAmount = 305000,
                        Status = OrderStatus.Pending,
                        PaymentStatus = PaymentStatus.Unpaid,
                        PaymentMethod = PaymentMethodEnum.BankTransfer,
                        ShippingName = "Nguyễn Văn A",
                        ShippingPhone = "0987654326",
                        ShippingAddress = "123 Đường ABC",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 1",
                        ShippingWard = "Phường 1",
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    new Order {
                        Id = 3,
                        OrderCode = "ORD003",
                        BuyerId = 3,
                        TotalAmount = 350000,
                        ShippingFee = 30000,
                        TaxAmount = 35000,
                        DiscountAmount = 0,
                        FinalAmount = 415000,
                        Status = OrderStatus.Processing,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.VNPay,
                        ShippingName = "Nguyễn Văn A",
                        ShippingPhone = "0987654326",
                        ShippingAddress = "123 Đường ABC",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 2",
                        ShippingWard = "Phường 2",
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
                    // Đơn hàng #1
                    new OrderDetail {
                        Id = 1,
                        OrderId = 1,
                        ProductId = 1,
                        Quantity = 2,
                        UnitPrice = 25000,
                        Subtotal = 50000,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    new OrderDetail {
                        Id = 2,
                        OrderId = 1,
                        ProductId = 2,
                        Quantity = 3,
                        UnitPrice = 15000,
                        Subtotal = 45000,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    new OrderDetail {
                        Id = 3,
                        OrderId = 1,
                        ProductId = 3,
                        Quantity = 2,
                        UnitPrice = 20000,
                        Subtotal = 40000,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    
                    // Đơn hàng #2
                    new OrderDetail {
                        Id = 4,
                        OrderId = 2,
                        ProductId = 4,
                        Quantity = 2,
                        UnitPrice = 80000,
                        Subtotal = 160000,
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    new OrderDetail {
                        Id = 5,
                        OrderId = 2,
                        ProductId = 5,
                        Quantity = 2,
                        UnitPrice = 45000,
                        Subtotal = 90000,
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    
                    // Đơn hàng #3
                    new OrderDetail {
                        Id = 6,
                        OrderId = 3,
                        ProductId = 7,
                        Quantity = 10,
                        UnitPrice = 35000,
                        Subtotal = 350000,
                        CreatedAt = DateTime.Now.AddDays(-1)
                    },
                    
                    // Đơn hàng #4
                    new OrderDetail {
                        Id = 7,
                        OrderId = 4,
                        ProductId = 2,
                        Quantity = 5,
                        UnitPrice = 15000,
                        Subtotal = 75000,
                        CreatedAt = DateTime.Now.AddDays(-7)
                    },
                    new OrderDetail {
                        Id = 8,
                        OrderId = 4,
                        ProductId = 3,
                        Quantity = 3,
                        UnitPrice = 20000,
                        Subtotal = 60000,
                        CreatedAt = DateTime.Now.AddDays(-7)
                    },
                    new OrderDetail {
                        Id = 9,
                        OrderId = 4,
                        ProductId = 1,
                        Quantity = 2,
                        UnitPrice = 25000,
                        Subtotal = 50000,
                        CreatedAt = DateTime.Now.AddDays(-7)
                    },
                    
                    // Đơn hàng #5
                    new OrderDetail {
                        Id = 10,
                        OrderId = 5,
                        ProductId = 6,
                        Quantity = 3,
                        UnitPrice = 55000,
                        Subtotal = 165000,
                        CreatedAt = DateTime.Now.AddDays(-10)
                    },
                    new OrderDetail {
                        Id = 11,
                        OrderId = 5,
                        ProductId = 5,
                        Quantity = 3,
                        UnitPrice = 45000,
                        Subtotal = 135000,
                        CreatedAt = DateTime.Now.AddDays(-10)
                    },
                    
                    // Đơn hàng #6
                    new OrderDetail {
                        Id = 12,
                        OrderId = 6,
                        ProductId = 9,
                        Quantity = 5,
                        UnitPrice = 30000,
                        Subtotal = 150000,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },
                    new OrderDetail {
                        Id = 13,
                        OrderId = 6,
                        ProductId = 7,
                        Quantity = 5,
                        UnitPrice = 35000,
                        Subtotal = 175000,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },
                    new OrderDetail {
                        Id = 14,
                        OrderId = 6,
                        ProductId = 8,
                        Quantity = 3,
                        UnitPrice = 25000,
                        Subtotal = 75000,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },
                    
                    // Đơn hàng #7
                    new OrderDetail {
                        Id = 15,
                        OrderId = 7,
                        ProductId = 10,
                        Quantity = 2,
                        UnitPrice = 120000,
                        Subtotal = 240000,
                        CreatedAt = DateTime.Now.AddDays(-8)
                    },
                    new OrderDetail {
                        Id = 16,
                        OrderId = 7,
                        ProductId = 3,
                        Quantity = 2,
                        UnitPrice = 20000,
                        Subtotal = 40000,
                        CreatedAt = DateTime.Now.AddDays(-8)
                    },
                    
                    // Đơn hàng #8
                    new OrderDetail {
                        Id = 17,
                        OrderId = 8,
                        ProductId = 13,
                        Quantity = 2,
                        UnitPrice = 45000,
                        Subtotal = 90000,
                        CreatedAt = DateTime.Now.AddDays(-2)
                    },
                    new OrderDetail {
                        Id = 18,
                        OrderId = 8,
                        ProductId = 14,
                        Quantity = 1,
                        UnitPrice = 90000,
                        Subtotal = 90000,
                        CreatedAt = DateTime.Now.AddDays(-2)
                    },
                    
                    // Đơn hàng #9
                    new OrderDetail {
                        Id = 19,
                        OrderId = 9,
                        ProductId = 16,
                        Quantity = 3,
                        UnitPrice = 75000,
                        Subtotal = 225000,
                        CreatedAt = DateTime.Now.AddDays(-4)
                    },
                    new OrderDetail {
                        Id = 20,
                        OrderId = 9,
                        ProductId = 17,
                        Quantity = 1,
                        UnitPrice = 120000,
                        Subtotal = 120000,
                        CreatedAt = DateTime.Now.AddDays(-4)
                    },
                    
                    // Đơn hàng #10
                    new OrderDetail {
                        Id = 21,
                        OrderId = 10,
                        ProductId = 19,
                        Quantity = 3,
                        UnitPrice = 55000,
                        Subtotal = 165000,
                        CreatedAt = DateTime.Now.AddDays(-6)
                    },
                    new OrderDetail {
                        Id = 22,
                        OrderId = 10,
                        ProductId = 20,
                        Quantity = 3,
                        UnitPrice = 30000,
                        Subtotal = 90000,
                        CreatedAt = DateTime.Now.AddDays(-6)
                    },
                    
                    // Đơn hàng #11
                    new OrderDetail {
                        Id = 23,
                        OrderId = 11,
                        ProductId = 22,
                        Quantity = 4,
                        UnitPrice = 40000,
                        Subtotal = 160000,
                        CreatedAt = DateTime.Now.AddDays(-12)
                    },
                    new OrderDetail {
                        Id = 24,
                        OrderId = 11,
                        ProductId = 23,
                        Quantity = 3,
                        UnitPrice = 50000,
                        Subtotal = 150000,
                        CreatedAt = DateTime.Now.AddDays(-12)
                    },
                    
                    // Đơn hàng #12
                    new OrderDetail {
                        Id = 25,
                        OrderId = 12,
                        ProductId = 25,
                        Quantity = 2,
                        UnitPrice = 200000,
                        Subtotal = 400000,
                        CreatedAt = DateTime.Now.AddDays(-1)
                    },
                    new OrderDetail {
                        Id = 26,
                        OrderId = 12,
                        ProductId = 26,
                        Quantity = 1,
                        UnitPrice = 50000,
                        Subtotal = 50000,
                        CreatedAt = DateTime.Now.AddDays(-1)
                    },
                    
                    // Đơn hàng #13
                    new OrderDetail {
                        Id = 27,
                        OrderId = 13,
                        ProductId = 28,
                        Quantity = 1,
                        UnitPrice = 180000,
                        Subtotal = 180000,
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    new OrderDetail {
                        Id = 28,
                        OrderId = 13,
                        ProductId = 30,
                        Quantity = 1,
                        UnitPrice = 20000,
                        Subtotal = 20000,
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    
                    // Đơn hàng #14
                    new OrderDetail {
                        Id = 29,
                        OrderId = 14,
                        ProductId = 1,
                        Quantity = 5,
                        UnitPrice = 25000,
                        Subtotal = 125000,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    new OrderDetail {
                        Id = 30,
                        OrderId = 14,
                        ProductId = 2,
                        Quantity = 7,
                        UnitPrice = 15000,
                        Subtotal = 105000,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    new OrderDetail {
                        Id = 31,
                        OrderId = 14,
                        ProductId = 3,
                        Quantity = 7,
                        UnitPrice = 20000,
                        Subtotal = 140000,
                        CreatedAt = DateTime.Now.AddDays(-5)
                    },
                    
                    // Đơn hàng #15
                    new OrderDetail {
                        Id = 32,
                        OrderId = 15,
                        ProductId = 4,
                        Quantity = 3,
                        UnitPrice = 80000,
                        Subtotal = 240000,
                        CreatedAt = DateTime.Now.AddDays(-9)
                    },
                    
                    // Đơn hàng #16
                    new OrderDetail {
                        Id = 33,
                        OrderId = 16,
                        ProductId = 7,
                        Quantity = 5,
                        UnitPrice = 35000,
                        Subtotal = 175000,
                        CreatedAt = DateTime.Now.AddDays(-7)
                    },
                    new OrderDetail {
                        Id = 34,
                        OrderId = 16,
                        ProductId = 9,
                        Quantity = 3,
                        UnitPrice = 30000,
                        Subtotal = 90000,
                        CreatedAt = DateTime.Now.AddDays(-7)
                    },
                    
                    // Đơn hàng #17
                    new OrderDetail {
                        Id = 35,
                        OrderId = 17,
                        ProductId = 10,
                        Quantity = 2,
                        UnitPrice = 120000,
                        Subtotal = 240000,
                        CreatedAt = DateTime.Now.AddDays(-14)
                    },
                    new OrderDetail {
                        Id = 36,
                        OrderId = 17,
                        ProductId = 11,
                        Quantity = 1,
                        UnitPrice = 90000,
                        Subtotal = 90000,
                        CreatedAt = DateTime.Now.AddDays(-14)
                    },
                    
                    // Đơn hàng #18
                    new OrderDetail {
                        Id = 37,
                        OrderId = 18,
                        ProductId = 14,
                        Quantity = 3,
                        UnitPrice = 90000,
                        Subtotal = 270000,
                        CreatedAt = DateTime.Now.AddDays(-2)
                    },
                    new OrderDetail {
                        Id = 38,
                        OrderId = 18,
                        ProductId = 15,
                        Quantity = 4,
                        UnitPrice = 35000,
                        Subtotal = 140000,
                        CreatedAt = DateTime.Now.AddDays(-2)
                    },
                    
                    // Đơn hàng #19
                    new OrderDetail {
                        Id = 39,
                        OrderId = 19,
                        ProductId = 18,
                        Quantity = 3,
                        UnitPrice = 25000,
                        Subtotal = 75000,
                        CreatedAt = DateTime.Now.AddDays(-4)
                    },
                    new OrderDetail {
                        Id = 40,
                        OrderId = 19,
                        ProductId = 16,
                        Quantity = 1,
                        UnitPrice = 75000,
                        Subtotal = 75000,
                        CreatedAt = DateTime.Now.AddDays(-4)
                    },
                    
                    // Đơn hàng #20
                    new OrderDetail {
                        Id = 41,
                        OrderId = 20,
                        ProductId = 20,
                        Quantity = 4,
                        UnitPrice = 30000,
                        Subtotal = 120000,
                        CreatedAt = DateTime.Now.AddDays(-1)
                    },
                    new OrderDetail {
                        Id = 42,
                        OrderId = 20,
                        ProductId = 21,
                        Quantity = 3,
                        UnitPrice = 65000,
                        Subtotal = 195000,
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
                    new Review { Id = 1, UserId = 3, ProductId = 1, Rating = 5, Content = "Việt quất tươi ngon, ngọt vừa phải", ImageUrl = "products1-min.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-5) },
                    new Review { Id = 2, UserId = 3, ProductId = 2, Rating = 4, Content = "Sữa tươi thơm ngon, đảm bảo chất lượng", ImageUrl = "products2-min.jpg", ShopResponse = "Chúng tôi sẽ cố gắng hơn nữa", CreatedAt = DateTime.Now.AddDays(-4) },
                    new Review { Id = 3, UserId = 3, ProductId = 3, Rating = 5, Content = "Sữa hạnh nhân rất thơm, vị béo nhẹ", ImageUrl = "products3-min.jpg", ShopResponse = "Cảm ơn bạn", CreatedAt = DateTime.Now.AddDays(-3) },
                    new Review { Id = 4, UserId = 3, ProductId = 4, Rating = 4, Content = "Súp lơ xanh tươi, giòn ngon", ImageUrl = "products4-min.jpg", ShopResponse = "Rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-2) },
                    new Review { Id = 5, UserId = 3, ProductId = 5, Rating = 5, Content = "Súp lơ tươi hữu cơ, không thuốc trừ sâu", ImageUrl = "products5-min.jpg", ShopResponse = "Cảm ơn bạn đã tin tưởng", CreatedAt = DateTime.Now.AddDays(-1) }
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
                    new Transaction { Id = 1, OrderId = 1, BuyerId = 3, Amount = 150000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-5) },
                    new Transaction { Id = 2, OrderId = 2, BuyerId = 3, Amount = 250000, Status = TransactionStatus.Pending, PaymentMethod = PaymentMethodEnum.BankTransfer, CreatedAt = DateTime.Now.AddDays(-3) },
                    new Transaction { Id = 3, OrderId = 3, BuyerId = 3, Amount = 350000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.VNPay, CreatedAt = DateTime.Now.AddDays(-1) }
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
                    new ChatRoom { Id = 1, NameRoom = "Chat Room 1", Description = "Chat về đơn hàng #1", BuyerId = 3, SellerId = 2, OrderId = 1, Type = ChatRoomType.ChatNormal, Status = ChatRoomStatus.InProgress, LastMessage = "Xin chào", LastMessageTime = DateTime.Now.AddDays(-5), IsActive = true },
                    new ChatRoom { Id = 2, NameRoom = "Chat Room 2", Description = "Chat về đơn hàng #2", BuyerId = 3, SellerId = 2, OrderId = 2, Type = ChatRoomType.ChatNormal, Status = ChatRoomStatus.InProgress, LastMessage = "Cảm ơn", LastMessageTime = DateTime.Now.AddDays(-3), IsActive = true },
                    new ChatRoom { Id = 3, NameRoom = "Chat Room 3", Description = "Chat với Admin", BuyerId = 3, SellerId = 1, OrderId = 3, Type = ChatRoomType.ChatNormal, Status = ChatRoomStatus.InProgress, LastMessage = "Tạm biệt", LastMessageTime = DateTime.Now.AddDays(-1), IsActive = true }
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
                    new Chat { Id = 1, SenderId = 3, ChatRoomId = 1, Content = "Xin chào", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 2, SenderId = 2, ChatRoomId = 1, Content = "Chào bạn", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 3, SenderId = 3, ChatRoomId = 2, Content = "Cảm ơn", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 4, SenderId = 2, ChatRoomId = 2, Content = "Không có gì", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 5, SenderId = 3, ChatRoomId = 3, Content = "Tạm biệt", ImageUrl = "", Type = ChatMessageType.Text },
                    new Chat { Id = 6, SenderId = 1, ChatRoomId = 3, Content = "Hẹn gặp lại", ImageUrl = "", Type = ChatMessageType.Text }
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
                        UserId = 3,
                        Content = "Đơn hàng #1234 của bạn đã được xác nhận",
                        LinkUrl = "/orders/1234",
                        Type = NotificationType.Order,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Notification
                    {
                        Id = 2,
                        UserId = 3,
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
                        Content = "Cửa hàng của bạn đã được duyệt",
                        LinkUrl = "/store",
                        Type = NotificationType.Store,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new Notification
                    {
                        Id = 4,
                        UserId = 1,
                        Content = "Tài khoản Admin đã được xác minh",
                        LinkUrl = "/profile",
                        Type = NotificationType.Account,
                        IsRead = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-3)
                    },
                    new Notification
                    {
                        Id = 5,
                        UserId = 2,
                        Content = "Bạn có đơn hàng mới từ Nguyễn Văn A",
                        LinkUrl = "/orders",
                        Type = NotificationType.Order,
                        IsRead = false,
                        CreatedAt = DateTime.UtcNow.AddDays(-1)
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
                        AccountHolderName = "Admin User",
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
                        AccountHolderName = "Fruit Paradise",
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
                        AccountHolderName = "Nguyễn Văn A",
                        BankName = "Momo",
                        UserId = 3,
                        CreatedAt = DateTime.UtcNow.AddDays(-2)
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
                        UserId = 2,
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
                        UserId = 3,
                        CreatedAt = DateTime.UtcNow.AddDays(-1)
                    }
                };

                await context.BusinessRegistrations.AddRangeAsync(businessRegistrations);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedOrderTimelinesAsync(VNFarmContext context)
        {
            
            if (!context.OrderTimelines.Any())
            {
                var timelines = new List<OrderTimeline>();

                for (int i = 1; i <= 20; i++) // Có 20 đơn hàng
                {
                    // Tạo số lượng timeline bằng Max(1, i%6) cho mỗi đơn hàng
                    int timelineCount = Math.Max(1, i % 7);
                    var baseTime = DateTime.Now.AddDays(-i);
                    
                    // Luôn thêm OrderCreated làm timeline đầu tiên
                    timelines.Add(new OrderTimeline
                    {
                        OrderId = i,
                        EventType = OrderEventType.OrderCreated,
                        Status = OrderTimelineStatus.Completed,
                        Description = "Đơn hàng được tạo tự động",
                        CreatedAt = baseTime
                    });
                    
                    // Danh sách các loại sự kiện có thể có
                    var possibleEvents = new List<OrderEventType>
                    {
                        OrderEventType.OrderAcceptedBySeller,
                        OrderEventType.OrderReadyToShip,
                        OrderEventType.OrderShipped,
                        OrderEventType.OrderCompleted
                    };
                    
                    // Thêm các timeline tiếp theo nếu cần
                    for (int j = 1; j < timelineCount; j++)
                    {
                        if (j < possibleEvents.Count)
                        {
                            timelines.Add(new OrderTimeline
                            {
                                OrderId = i,
                                EventType = possibleEvents[j],
                                Status = OrderTimelineStatus.Completed,
                                Description = OrderUtils.GetContentForOrderTimeline(new OrderTimeline { EventType = possibleEvents[j] }),
                                CreatedAt = baseTime.AddHours(j * 4), // Mỗi sự kiện cách nhau 4 giờ
                                UpdatedAt = baseTime.AddHours(j * 4)
                            });
                        }
                    }
                }

                await context.OrderTimelines.AddRangeAsync(timelines);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedDiscountsAsync(VNFarmContext context)
        {
            if (!context.Discounts.Any())
            {
                var discounts = new List<Discount>
                {
                    // Mã giảm giá theo phần trăm
                    new Discount
                    {
                        Id = 1,
                        Code = "GIAM10PT",
                        Description = "Giảm 10% cho tất cả đơn hàng",
                        RemainingQuantity = 100,
                        Status = DiscountStatus.Active,
                        StartDate = DateTime.Now.AddDays(-10),
                        EndDate = DateTime.Now.AddDays(30),
                        Type = DiscountType.Percentage,
                        DiscountAmount = 10,
                        MinimumOrderAmount = 100000,
                        MaximumDiscountAmount = 50000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Discount
                    {
                        Id = 2,
                        Code = "GIAM20PT",
                        Description = "Giảm 20% cho tất cả đơn hàng",
                        RemainingQuantity = 50,
                        Status = DiscountStatus.Active,
                        StartDate = DateTime.Now.AddDays(-5),
                        EndDate = DateTime.Now.AddDays(15),
                        Type = DiscountType.Percentage,
                        DiscountAmount = 20,
                        MinimumOrderAmount = 200000,
                        MaximumDiscountAmount = 100000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Discount
                    {
                        Id = 3,
                        Code = "GIAM30PT",
                        Description = "Giảm 30% cho tất cả đơn hàng",
                        RemainingQuantity = 30,
                        Status = DiscountStatus.Active,
                        StartDate = DateTime.Now.AddDays(-2),
                        EndDate = DateTime.Now.AddDays(10),
                        Type = DiscountType.Percentage,
                        DiscountAmount = 30,
                        MinimumOrderAmount = 300000,
                        MaximumDiscountAmount = 150000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    
                    // Mã giảm giá theo số tiền cố định
                    new Discount
                    {
                        Id = 4,
                        Code = "GIAM50K",
                        Description = "Giảm 50.000đ cho tất cả đơn hàng",
                        RemainingQuantity = 100,
                        Status = DiscountStatus.Active,
                        StartDate = DateTime.Now.AddDays(-10),
                        EndDate = DateTime.Now.AddDays(30),
                        Type = DiscountType.FixedAmount,
                        DiscountAmount = 50000,
                        MinimumOrderAmount = 150000,
                        MaximumDiscountAmount = 50000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Discount
                    {
                        Id = 5,
                        Code = "GIAM100K",
                        Description = "Giảm 100.000đ cho tất cả đơn hàng",
                        RemainingQuantity = 50,
                        Status = DiscountStatus.Active,
                        StartDate = DateTime.Now.AddDays(-5),
                        EndDate = DateTime.Now.AddDays(15),
                        Type = DiscountType.FixedAmount,
                        DiscountAmount = 100000,
                        MinimumOrderAmount = 300000,
                        MaximumDiscountAmount = 100000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Discount
                    {
                        Id = 6,
                        Code = "GIAM200K",
                        Description = "Giảm 200.000đ cho tất cả đơn hàng",
                        RemainingQuantity = 30,
                        Status = DiscountStatus.Active,
                        StartDate = DateTime.Now.AddDays(-2),
                        EndDate = DateTime.Now.AddDays(10),
                        Type = DiscountType.FixedAmount,
                        DiscountAmount = 200000,
                        MinimumOrderAmount = 500000,
                        MaximumDiscountAmount = 200000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                };

                await context.Discounts.AddRangeAsync(discounts);
                await context.SaveChangesAsync();
            }
        }
    }
}