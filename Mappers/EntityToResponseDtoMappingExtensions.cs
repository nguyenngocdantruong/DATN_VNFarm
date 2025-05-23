using VNFarm.DTOs.Response;
using VNFarm.Entities;

namespace VNFarm.Mappers
{
    public static class EntityToResponseDtoMappingExtensions
    {
        public static AddressResponseDTO ToAddressResponseDTO(this Order order)
        {
            return new AddressResponseDTO
            {
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderId = order.Id,
                ShippingName = order.ShippingName,
                ShippingPhone = order.ShippingPhone,
                ShippingAddress = order.ShippingAddress,
                ShippingProvince = order.ShippingProvince,
                ShippingDistrict = order.ShippingDistrict,
                ShippingWard = order.ShippingWard,
            };
        }
        //public static BusinessRegistrationResponseDTO ToResponseDTO(this BusinessRegistration businessRegistration)
        //{
        //    return new BusinessRegistrationResponseDTO
        //    {
        //        Id = businessRegistration.Id,
        //        CreatedAt = businessRegistration.CreatedAt,
        //        UpdatedAt = businessRegistration.UpdatedAt,
        //        UserId = businessRegistration.UserId,
        //        BusinessName = businessRegistration.BusinessName,
        //        TaxCode = businessRegistration.TaxCode,
        //        BusinessLicenseUrl = businessRegistration.BusinessLicenseUrl,
        //        Address = businessRegistration.Address,
        //        Notes = businessRegistration.Notes,
        //        RegistrationStatus = businessRegistration.RegistrationStatus,
        //        BusinessType = businessRegistration.BusinessType,
        //        //Navigation Properties
        //        User = businessRegistration.User?.ToResponseDTO(),
        //        ApprovalResults = businessRegistration.ApprovalResults?.Select(ar => ar.ToResponseDTO()).ToList(),
        //    };
        //}
        public static CategoryResponseDTO ToResponseDTO(this Category category)
        {
            return new CategoryResponseDTO
            {
                Id = category.Id,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                Name = category.Name,
                Description = category.Description,
                IconUrl = category.IconUrl,
                ProductCount = category.Products.Count,
                MinPrice = category.MinPrice,
                MaxPrice = category.MaxPrice,
            };
        }
        public static ChatResponseDTO ToResponseDTO(this Chat chat)
        {
            return new ChatResponseDTO
            {
                Id = chat.Id,
                CreatedAt = chat.CreatedAt,
                UpdatedAt = chat.UpdatedAt,
                ChatRoomId = chat.ChatRoomId,
                SenderId = chat.SenderId,
                Content = chat.Content,
                Type = chat.Type,
                ImageUrl = chat.ImageUrl,
            };
        }
        public static ChatRoomResponseDTO ToResponseDTO(this ChatRoom chatRoom)
        {
            return new ChatRoomResponseDTO
            {
                Id = chatRoom.Id,
                CreatedAt = chatRoom.CreatedAt,
                UpdatedAt = chatRoom.UpdatedAt,
                NameRoom = chatRoom.NameRoom,
                Description = chatRoom.Description,
                Status = chatRoom.Status,
                Type = chatRoom.Type,
                LastMessage = chatRoom.LastMessage,
                LastMessageTime = chatRoom.LastMessageTime,
                IsActive = chatRoom.IsActive,
                BuyerId = chatRoom.BuyerId,
                SellerId = chatRoom.SellerId,
                OrderId = chatRoom.OrderId,
                Buyer = chatRoom.Buyer?.ToResponseDTO(),
                Seller = chatRoom.Seller?.ToResponseDTO(),
            };
        }
        public static DiscountResponseDTO ToResponseDTO(this Discount discount)
        {
            return new DiscountResponseDTO
            {
                Id = discount.Id,
                CreatedAt = discount.CreatedAt,
                UpdatedAt = discount.UpdatedAt,
                Code = discount.Code,
                Description = discount.Description,
                RemainingQuantity = discount.RemainingQuantity,
                Status = discount.Status,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                Type = discount.Type,
                StoreId = discount.StoreId,
                UserId = discount.UserId,
                DiscountAmount = discount.DiscountAmount,
                MinimumOrderAmount = discount.MinimumOrderAmount,
                MaximumDiscountAmount = discount.MaximumDiscountAmount,
            };
        }
        public static NotificationResponseDTO ToResponseDTO(this Notification notification)
        {
            return new NotificationResponseDTO
            {
                Id = notification.Id,
                CreatedAt = notification.CreatedAt,
                UpdatedAt = notification.UpdatedAt,
                UserId = notification.UserId,
                Content = notification.Content,
                Type = notification.Type,
                IsRead = notification.IsRead,
            };
        }
       
