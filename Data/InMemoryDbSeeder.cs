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

                await SeedUsersAsync(context);
                await SeedCategoriesAsync(context);
                await SeedStoresAsync(context);
                await SeedProductsAsync(context);
                await SeedOrdersAsync(context);
                await SeedReviewsAsync(context);
                await SeedChatRoomsAsync(context);
                await SeedChatsAsync(context);
                await SeedNotificationsAsync(context);
                // await SeedOrderTimelinesAsync(context);
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
                    new Store { Id = 1, Name = "Fruit Paradise", Description = "Chuyên cung cấp trái cây tươi", Address = "Tiền Giang", PhoneNumber = "0987654324", Email = "fruit@vnfarm.com", LogoUrl = "/images/stores/fruit.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = (float)4.7, ReviewCount = 120, UserId = 2, CreatedAt = DateTime.Now },
                    new Store { Id = 2, Name = "Admin Store", Description = "Cửa hàng quản trị viên", Address = "Hà Nội", PhoneNumber = "0987654321", Email = "admin@vnfarm.com", LogoUrl = "/images/stores/admin.png", IsActive = true, VerificationStatus = StoreStatus.Verified, AverageRating = (float)4.8, ReviewCount = 150, UserId = 1, CreatedAt = DateTime.Now }
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
                    new Product { Id = 1, Name = "Việt quất", Description = "Việt quất tươi ngon, giàu chất chống oxy hóa", Price = 25000, ImageUrl = "products1-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 50, StoreId = 1, CategoryId = 2, IsActive = false, Origin = "Đà Lạt", AverageRating = (float)4.5, TotalSoldQuantity = 50, ReviewCount = 10, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 3, ReviewStar5Count = 6, CreatedAt = DateTime.Now },
                    new Product { Id = 2, Name = "Sữa tươi", Description = "Sữa tươi thanh trùng nguyên chất", Price = 15000, ImageUrl = "products2-min.jpg", StockQuantity = 200, Unit = Unit.Bag, SoldQuantity = 100, StoreId = 1, CategoryId = 6, IsActive = true, Origin = "Việt Nam", AverageRating = (float)4.3, TotalSoldQuantity = 100, ReviewCount = 15, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 2, ReviewStar4Count = 7, ReviewStar5Count = 5, CreatedAt = DateTime.Now },
                    new Product { Id = 3, Name = "Sữa hạnh nhân", Description = "Sữa hạnh nhân hữu cơ, không đường", Price = 20000, ImageUrl = "products3-min.jpg", StockQuantity = 150, Unit = Unit.Bag, SoldQuantity = 75, StoreId = 1, CategoryId = 6, IsActive = true, Origin = "Mỹ", AverageRating = (float)4.4, TotalSoldQuantity = 75, ReviewCount = 12, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 2, ReviewStar4Count = 5, ReviewStar5Count = 5, CreatedAt = DateTime.Now },
                    
                    // Sản phẩm của cửa hàng Admin Store (StoreId = 2, UserId = 1)
                    new Product { Id = 4, Name = "Súp lơ", Description = "Súp lơ xanh tươi ngon", Price = 80000, ImageUrl = "products4-min.jpg", StockQuantity = 80, Unit = Unit.Kg, SoldQuantity = 30, StoreId = 2, CategoryId = 1, IsActive = false, Origin = "Đà Lạt", AverageRating = (float)4.8, TotalSoldQuantity = 30, ReviewCount = 8, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 6, CreatedAt = DateTime.Now },
                    new Product { Id = 5, Name = "Súp lơ", Description = "Súp lơ tươi hữu cơ", Price = 45000, ImageUrl = "products5-min.jpg", StockQuantity = 120, Unit = Unit.Kg, SoldQuantity = 60, StoreId = 2, CategoryId = 1, IsActive = true, Origin = "Đà Lạt", AverageRating = (float)4.6, TotalSoldQuantity = 60, ReviewCount = 20, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 5, ReviewStar5Count = 13, CreatedAt = DateTime.Now },
                    
                    // Thêm các sản phẩm của Fruit Paradise
                    new Product { Id = 6, Name = "Thanh long", Description = "Thanh long ruột đỏ Bình Thuận", Price = 90000, ImageUrl = "products14-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 55, StoreId = 1, CategoryId = 2, IsActive = true, Origin = "Bình Thuận", AverageRating = (float)4.9, TotalSoldQuantity = 55, ReviewCount = 22, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 2, ReviewStar5Count = 20, CreatedAt = DateTime.Now },
                    new Product { Id = 7, Name = "Nho", Description = "Nho xanh không hạt", Price = 35000, ImageUrl = "products7-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 100, StoreId = 1, CategoryId = 2, IsActive = false, Origin = "Ninh Thuận", AverageRating = (float)5.0, TotalSoldQuantity = 100, ReviewCount = 25, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 0, ReviewStar4Count = 0, ReviewStar5Count = 25, CreatedAt = DateTime.Now },
                    new Product { Id = 8, Name = "Vải thiều", Description = "Vải thiều tươi ngon từ Bắc Giang", Price = 25000, ImageUrl = "products8-min.jpg", StockQuantity = 150, Unit = Unit.Kg, SoldQuantity = 75, StoreId = 1, CategoryId = 2, IsActive = true, Origin = "Bắc Giang", AverageRating = (float)4.5, TotalSoldQuantity = 75, ReviewCount = 18, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 7, ReviewStar5Count = 9, CreatedAt = DateTime.Now },
                    
                    // Thêm các sản phẩm của Admin Store
                    new Product { Id = 9, Name = "Thịt bò", Description = "Thịt bò tươi ngon, thăn ngoại", Price = 30000, ImageUrl = "products9-min.jpg", StockQuantity = 180, Unit = Unit.Kg, SoldQuantity = 90, StoreId = 2, CategoryId = 4, IsActive = false, Origin = "Việt Nam", AverageRating = (float)4.6, TotalSoldQuantity = 90, ReviewCount = 22, ReviewStar1Count = 0, ReviewStar2Count = 1, ReviewStar3Count = 1, ReviewStar4Count = 6, ReviewStar5Count = 14, CreatedAt = DateTime.Now },
                    new Product { Id = 10, Name = "Sườn bò", Description = "Sườn bò tươi ngon", Price = 120000, ImageUrl = "products10-min.jpg", StockQuantity = 0, Unit = Unit.Kg, SoldQuantity = 25, StoreId = 2, CategoryId = 4, IsActive = true, Origin = "Việt Nam", AverageRating = (float)4.7, TotalSoldQuantity = 25, ReviewCount = 12, ReviewStar1Count = 0, ReviewStar2Count = 0, ReviewStar3Count = 1, ReviewStar4Count = 2, ReviewStar5Count = 9, CreatedAt = DateTime.Now }
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