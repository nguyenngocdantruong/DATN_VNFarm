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
                    new User { Id = 1, FullName = "Admin User", Email = "admin@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654321", Address = "Hà Nội", ImageUrl = "avatar-1.jpg", Role = UserRole.Admin, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 2, FullName = "System Admin", Email = "sysadmin@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654322", Address = "TP.HCM", ImageUrl = "avatar-2.jpg", Role = UserRole.Admin, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    
                    // Seller users
                    new User { Id = 3, FullName = "Organic Farm", Email = "organic@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654323", Address = "Đà Lạt", ImageUrl = "avatar-3.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 4, FullName = "Fruit Paradise", Email = "fruit@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654324", Address = "Tiền Giang", ImageUrl = "avatar-4.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 5, FullName = "Rice King", Email = "rice@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654325", Address = "Cần Thơ", ImageUrl = "avatar-5.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    
                    // Buyer users
                    new User { Id = 6, FullName = "Nguyễn Văn A", Email = "buyer1@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654326", Address = "Hà Nội", ImageUrl = "avatar-6.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 7, FullName = "Trần Thị B", Email = "buyer2@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654327", Address = "TP.HCM", ImageUrl = "avatar-7.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 8, FullName = "Lê Văn C", Email = "buyer3@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654328", Address = "Đà Nẵng", ImageUrl = "avatar-8.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 9, FullName = "Phạm Thị D", Email = "buyer4@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654329", Address = "Cần Thơ", ImageUrl = "avatar-9.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 10, FullName = "Hoàng Văn E", Email = "buyer5@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654330", Address = "Hải Phòng", ImageUrl = "avatar-10.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 11, FullName = "Vũ Thị F", Email = "buyer6@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654331", Address = "Nha Trang", ImageUrl = "avatar-11.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 12, FullName = "Đặng Văn G", Email = "buyer7@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654332", Address = "Vũng Tàu", ImageUrl = "avatar-12.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    
                    // Thêm nhiều Seller hơn nữa
                    new User { Id = 13, FullName = "Fresh Veggies", Email = "veggies@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654333", Address = "Lâm Đồng", ImageUrl = "avatar-1.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 14, FullName = "Meat Market", Email = "meat@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654334", Address = "Đồng Nai", ImageUrl = "avatar-2.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 15, FullName = "Spice World", Email = "spice@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654335", Address = "Bình Dương", ImageUrl = "avatar-3.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 16, FullName = "Tea Garden", Email = "tea@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654336", Address = "Thái Nguyên", ImageUrl = "avatar-4.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 17, FullName = "Coffee Paradise", Email = "coffee@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654337", Address = "Buôn Ma Thuột", ImageUrl = "avatar-5.jpg", Role = UserRole.Seller, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    
                    // Thêm nhiều Buyer hơn nữa
                    new User { Id = 18, FullName = "Đinh Thị H", Email = "buyer8@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654338", Address = "Hà Tĩnh", ImageUrl = "avatar-6.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 19, FullName = "Ngô Văn I", Email = "buyer9@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654339", Address = "Nghệ An", ImageUrl = "avatar-7.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 20, FullName = "Trịnh Thị K", Email = "buyer10@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654340", Address = "Thanh Hóa", ImageUrl = "avatar-8.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 21, FullName = "Lý Văn L", Email = "buyer11@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654341", Address = "Hà Nam", ImageUrl = "avatar-9.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 22, FullName = "Dương Thị M", Email = "buyer12@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654342", Address = "Thái Bình", ImageUrl = "avatar-10.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 23, FullName = "Đỗ Văn N", Email = "buyer13@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654343", Address = "Nam Định", ImageUrl = "avatar-11.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 24, FullName = "Hồ Thị O", Email = "buyer14@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654344", Address = "Hưng Yên", ImageUrl = "avatar-12.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now },
                    new User { Id = 25, FullName = "Trương Văn P", Email = "buyer15@vnfarm.com", PasswordHash = "AQAAAAIAAYagAAAAEMjjLtAFwg9+Q90qrxcrzlkFjHzTsIJzBP9hD2LBtZjBKtV+yFGAT3J2+MG62Z81OQ==", PhoneNumber = "0987654345", Address = "Quảng Ninh", ImageUrl = "avatar-1.jpg", Role = UserRole.Buyer, IsActive = true, EmailVerified = true, CreatedAt = DateTime.Now }
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
                    new Store { Id = 1, Name = "Organic Farm", Description = "Chuyên cung cấp rau củ organic", Address = "Đà Lạt", PhoneNumber = "0987654323", Email = "organic@vnfarm.com", LogoUrl = "/images/stores/organic.png", IsActive = true, VerificationStatus = StoreStatus.Pending, AverageRating = 4.8m, ReviewCount = 150, UserId = 3, CreatedAt = DateTime.Now },
                    new Store { Id = 2, Name = "Fruit Paradise", Description = "Chuyên cung cấp trái cây tươi", Address = "Tiền Giang", PhoneNumber = "0987654324", Email = "fruit@vnfarm.com", LogoUrl = "/images/stores/fruit.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.7m, ReviewCount = 120, UserId = 4, CreatedAt = DateTime.Now },
                    new Store { Id = 3, Name = "Rice King", Description = "Chuyên cung cấp gạo chất lượng cao", Address = "Cần Thơ", PhoneNumber = "0987654325", Email = "rice@vnfarm.com", LogoUrl = "/images/stores/rice.png", IsActive = true, VerificationStatus = StoreStatus.Rejected, AverageRating = 4.9m, ReviewCount = 200, UserId = 5, CreatedAt = DateTime.Now },
                    new Store { Id = 4, Name = "Meat Master", Description = "Chuyên cung cấp thịt cá tươi sống", Address = "Hà Nội", PhoneNumber = "0987654333", Email = "meat@vnfarm.com", LogoUrl = "/images/stores/meat.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.6m, ReviewCount = 100, UserId = 13, CreatedAt = DateTime.Now },
                    new Store { Id = 5, Name = "Spice World", Description = "Chuyên cung cấp gia vị", Address = "TP.HCM", PhoneNumber = "0987654334", Email = "spice@vnfarm.com", LogoUrl = "/images/stores/spice.png", IsActive = true, VerificationStatus = StoreStatus.Pending, AverageRating = 4.5m, ReviewCount = 80, UserId = 14, CreatedAt = DateTime.Now },
                    new Store { Id = 6, Name = "Drink Heaven", Description = "Chuyên cung cấp đồ uống", Address = "Đà Nẵng", PhoneNumber = "0987654335", Email = "drink@vnfarm.com", LogoUrl = "/images/stores/drink.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.4m, ReviewCount = 90, UserId = 15, CreatedAt = DateTime.Now },
                    new Store { Id = 7, Name = "Sweet Dreams", Description = "Chuyên cung cấp bánh kẹo", Address = "Hải Phòng", PhoneNumber = "0987654336", Email = "sweet@vnfarm.com", LogoUrl = "/images/stores/sweet.png", IsActive = true, VerificationStatus = StoreStatus.Rejected, AverageRating = 4.7m, ReviewCount = 110, UserId = 16, CreatedAt = DateTime.Now },
                    new Store { Id = 8, Name = "Canned Food", Description = "Chuyên cung cấp đồ hộp", Address = "Nha Trang", PhoneNumber = "0987654337", Email = "canned@vnfarm.com", LogoUrl = "/images/stores/canned.png", IsActive = true, VerificationStatus = StoreStatus.Rejected, AverageRating = 4.3m, ReviewCount = 70, UserId = 17, CreatedAt = DateTime.Now },
                    new Store { Id = 9, Name = "Frozen Food", Description = "Chuyên cung cấp đồ đông lạnh", Address = "Vũng Tàu", PhoneNumber = "0987654338", Email = "frozen@vnfarm.com", LogoUrl = "/images/stores/frozen.png", IsActive = true, VerificationStatus = StoreStatus.Pending, AverageRating = 4.6m, ReviewCount = 95, UserId = 18, CreatedAt = DateTime.Now },
                    new Store { Id = 10, Name = "Dried Food", Description = "Chuyên cung cấp đồ khô", Address = "Cần Thơ", PhoneNumber = "0987654339", Email = "dried@vnfarm.com", LogoUrl = "/images/stores/dried.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.5m, ReviewCount = 85, UserId = 19, CreatedAt = DateTime.Now },
                    new Store { Id = 11, Name = "Vegetarian Food", Description = "Chuyên cung cấp đồ chay", Address = "TP.HCM", PhoneNumber = "0987654340", Email = "vegetarian@vnfarm.com", LogoUrl = "/images/stores/vegetarian.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = 4.8m, ReviewCount = 130, UserId = 20, CreatedAt = DateTime.Now },
                    new Store { Id = 12, Name = "Fast Food", Description = "Chuyên cung cấp đồ ăn nhanh", Address = "Hà Nội", PhoneNumber = "0987654341", Email = "fastfood@vnfarm.com", LogoUrl = "/images/stores/fast-food.png", IsActive = true, VerificationStatus = StoreStatus.Rejected, AverageRating = 4.4m, ReviewCount = 75, UserId = 21, CreatedAt = DateTime.Now }
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
                    // Updating product information based on image names
                    new Product { Id = 1, Name = "Việt quất", Description = "Việt quất tươi ngon, giàu chất chống oxy hóa", Price = 25000, ImageUrl = "products1-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 50, StoreId = 1, CategoryId = 2, IsActive = false, Origin = "Đà Lạt", AverageRating = 4.5m, TotalSoldQuantity = 50, ReviewCount = 10, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 3, ReviewStar5Count = 6, CreatedAt = DateTime.Now },
                    new Product { Id = 2, Name = "Sữa tươi", Description = "Sữa tươi thanh trùng nguyên chất", Price = 15000, ImageUrl = "products2-min.jpg", StockQuantity = 200, Unit = Unit.Bag, SoldQuantity = 100, StoreId = 1, CategoryId = 6, IsActive = true, Origin = "Việt Nam", AverageRating = 4.3m, TotalSoldQuantity = 100, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 2, ReviewStar4Count = 7, ReviewStar5Count = 5, CreatedAt = DateTime.Now },
                    new Product { Id = 3, Name = "Sữa hạnh nhân", Description = "Sữa hạnh nhân hữu cơ, không đường", Price = 20000, ImageUrl = "products3-min.jpg", StockQuantity = 150, Unit = Unit.Bag, SoldQuantity = 75, StoreId = 1, CategoryId = 6, IsActive = true, Origin = "Mỹ", AverageRating = 4.4m, TotalSoldQuantity = 75, ReviewCount = 12, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 2, ReviewStar4Count = 5, ReviewStar5Count = 5, CreatedAt = DateTime.Now },
                    
                    // Vegetables
                    new Product { Id = 4, Name = "Súp lơ", Description = "Súp lơ xanh tươi ngon", Price = 80000, ImageUrl = "products4-min.jpg", StockQuantity = 80, Unit = Unit.Kg, SoldQuantity = 30, StoreId = 2, CategoryId = 1, IsActive = false, Origin = "Đà Lạt", AverageRating = 4.8m, TotalSoldQuantity = 30, ReviewCount = 8, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 6, CreatedAt = DateTime.Now },
                    new Product { Id = 5, Name = "Súp lơ", Description = "Súp lơ tươi hữu cơ", Price = 45000, ImageUrl = "products5-min.jpg", StockQuantity = 120, Unit = Unit.Kg, SoldQuantity = 60, StoreId = 2, CategoryId = 1, IsActive = true, Origin = "Đà Lạt", AverageRating = 4.6m, TotalSoldQuantity = 60, ReviewCount = 20, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 5, ReviewStar5Count = 13, CreatedAt = DateTime.Now },
                    new Product { Id = 6, Name = "Súp lơ", Description = "Súp lơ nhỏ tươi ngon", Price = 55000, ImageUrl = "products6-min.jpg", StockQuantity = 90, Unit = Unit.Kg, SoldQuantity = 45, StoreId = 2, CategoryId = 1, IsActive = true, Origin = "Lâm Đồng", AverageRating = 4.7m, TotalSoldQuantity = 45, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 3, ReviewStar5Count = 11, CreatedAt = DateTime.Now },
                    
                    // Fruits
                    new Product { Id = 7, Name = "Nho", Description = "Nho xanh không hạt", Price = 35000, ImageUrl = "products7-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 100, StoreId = 3, CategoryId = 2, IsActive = false, Origin = "Ninh Thuận", AverageRating = 5.0m, TotalSoldQuantity = 100, ReviewCount = 25, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 0, ReviewStar5Count = 25, CreatedAt = DateTime.Now },
                    new Product { Id = 8, Name = "Vải thiều", Description = "Vải thiều tươi ngon từ Bắc Giang", Price = 25000, ImageUrl = "products8-min.jpg", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 75, StoreId = 3, CategoryId = 2, IsActive = true, Origin = "Bắc Giang", AverageRating = 4.5m, TotalSoldQuantity = 75, ReviewCount = 18, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 7, ReviewStar5Count = 9, CreatedAt = DateTime.Now },
                    new Product { Id = 9, Name = "Thịt bò", Description = "Thịt bò tươi ngon, thăn ngoại", Price = 30000, ImageUrl = "products9-min.jpg", StockQuantity = 180, Unit = Unit.Kg, SoldQuantity = 90, StoreId = 3, CategoryId = 4, IsActive = false, Origin = "Việt Nam", AverageRating = 4.6m, TotalSoldQuantity = 90, ReviewCount = 22, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 6, ReviewStar5Count = 14, CreatedAt = DateTime.Now },
                    
                    // Meat
                    new Product { Id = 10, Name = "Sườn bò", Description = "Sườn bò tươi ngon", Price = 120000, ImageUrl = "products10-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 25, StoreId = 4, CategoryId = 4, IsActive = true, Origin = "Việt Nam", AverageRating = 4.7m, TotalSoldQuantity = 25, ReviewCount = 12, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 2, ReviewStar5Count = 9, CreatedAt = DateTime.Now },
                    new Product { Id = 11, Name = "Kiwi", Description = "Kiwi xanh New Zealand", Price = 90000, ImageUrl = "products11-min.jpg", StockQuantity = 40, Unit = Unit.Kg, SoldQuantity = 20, StoreId = 4, CategoryId = 2, IsActive = true, Origin = "New Zealand", AverageRating = 4.8m, TotalSoldQuantity = 20, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 2, ReviewStar5Count = 12, CreatedAt = DateTime.Now },
                    new Product { Id = 12, Name = "Dâu tây", Description = "Dâu tây Đà Lạt tươi ngọt", Price = 250000, ImageUrl = "products12-min.jpg", StockQuantity = 30, Unit = Unit.Kg, SoldQuantity = 15, StoreId = 4, CategoryId = 2, IsActive = true, Origin = "Đà Lạt", AverageRating = 4.9m, TotalSoldQuantity = 15, ReviewCount = 20, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 18, CreatedAt = DateTime.Now },
                    
                    // Thêm nhiều sản phẩm khác
                    // More fruits
                    new Product { Id = 13, Name = "Măng cụt", Description = "Măng cụt tươi ngọt, múi dày", Price = 45000, ImageUrl = "products13-min.jpg", StockQuantity = 120, Unit = Unit.Kg, SoldQuantity = 60, StoreId = 5, CategoryId = 2, IsActive = true, Origin = "Bến Tre", AverageRating = 4.7m, TotalSoldQuantity = 60, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 3, ReviewStar5Count = 11, CreatedAt = DateTime.Now },
                    new Product { Id = 14, Name = "Thanh long", Description = "Thanh long ruột đỏ Bình Thuận", Price = 90000, ImageUrl = "products14-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 55, StoreId = 5, CategoryId = 2, IsActive = true, Origin = "Bình Thuận", AverageRating = 4.9m, TotalSoldQuantity = 55, ReviewCount = 22, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 20, CreatedAt = DateTime.Now },
                    new Product { Id = 15, Name = "Gừng", Description = "Gừng tươi hữu cơ", Price = 35000, ImageUrl = "products15-min.jpg", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 75, StoreId = 5, CategoryId = 5, IsActive = true, Origin = "Quảng Nam", AverageRating = 4.6m, TotalSoldQuantity = 75, ReviewCount = 18, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 2, ReviewStar4Count = 5, ReviewStar5Count = 11, CreatedAt = DateTime.Now },
                    
                    // Vegetables and fruits
                    new Product { Id = 16, Name = "Susu", Description = "Susu tươi ngon từ Sapa", Price = 75000, ImageUrl = "products16-min.jpg", StockQuantity = 90, Unit = Unit.Kg, SoldQuantity = 45, StoreId = 6, CategoryId = 1, IsActive = true, Origin = "Sapa", AverageRating = 4.8m, TotalSoldQuantity = 45, ReviewCount = 20, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 3, ReviewStar5Count = 16, CreatedAt = DateTime.Now },
                    new Product { Id = 17, Name = "Chanh", Description = "Chanh tươi không hạt", Price = 120000, ImageUrl = "products17-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 40, StoreId = 6, CategoryId = 2, IsActive = true, Origin = "Tiền Giang", AverageRating = 4.9m, TotalSoldQuantity = 40, ReviewCount = 25, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 3, ReviewStar5Count = 22, CreatedAt = DateTime.Now },
                    new Product { Id = 18, Name = "Chanh", Description = "Chanh vàng nhập khẩu", Price = 25000, ImageUrl = "products18-min.jpg", StockQuantity = 200, Unit = Unit.Kg, SoldQuantity = 150, StoreId = 6, CategoryId = 2, IsActive = true, Origin = "Mỹ", AverageRating = 4.7m, TotalSoldQuantity = 150, ReviewCount = 30, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 2, ReviewStar4Count = 5, ReviewStar5Count = 22, CreatedAt = DateTime.Now },
                    
                    // More products
                    new Product { Id = 19, Name = "Chuối", Description = "Chuối tiêu chín tự nhiên", Price = 55000, ImageUrl = "products19-min.jpg", StockQuantity = 120, Unit = Unit.Kg, SoldQuantity = 70, StoreId = 7, CategoryId = 2, IsActive = true, Origin = "Đồng Nai", AverageRating = 4.6m, TotalSoldQuantity = 70, ReviewCount = 18, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 2, ReviewStar4Count = 4, ReviewStar5Count = 12, CreatedAt = DateTime.Now },
                    new Product { Id = 20, Name = "Thịt xay", Description = "Thịt bò xay tươi ngon", Price = 30000, ImageUrl = "products20-min.jpg", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 90, StoreId = 7, CategoryId = 4, IsActive = true, Origin = "Việt Nam", AverageRating = 4.8m, TotalSoldQuantity = 90, ReviewCount = 25, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 4, ReviewStar5Count = 20, CreatedAt = DateTime.Now },
                    new Product { Id = 21, Name = "Nho tím", Description = "Nho tím không hạt nhập khẩu", Price = 65000, ImageUrl = "products21-min.jpg", StockQuantity = 100, Unit = Unit.Kg, SoldQuantity = 50, StoreId = 7, CategoryId = 2, IsActive = true, Origin = "Úc", AverageRating = 4.9m, TotalSoldQuantity = 50, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 13, CreatedAt = DateTime.Now },
                    
                    // More fruits and vegetables
                    new Product { Id = 22, Name = "Đào", Description = "Đào tươi mọng nước", Price = 40000, ImageUrl = "products22-min.jpg", StockQuantity = 180, Unit = Unit.Kg, SoldQuantity = 90, StoreId = 8, CategoryId = 2, IsActive = true, Origin = "Trung Quốc", AverageRating = 4.4m, TotalSoldQuantity = 90, ReviewCount = 20, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 2, ReviewStar4Count = 8, ReviewStar5Count = 9, CreatedAt = DateTime.Now },
                    new Product { Id = 23, Name = "Súp lơ trắng", Description = "Súp lơ trắng tươi ngon", Price = 50000, ImageUrl = "products23-min.jpg", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 75, StoreId = 8, CategoryId = 1, IsActive = true, Origin = "Đà Lạt", AverageRating = 4.5m, TotalSoldQuantity = 75, ReviewCount = 22, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 8, ReviewStar5Count = 12, CreatedAt = DateTime.Now },
                    new Product { Id = 24, Name = "Súp lơ trắng", Description = "Súp lơ trắng hữu cơ", Price = 35000, ImageUrl = "products24-min.jpg", StockQuantity = 200, Unit = Unit.Kg, SoldQuantity = 100, StoreId = 8, CategoryId = 1, IsActive = true, Origin = "Lâm Đồng", AverageRating = 4.3m, TotalSoldQuantity = 100, ReviewCount = 18, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 3, ReviewStar4Count = 6, ReviewStar5Count = 8, CreatedAt = DateTime.Now },
                    
                    // Last products
                    new Product { Id = 25, Name = "Súp lơ trắng", Description = "Súp lơ trắng tươi ngon size lớn", Price = 200000, ImageUrl = "products25-min.jpg", StockQuantity = 80, Unit = Unit.Kg, SoldQuantity = 40, StoreId = 9, CategoryId = 1, IsActive = true, Origin = "Đà Lạt", AverageRating = 4.8m, TotalSoldQuantity = 40, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 3, ReviewStar5Count = 12, CreatedAt = DateTime.Now },
                    new Product { Id = 26, Name = "Thịt viên", Description = "Thịt viên tươi ngon", Price = 75000, ImageUrl = "products26-min.jpg", StockQuantity = 120, Unit = Unit.Pack, SoldQuantity = 80, StoreId = 9, CategoryId = 4, IsActive = true, Origin = "Việt Nam", AverageRating = 4.7m, TotalSoldQuantity = 80, ReviewCount = 25, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 2, ReviewStar4Count = 5, ReviewStar5Count = 18, CreatedAt = DateTime.Now },
                    new Product { Id = 27, Name = "Thanh long", Description = "Thanh long ruột trắng", Price = 60000, ImageUrl = "products27-min.jpg", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 85, StoreId = 9, CategoryId = 2, IsActive = true, Origin = "Bình Thuận", AverageRating = 4.6m, TotalSoldQuantity = 85, ReviewCount = 20, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 2, ReviewStar4Count = 6, ReviewStar5Count = 12, CreatedAt = DateTime.Now },
                    
                    // Fruits and meat
                    new Product { Id = 28, Name = "Vải thiều", Description = "Vải thiều loại to ngon ngọt", Price = 180000, ImageUrl = "products28-min.jpg", StockQuantity = 100, Unit = Unit.Kg, SoldQuantity = 50, StoreId = 10, CategoryId = 2, IsActive = true, Origin = "Bắc Giang", AverageRating = 4.9m, TotalSoldQuantity = 50, ReviewCount = 18, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 16, CreatedAt = DateTime.Now },
                    new Product { Id = 29, Name = "Cánh gà", Description = "Cánh gà tươi", Price = 120000, ImageUrl = "products29-min.jpg", StockQuantity = 85, Unit = Unit.Kg, SoldQuantity = 45, StoreId = 10, CategoryId = 4, IsActive = true, Origin = "Việt Nam", AverageRating = 4.8m, TotalSoldQuantity = 45, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 3, ReviewStar5Count = 12, CreatedAt = DateTime.Now },
                    new Product { Id = 30, Name = "Đùi gà", Description = "Đùi gà tươi", Price = 90000, ImageUrl = "products30-min.jpg", StockQuantity = 120, Unit = Unit.Kg, SoldQuantity = 60, StoreId = 10, CategoryId = 4, IsActive = true, Origin = "Việt Nam", AverageRating = 4.7m, TotalSoldQuantity = 60, ReviewCount = 20, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 5, ReviewStar5Count = 14, CreatedAt = DateTime.Now }
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
                    },
                    
                    // Thêm nhiều đơn hàng hơn
                    new Order {
                        Id = 4,
                        OrderCode = "ORD004",
                        BuyerId = 9,
                        StoreId = 1,
                        TotalAmount = 180000,
                        ShippingFee = 30000,
                        TaxAmount = 18000,
                        DiscountAmount = 0,
                        FinalAmount = 228000,
                        Status = OrderStatus.Delivered,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.PaymentAfter,
                        ShippingName = "Phạm Thị D",
                        ShippingPhone = "0987654329",
                        ShippingAddress = "321 Đường GHI",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 4",
                        ShippingWard = "Phường 4",
                        CreatedAt = DateTime.Now.AddDays(-7),
                        PaidAt = DateTime.Now.AddDays(-6)
                    },
                    new Order {
                        Id = 5,
                        OrderCode = "ORD005",
                        BuyerId = 10,
                        StoreId = 2,
                        TotalAmount = 320000,
                        ShippingFee = 30000,
                        TaxAmount = 32000,
                        DiscountAmount = 0,
                        FinalAmount = 382000,
                        Status = OrderStatus.Completed,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.BankTransfer,
                        ShippingName = "Hoàng Văn E",
                        ShippingPhone = "0987654330",
                        ShippingAddress = "654 Đường JKL",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 5",
                        ShippingWard = "Phường 5",
                        CreatedAt = DateTime.Now.AddDays(-10),
                        PaidAt = DateTime.Now.AddDays(-9)
                    },
                    new Order {
                        Id = 6,
                        OrderCode = "ORD006",
                        BuyerId = 11,
                        StoreId = 3,
                        TotalAmount = 410000,
                        ShippingFee = 30000,
                        TaxAmount = 41000,
                        DiscountAmount = 0,
                        FinalAmount = 481000,
                        Status = OrderStatus.Cancelled,
                        PaymentStatus = PaymentStatus.Refunded,
                        PaymentMethod = PaymentMethodEnum.VNPay,
                        ShippingName = "Vũ Thị F",
                        ShippingPhone = "0987654331",
                        ShippingAddress = "987 Đường MNO",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 6",
                        ShippingWard = "Phường 6",
                        CreatedAt = DateTime.Now.AddDays(-15),
                        PaidAt = DateTime.Now.AddDays(-15)
                    },
                    new Order {
                        Id = 7,
                        OrderCode = "ORD007",
                        BuyerId = 12,
                        StoreId = 4,
                        TotalAmount = 280000,
                        ShippingFee = 30000,
                        TaxAmount = 28000,
                        DiscountAmount = 0,
                        FinalAmount = 338000,
                        Status = OrderStatus.Completed,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.PaymentAfter,
                        ShippingName = "Đặng Văn G",
                        ShippingPhone = "0987654332",
                        ShippingAddress = "741 Đường PQR",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 7",
                        ShippingWard = "Phường 7",
                        CreatedAt = DateTime.Now.AddDays(-8),
                        PaidAt = DateTime.Now.AddDays(-7)
                    },
                    new Order {
                        Id = 8,
                        OrderCode = "ORD008",
                        BuyerId = 18,
                        StoreId = 5,
                        TotalAmount = 190000,
                        ShippingFee = 30000,
                        TaxAmount = 19000,
                        DiscountAmount = 0,
                        FinalAmount = 239000,
                        Status = OrderStatus.Pending,
                        PaymentStatus = PaymentStatus.Unpaid,
                        PaymentMethod = PaymentMethodEnum.BankTransfer,
                        ShippingName = "Đinh Thị H",
                        ShippingPhone = "0987654338",
                        ShippingAddress = "852 Đường STU",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 8",
                        ShippingWard = "Phường 8",
                        CreatedAt = DateTime.Now.AddDays(-2)
                    },
                    new Order {
                        Id = 9,
                        OrderCode = "ORD009",
                        BuyerId = 19,
                        StoreId = 6,
                        TotalAmount = 360000,
                        ShippingFee = 30000,
                        TaxAmount = 36000,
                        DiscountAmount = 0,
                        FinalAmount = 426000,
                        Status = OrderStatus.Shipping,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.VNPay,
                        ShippingName = "Ngô Văn I",
                        ShippingPhone = "0987654339",
                        ShippingAddress = "963 Đường VWX",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 9",
                        ShippingWard = "Phường 9",
                        CreatedAt = DateTime.Now.AddDays(-4),
                        PaidAt = DateTime.Now.AddDays(-4)
                    },
                    new Order {
                        Id = 10,
                        OrderCode = "ORD010",
                        BuyerId = 20,
                        StoreId = 7,
                        TotalAmount = 270000,
                        ShippingFee = 30000,
                        TaxAmount = 27000,
                        DiscountAmount = 0,
                        FinalAmount = 327000,
                        Status = OrderStatus.Delivered,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.PaymentAfter,
                        ShippingName = "Trịnh Thị K",
                        ShippingPhone = "0987654340",
                        ShippingAddress = "147 Đường YZ",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 10",
                        ShippingWard = "Phường 10",
                        CreatedAt = DateTime.Now.AddDays(-6),
                        PaidAt = DateTime.Now.AddDays(-5)
                    },
                    new Order {
                        Id = 11,
                        OrderCode = "ORD011",
                        BuyerId = 21,
                        StoreId = 8,
                        TotalAmount = 320000,
                        ShippingFee = 30000,
                        TaxAmount = 32000,
                        DiscountAmount = 0,
                        FinalAmount = 382000,
                        Status = OrderStatus.Completed,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.BankTransfer,
                        ShippingName = "Lý Văn L",
                        ShippingPhone = "0987654341",
                        ShippingAddress = "258 Đường AB",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 11",
                        ShippingWard = "Phường 11",
                        CreatedAt = DateTime.Now.AddDays(-12),
                        PaidAt = DateTime.Now.AddDays(-11)
                    },
                    new Order {
                        Id = 12,
                        OrderCode = "ORD012",
                        BuyerId = 22,
                        StoreId = 9,
                        TotalAmount = 450000,
                        ShippingFee = 30000,
                        TaxAmount = 45000,
                        DiscountAmount = 0,
                        FinalAmount = 525000,
                        Status = OrderStatus.Pending,
                        PaymentStatus = PaymentStatus.Unpaid,
                        PaymentMethod = PaymentMethodEnum.VNPay,
                        ShippingName = "Dương Thị M",
                        ShippingPhone = "0987654342",
                        ShippingAddress = "369 Đường CD",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 12",
                        ShippingWard = "Phường 12",
                        CreatedAt = DateTime.Now.AddDays(-1)
                    },
                    new Order {
                        Id = 13,
                        OrderCode = "ORD013",
                        BuyerId = 23,
                        StoreId = 10,
                        TotalAmount = 200000,
                        ShippingFee = 30000,
                        TaxAmount = 20000,
                        DiscountAmount = 0,
                        FinalAmount = 250000,
                        Status = OrderStatus.Processing,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.PaymentAfter,
                        ShippingName = "Đỗ Văn N",
                        ShippingPhone = "0987654343",
                        ShippingAddress = "480 Đường EF",
                        ShippingProvince = "Hà Nội",
                        ShippingDistrict = "Quận Ba Đình",
                        ShippingWard = "Phường Phúc Xá",
                        CreatedAt = DateTime.Now.AddDays(-3),
                        PaidAt = DateTime.Now.AddDays(-3)
                    },
                    new Order {
                        Id = 14,
                        OrderCode = "ORD014",
                        BuyerId = 24,
                        StoreId = 1,
                        TotalAmount = 380000,
                        ShippingFee = 30000,
                        TaxAmount = 38000,
                        DiscountAmount = 0,
                        FinalAmount = 448000,
                        Status = OrderStatus.Completed,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.BankTransfer,
                        ShippingName = "Hồ Thị O",
                        ShippingPhone = "0987654344",
                        ShippingAddress = "591 Đường GH",
                        ShippingProvince = "Hà Nội",
                        ShippingDistrict = "Quận Cầu Giấy",
                        ShippingWard = "Phường Dịch Vọng",
                        CreatedAt = DateTime.Now.AddDays(-5),
                        PaidAt = DateTime.Now.AddDays(-5)
                    },
                    new Order {
                        Id = 15,
                        OrderCode = "ORD015",
                        BuyerId = 25,
                        StoreId = 2,
                        TotalAmount = 240000,
                        ShippingFee = 30000,
                        TaxAmount = 24000,
                        DiscountAmount = 0,
                        FinalAmount = 294000,
                        Status = OrderStatus.Completed,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.VNPay,
                        ShippingName = "Trương Văn P",
                        ShippingPhone = "0987654345",
                        ShippingAddress = "602 Đường IJ",
                        ShippingProvince = "Hà Nội",
                        ShippingDistrict = "Quận Hai Bà Trưng",
                        ShippingWard = "Phường Bách Khoa",
                        CreatedAt = DateTime.Now.AddDays(-9),
                        PaidAt = DateTime.Now.AddDays(-9)
                    },
                    new Order {
                        Id = 16,
                        OrderCode = "ORD016",
                        BuyerId = 6,
                        StoreId = 3,
                        TotalAmount = 290000,
                        ShippingFee = 30000,
                        TaxAmount = 29000,
                        DiscountAmount = 0,
                        FinalAmount = 349000,
                        Status = OrderStatus.Delivered,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.PaymentAfter,
                        ShippingName = "Nguyễn Văn A",
                        ShippingPhone = "0987654326",
                        ShippingAddress = "123 Đường ABC",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 1",
                        ShippingWard = "Phường 1",
                        CreatedAt = DateTime.Now.AddDays(-7),
                        PaidAt = DateTime.Now.AddDays(-6)
                    },
                    new Order {
                        Id = 17,
                        OrderCode = "ORD017",
                        BuyerId = 7,
                        StoreId = 4,
                        TotalAmount = 310000,
                        ShippingFee = 30000,
                        TaxAmount = 31000,
                        DiscountAmount = 0,
                        FinalAmount = 371000,
                        Status = OrderStatus.Cancelled,
                        PaymentStatus = PaymentStatus.Refunded,
                        PaymentMethod = PaymentMethodEnum.BankTransfer,
                        ShippingName = "Trần Thị B",
                        ShippingPhone = "0987654327",
                        ShippingAddress = "456 Đường XYZ",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 2",
                        ShippingWard = "Phường 2",
                        CreatedAt = DateTime.Now.AddDays(-14),
                        PaidAt = DateTime.Now.AddDays(-14)
                    },
                    new Order {
                        Id = 18,
                        OrderCode = "ORD018",
                        BuyerId = 8,
                        StoreId = 5,
                        TotalAmount = 420000,
                        ShippingFee = 30000,
                        TaxAmount = 42000,
                        DiscountAmount = 0,
                        FinalAmount = 492000,
                        Status = OrderStatus.Processing,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.VNPay,
                        ShippingName = "Lê Văn C",
                        ShippingPhone = "0987654328",
                        ShippingAddress = "789 Đường DEF",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 3",
                        ShippingWard = "Phường 3",
                        CreatedAt = DateTime.Now.AddDays(-2),
                        PaidAt = DateTime.Now.AddDays(-2)
                    },
                    new Order {
                        Id = 19,
                        OrderCode = "ORD019",
                        BuyerId = 9,
                        StoreId = 6,
                        TotalAmount = 180000,
                        ShippingFee = 30000,
                        TaxAmount = 18000,
                        DiscountAmount = 0,
                        FinalAmount = 228000,
                        Status = OrderStatus.Completed,
                        PaymentStatus = PaymentStatus.Paid,
                        PaymentMethod = PaymentMethodEnum.PaymentAfter,
                        ShippingName = "Phạm Thị D",
                        ShippingPhone = "0987654329",
                        ShippingAddress = "321 Đường GHI",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 4",
                        ShippingWard = "Phường 4",
                        CreatedAt = DateTime.Now.AddDays(-4),
                        PaidAt = DateTime.Now.AddDays(-4)
                    },
                    new Order {
                        Id = 20,
                        OrderCode = "ORD020",
                        BuyerId = 10,
                        StoreId = 7,
                        TotalAmount = 340000,
                        ShippingFee = 30000,
                        TaxAmount = 34000,
                        DiscountAmount = 0,
                        FinalAmount = 404000,
                        Status = OrderStatus.Pending,
                        PaymentStatus = PaymentStatus.Unpaid,
                        PaymentMethod = PaymentMethodEnum.BankTransfer,
                        ShippingName = "Hoàng Văn E",
                        ShippingPhone = "0987654330",
                        ShippingAddress = "654 Đường JKL",
                        ShippingProvince = "TP.HCM",
                        ShippingDistrict = "Quận 5",
                        ShippingWard = "Phường 5",
                        CreatedAt = DateTime.Now.AddDays(-1)
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
                    new Review { Id = 1, UserId = 6, ProductId = 1, Rating = 5, Content = "Việt quất tươi ngon, ngọt vừa phải", ImageUrl = "products1-min.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-5) },
                    new Review { Id = 2, UserId = 7, ProductId = 2, Rating = 4, Content = "Sữa tươi thơm ngon, đảm bảo chất lượng", ImageUrl = "products2-min.jpg", ShopResponse = "Chúng tôi sẽ cố gắng hơn nữa", CreatedAt = DateTime.Now.AddDays(-4) },
                    new Review { Id = 3, UserId = 8, ProductId = 3, Rating = 5, Content = "Sữa hạnh nhân rất thơm, vị béo nhẹ", ImageUrl = "products3-min.jpg", ShopResponse = "Cảm ơn bạn", CreatedAt = DateTime.Now.AddDays(-3) },
                    new Review { Id = 4, UserId = 9, ProductId = 4, Rating = 4, Content = "Súp lơ xanh tươi, giòn ngon", ImageUrl = "products4-min.jpg", ShopResponse = "Rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-2) },
                    new Review { Id = 5, UserId = 10, ProductId = 5, Rating = 5, Content = "Súp lơ tươi hữu cơ, không thuốc trừ sâu", ImageUrl = "products5-min.jpg", ShopResponse = "Cảm ơn bạn đã tin tưởng", CreatedAt = DateTime.Now.AddDays(-1) },
                    new Review { Id = 6, UserId = 11, ProductId = 6, Rating = 4, Content = "Súp lơ nhỏ tươi ngon, giá hợp lý", ImageUrl = "products6-min.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-6) },
                    new Review { Id = 7, UserId = 12, ProductId = 7, Rating = 5, Content = "Nho xanh không hạt rất ngọt, chất lượng", ImageUrl = "products7-min.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-7) },
                    new Review { Id = 8, UserId = 6, ProductId = 8, Rating = 4, Content = "Vải thiều Bắc Giang tươi ngon, ngọt đậm đà", ImageUrl = "products8-min.jpg", ShopResponse = "Chúng tôi sẽ tiếp tục cải thiện", CreatedAt = DateTime.Now.AddDays(-8) },
                    new Review { Id = 9, UserId = 7, ProductId = 9, Rating = 5, Content = "Thịt bò tươi ngon, màu đỏ tự nhiên", ImageUrl = "products9-min.jpg", ShopResponse = "Cảm ơn bạn", CreatedAt = DateTime.Now.AddDays(-9) },
                    new Review { Id = 10, UserId = 8, ProductId = 10, Rating = 4, Content = "Sườn bò tươi ngon, thịt mềm", ImageUrl = "products10-min.jpg", ShopResponse = "Rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-10) },
                    
                    // Thêm nhiều đánh giá khác
                    new Review { Id = 11, UserId = 9, ProductId = 11, Rating = 5, Content = "Kiwi xanh New Zealand ngọt, chua nhẹ rất ngon", ImageUrl = "products11-min.jpg", ShopResponse = "Cảm ơn bạn đã tin tưởng sản phẩm của chúng tôi", CreatedAt = DateTime.Now.AddDays(-11) },
                    new Review { Id = 12, UserId = 10, ProductId = 12, Rating = 5, Content = "Dâu tây Đà Lạt cực ngọt, màu đẹp", ImageUrl = "products12-min.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-12) },
                    new Review { Id = 13, UserId = 11, ProductId = 13, Rating = 4, Content = "Măng cụt ngọt, múi to và dày", ImageUrl = "products13-min.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-13) },
                    new Review { Id = 14, UserId = 12, ProductId = 14, Rating = 5, Content = "Thanh long ruột đỏ ngọt, thơm", ImageUrl = "products14-min.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-14) },
                    new Review { Id = 15, UserId = 18, ProductId = 15, Rating = 4, Content = "Gừng tươi thơm nồng, chất lượng", ImageUrl = "products15-min.jpg", ShopResponse = "Cảm ơn bạn đã đánh giá", CreatedAt = DateTime.Now.AddDays(-15) },
                    new Review { Id = 16, UserId = 19, ProductId = 16, Rating = 5, Content = "Susu tươi ngon từ Sapa, chất lượng tuyệt vời", ImageUrl = "products16-min.jpg", ShopResponse = "Cảm ơn bạn đã tin tưởng", CreatedAt = DateTime.Now.AddDays(-16) },
                    new Review { Id = 17, UserId = 20, ProductId = 17, Rating = 5, Content = "Chanh tươi không hạt, vị chua đậm đà", ImageUrl = "products17-min.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-17) },
                    new Review { Id = 18, UserId = 21, ProductId = 18, Rating = 4, Content = "Chanh vàng nhập khẩu thơm, vỏ mỏng", ImageUrl = "products18-min.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-18) },
                    new Review { Id = 19, UserId = 22, ProductId = 19, Rating = 5, Content = "Chuối tiêu chín tự nhiên, ngọt thơm", ImageUrl = "products19-min.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-19) },
                    new Review { Id = 20, UserId = 23, ProductId = 20, Rating = 4, Content = "Thịt bò xay tươi ngon, không lẫn gân", ImageUrl = "products20-min.jpg", ShopResponse = "Cảm ơn bạn đã đánh giá", CreatedAt = DateTime.Now.AddDays(-20) },
                    new Review { Id = 21, UserId = 24, ProductId = 21, Rating = 5, Content = "Nho tím không hạt ngọt, quả to đều", ImageUrl = "products21-min.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-21) },
                    new Review { Id = 22, UserId = 25, ProductId = 22, Rating = 4, Content = "Đào tươi mọng nước, thơm ngọt", ImageUrl = "products22-min.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-22) },
                    new Review { Id = 23, UserId = 6, ProductId = 23, Rating = 4, Content = "Súp lơ trắng tươi ngon, giòn ngọt", ImageUrl = "products23-min.jpg", ShopResponse = "Chúng tôi sẽ tiếp tục duy trì chất lượng", CreatedAt = DateTime.Now.AddDays(-23) },
                    new Review { Id = 24, UserId = 7, ProductId = 24, Rating = 5, Content = "Súp lơ trắng hữu cơ, không thuốc trừ sâu", ImageUrl = "products24-min.jpg", ShopResponse = "Cảm ơn bạn đã tin tưởng", CreatedAt = DateTime.Now.AddDays(-24) },
                    new Review { Id = 25, UserId = 8, ProductId = 25, Rating = 5, Content = "Súp lơ trắng tươi ngon size lớn, chất lượng", ImageUrl = "products25-min.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-25) },
                    new Review { Id = 26, UserId = 9, ProductId = 26, Rating = 4, Content = "Thịt viên tươi ngon, vị đậm đà", ImageUrl = "products26-min.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-26) },
                    new Review { Id = 27, UserId = 10, ProductId = 27, Rating = 4, Content = "Thanh long ruột trắng ngọt, giòn", ImageUrl = "products27-min.jpg", ShopResponse = "Chúng tôi sẽ tiếp tục duy trì chất lượng", CreatedAt = DateTime.Now.AddDays(-27) },
                    new Review { Id = 28, UserId = 11, ProductId = 28, Rating = 5, Content = "Vải thiều loại to ngon ngọt, thơm", ImageUrl = "products28-min.jpg", ShopResponse = "Cảm ơn bạn đã tin tưởng", CreatedAt = DateTime.Now.AddDays(-28) },
                    new Review { Id = 29, UserId = 12, ProductId = 29, Rating = 5, Content = "Cánh gà tươi, thịt dai ngon", ImageUrl = "products29-min.jpg", ShopResponse = "Chúng tôi rất vui khi bạn hài lòng", CreatedAt = DateTime.Now.AddDays(-29) },
                    new Review { Id = 30, UserId = 18, ProductId = 30, Rating = 4, Content = "Đùi gà tươi, thịt mềm ngọt", ImageUrl = "products30-min.jpg", ShopResponse = "Cảm ơn bạn đã ủng hộ", CreatedAt = DateTime.Now.AddDays(-30) }
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
                    new Transaction { Id = 1, OrderId = 1, BuyerId = 1, Amount = 150000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-5) },
                    new Transaction { Id = 2, OrderId = 2, BuyerId = 1, Amount = 250000, Status = TransactionStatus.Pending, PaymentMethod = PaymentMethodEnum.BankTransfer, CreatedAt = DateTime.Now.AddDays(-3) },
                    new Transaction { Id = 3, OrderId = 3, BuyerId = 1, Amount = 350000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.VNPay, CreatedAt = DateTime.Now.AddDays(-1) },
                    new Transaction { Id = 4, OrderId = 4, BuyerId = 1, Amount = 200000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-2) },
                    new Transaction { Id = 5, OrderId = 5, BuyerId = 1, Amount = 180000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.BankTransfer, CreatedAt = DateTime.Now.AddDays(-4) },
                    new Transaction { Id = 6, OrderId = 6, BuyerId = 1, Amount = 300000, Status = TransactionStatus.Cancelled, PaymentMethod = PaymentMethodEnum.VNPay, CreatedAt = DateTime.Now.AddDays(-6) },
                    new Transaction { Id = 7, OrderId = 7, BuyerId = 1, Amount = 220000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-7) },
                    new Transaction { Id = 8, OrderId = 8, BuyerId = 1, Amount = 280000, Status = TransactionStatus.Pending, PaymentMethod = PaymentMethodEnum.BankTransfer, CreatedAt = DateTime.Now.AddDays(-8) },
                    new Transaction { Id = 9, OrderId = 9, BuyerId = 1, Amount = 190000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.VNPay, CreatedAt = DateTime.Now.AddDays(-9) },
                    new Transaction { Id = 10, OrderId = 10, BuyerId = 1, Amount = 240000, Status = TransactionStatus.Success, PaymentMethod = PaymentMethodEnum.PaymentAfter, CreatedAt = DateTime.Now.AddDays(-10) }
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
        private static async Task SeedOrderTimelinesAsync(VNFarmContext context)
        {
            if (!context.OrderTimelines.Any())
            {
                var timelines = new List<OrderTimeline>();

                for (int i = 1; i <= 20; i++) // Có 20 đơn hàng
                {
                    timelines.AddRange(new List<OrderTimeline>
            {
                new OrderTimeline
                {
                    OrderId = i,
                    EventType = OrderEventType.OrderCreated,
                    Status = OrderTimelineStatus.Completed,
                    Description = "Đơn hàng được tạo tự động",
                    CreatedAt = DateTime.Now.AddDays(-i)
                },
                new OrderTimeline
                {
                    OrderId = i,
                    EventType = OrderEventType.OrderAcceptedBySeller,
                    Status = OrderTimelineStatus.Completed,
                    Description = "Người bán đã xác nhận đơn hàng",
                    CreatedAt = DateTime.Now.AddDays(-i).AddHours(2)
                },
                new OrderTimeline
                {
                    OrderId = i,
                    EventType = OrderEventType.OrderPackaging,
                    Status = OrderTimelineStatus.Completed,
                    Description = "Đơn hàng đang được đóng gói",
                    CreatedAt = DateTime.Now.AddDays(-i).AddHours(5)
                },
                new OrderTimeline
                {
                    OrderId = i,
                    EventType = OrderEventType.OrderReadyToShip,
                    Status = OrderTimelineStatus.Completed,
                    Description = "Đơn hàng sẵn sàng để giao",
                    CreatedAt = DateTime.Now.AddDays(-i).AddHours(10)
                },
                new OrderTimeline
                {
                    OrderId = i,
                    EventType = OrderEventType.OrderShipped,
                    Status = OrderTimelineStatus.Completed,
                    Description = "Đơn hàng đã được giao cho đơn vị vận chuyển",
                    CreatedAt = DateTime.Now.AddDays(-i).AddHours(15)
                },
                new OrderTimeline
                {
                    OrderId = i,
                    EventType = OrderEventType.OrderCompleted,
                    Status = OrderTimelineStatus.Completed,
                    Description = "Đơn hàng đã hoàn tất",
                    CreatedAt = DateTime.Now.AddDays(-i).AddHours(20)
                },
            });
                }

                await context.OrderTimelines.AddRangeAsync(timelines);
                await context.SaveChangesAsync();
            }
        }

    }
}