        public static OrderResponseDTO ToResponseDTO(this Order entity)
        {
            if (entity == null) return null;

            return new OrderResponseDTO
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                OrderCode = entity.OrderCode,
                Status = entity.Status,
                Notes = entity.Notes,
                TotalAmount = entity.TotalAmount,
                ShippingFee = entity.ShippingFee,
                TaxAmount = entity.TaxAmount,
                DiscountAmount = entity.DiscountAmount,
                FinalAmount = entity.FinalAmount,
                PaymentStatus = entity.PaymentStatus,
                PaymentMethod = entity.PaymentMethod,
                PaidAt = entity.PaidAt,
                BuyerId = entity.BuyerId,
                DiscountId = entity.DiscountId,
                Address = new AddressResponseDTO
                {
                    OrderId = entity.Id,
                    ShippingName = entity.ShippingName,
                    ShippingPhone = entity.ShippingPhone,
                    ShippingAddress = entity.ShippingAddress,
                    ShippingProvince = entity.ShippingProvince,
                    ShippingDistrict = entity.ShippingDistrict,
                    ShippingWard = entity.ShippingWard
                },
                Shipping = new ShippingResponseDTO
                {
                    OrderId = entity.Id,
                    TrackingNumber = entity.TrackingNumber,
                    ShippingMethod = entity.ShippingMethod,
                    ShippingPartner = entity.ShippingPartner,
                    ShippedAt = entity.ShippedAt,
                    DeliveredAt = entity.DeliveredAt
                },
                OrderItems = entity.OrderItems?.Select(od => od.ToResponseDTO()).ToList() ?? []
            };
        }
        public static OrderTimelineResponseDTO ToResponseDTO(this OrderTimeline orderTimeline)
        {
            return new OrderTimelineResponseDTO
            {
                Id = orderTimeline.Id,
                CreatedAt = orderTimeline.CreatedAt,
                UpdatedAt = orderTimeline.UpdatedAt,
                OrderId = orderTimeline.OrderId,
                EventType = orderTimeline.EventType,
                Status = orderTimeline.Status,
                Description = orderTimeline.Description,
            };
        }
        
        public static ProductResponseDTO ToResponseDTO(this Product product)
        {
            return new ProductResponseDTO
            {
                Id = product.Id,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SoldQuantity = product.SoldQuantity,
                Unit = product.Unit,
                StoreId = product.StoreId,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                Origin = product.Origin,
                TotalSoldQuantity = product.TotalSoldQuantity,
                ImageUrl = product.ImageUrl,
                AverageRating = product.AverageRating,
                Category = product.Category?.ToResponseDTO(),
                ReviewStar1Count = product.ReviewStar1Count,
                ReviewStar2Count = product.ReviewStar2Count,
                ReviewStar3Count = product.ReviewStar3Count,
                ReviewStar4Count = product.ReviewStar4Count,
                ReviewStar5Count = product.ReviewStar5Count,
            };
        }
        
        public static ReviewResponseDTO ToResponseDTO(this Review review)
        {
            return new ReviewResponseDTO
            {
                Id = review.Id,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                UserId = review.UserId,
                ProductId = review.ProductId,
                Rating = review.Rating,
                Content = review.Content,
                ShopResponse = review.ShopResponse,
                ImageUrl = review.ImageUrl,
            };
        }
        public static ShippingResponseDTO ToShippingResponseDTO(this Order order)
        {
            return new ShippingResponseDTO
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderId = order.Id,
                TrackingNumber = order.TrackingNumber,
                ShippingMethod = order.ShippingMethod,
                ShippingPartner = order.ShippingPartner,
                ShippedAt = order.ShippedAt,
                DeliveredAt = order.DeliveredAt,
                CancelledAt = order.CancelledAt,
            };
        }
        public static StoreResponseDTO ToResponseDTO(this Store store)
        {
            return new StoreResponseDTO
            {
                Id = store.Id,
                CreatedAt = store.CreatedAt,
                UpdatedAt = store.UpdatedAt,
                Name = store.Name,
                Description = store.Description,
                LogoUrl = store.LogoUrl,
                Address = store.Address,
                PhoneNumber = store.PhoneNumber,
                Email = store.Email,
                BusinessType = store.BusinessType,
                VerificationStatus = store.VerificationStatus,
                IsActive = store.IsActive,
                AverageRating = store.AverageRating,
                ReviewCount = store.ReviewCount,
                OwnerId = store.UserId,
                //Navigation properties
                Owner = store.User?.ToResponseDTO(),
            };
        }
        
        public static UserResponseDTO ToResponseDTO(this User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ImageUrl = user.ImageUrl,
                Role = user.Role,
                IsActive = user.IsActive,
                EmailVerified = user.EmailVerified,
                EmailNotificationsEnabled = user.EmailNotificationsEnabled,
                OrderStatusNotificationsEnabled = user.OrderStatusNotificationsEnabled,
                DiscountNotificationsEnabled = user.DiscountNotificationsEnabled,
                AdminNotificationsEnabled = user.AdminNotificationsEnabled,
            };
        }
        public static CartResponseDTO ToResponseDTO(this Cart cart)
        {
            return new CartResponseDTO
            {
                Id = cart.Id,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                UserId = cart.UserId,
                User = cart.User?.ToResponseDTO(),
                ShopCarts = cart.ShopCarts?.Select(sc => sc.ToResponseDTO()).ToList()
            };
        }
        public static ShopCartResponseDTO ToResponseDTO(this ShopCart shopCart)
        {
            return new ShopCartResponseDTO
            {
                Id = shopCart.Id,
                CreatedAt = shopCart.CreatedAt,
                UpdatedAt = shopCart.UpdatedAt,
                ShopId = shopCart.ShopId,
                CartId = shopCart.CartId,
                Shop = shopCart.Shop?.ToResponseDTO(),
                CartItems = shopCart.CartItems?.Select(ci => ci.ToResponseDTO()).ToList()
            };
        }
        public static CartItemResponseDTO ToResponseDTO(this CartItem cartItem)
        {
            return new CartItemResponseDTO
            {
                Id = cartItem.Id,
                CreatedAt = cartItem.CreatedAt,
                UpdatedAt = cartItem.UpdatedAt,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                ShopCartId = cartItem.ShopCartId,
                Product = cartItem.Product?.ToResponseDTO()
            };
        }
        public static OrderItemResponseDTO ToResponseDTO(this OrderItem entity)
        {
            if (entity == null) return null;
            
            return new OrderItemResponseDTO
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                OrderId = entity.OrderId,
                ProductId = entity.ProductId,
                Quantity = entity.Quantity,
                Unit = entity.Unit,
                UnitPrice = entity.UnitPrice,
                ShippingFee = entity.ShippingFee,
                TaxAmount = entity.TaxAmount,
                Subtotal = entity.Subtotal,
                PackagingStatus = entity.PackagingStatus,
                ShopId = entity.ShopId,
                Product = entity.Product?.ToResponseDTO(),
                Shop = entity.Shop?.ToResponseDTO()
            };
        }
    }
}
