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
        public static BusinessRegistrationResponseDTO ToResponseDTO(this BusinessRegistration businessRegistration)
        {
            return new BusinessRegistrationResponseDTO
            {
                Id = businessRegistration.Id,
                CreatedAt = businessRegistration.CreatedAt,
                UpdatedAt = businessRegistration.UpdatedAt,
                UserId = businessRegistration.UserId,
                BusinessName = businessRegistration.BusinessName,
                TaxCode = businessRegistration.TaxCode,
                BusinessLicenseUrl = businessRegistration.BusinessLicenseUrl,
                Address = businessRegistration.Address,
                Notes = businessRegistration.Notes,
                RegistrationStatus = businessRegistration.RegistrationStatus,
                BusinessType = businessRegistration.BusinessType,
                //Navigation Properties
                User = businessRegistration.User?.ToResponseDTO(),
                ApprovalResults = businessRegistration.ApprovalResults?.Select(ar => ar.ToResponseDTO()).ToList(),
            };
        }
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
        public static OrderDetailResponseDTO ToResponseDTO(this OrderDetail orderDetail)
        {
            return new OrderDetailResponseDTO
            {
                Id = orderDetail.Id,
                CreatedAt = orderDetail.CreatedAt,
                UpdatedAt = orderDetail.UpdatedAt,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                UnitPrice = orderDetail.UnitPrice,
                ShippingFee = orderDetail.ShippingFee,
                TaxAmount = orderDetail.TaxAmount,
                Subtotal = orderDetail.Subtotal,
                PackagingStatus = orderDetail.PackagingStatus,
                Unit = orderDetail.Unit,
                ImageUrl = orderDetail.Product?.ImageUrl,
            };
        }
        public static OrderResponseDTO ToResponseDTO(this Order order)
        {
            var orderResponseDto =  new OrderResponseDTO
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderCode = order.OrderCode,
                Status = order.Status,
                Notes = order.Notes,
                TotalAmount = order.TotalAmount,
                ShippingFee = order.ShippingFee,
                TaxAmount = order.TaxAmount,
                DiscountAmount = order.DiscountAmount,
                FinalAmount = order.FinalAmount,
                PaymentStatus = order.PaymentStatus,
                PaymentMethod = order.PaymentMethod,
                PaidAt = order.PaidAt,
                BuyerId = order.BuyerId,
                StoreId = order.StoreId,
                DiscountId = order.DiscountId,
                //Navigation properties
                Buyer = order.Buyer?.ToResponseDTO(),
                Store = order.Store?.ToResponseDTO(),
                Discount = order.Discount?.ToResponseDTO(),
                OrderDetails = order.OrderDetails?.Select(od => od.ToResponseDTO()).ToList(),
                OrderTimelines = order.OrderTimelines?.Select(ot => ot.ToResponseDTO()).ToList(),
                Shipping = order.ToShippingResponseDTO(),
                Address = order.ToAddressResponseDTO(),
            };
            return orderResponseDto;
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
        public static PaymentMethodResponseDTO ToResponseDTO(this PaymentMethod paymentMethod)
        {
            return new PaymentMethodResponseDTO
            {
                Id = paymentMethod.Id,
                CreatedAt = paymentMethod.CreatedAt,
                UpdatedAt = paymentMethod.UpdatedAt,
                CardName = paymentMethod.CardName,
                PaymentType = paymentMethod.PaymentType,
                AccountNumber = paymentMethod.AccountNumber,
                AccountHolderName = paymentMethod.AccountHolderName,
                BankName = paymentMethod.BankName,
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
                Store = product.Store?.ToResponseDTO(),
                Reviews = product.Reviews?.Select(r => r.ToResponseDTO()).ToList(),
                ReviewStar1Count = product.ReviewStar1Count,
                ReviewStar2Count = product.ReviewStar2Count,
                ReviewStar3Count = product.ReviewStar3Count,
                ReviewStar4Count = product.ReviewStar4Count,
                ReviewStar5Count = product.ReviewStar5Count,
            };
        }
        public static RegistrationApprovalResultResponseDTO ToResponseDTO(this RegistrationApprovalResult registrationApprovalResult)
        {
            return new RegistrationApprovalResultResponseDTO
            {
                Id = registrationApprovalResult.Id,
                CreatedAt = registrationApprovalResult.CreatedAt,
                UpdatedAt = registrationApprovalResult.UpdatedAt,
                RegistrationId = registrationApprovalResult.RegistrationId,
                AdminId = registrationApprovalResult.AdminId,
                ApprovalResult = registrationApprovalResult.ApprovalResult,
                Note = registrationApprovalResult.Note,
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
                Products = store.Products?.Select(p => p.ToResponseDTO()).ToList()
            };
        }
        public static TransactionResponseDTO ToResponseDTO(this Transaction transaction)
        {
            return new TransactionResponseDTO
            {
                Id = transaction.Id,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt,
                TransactionCode = transaction.TransactionCode,
                OrderId = transaction.OrderId,
                BuyerId = transaction.BuyerId,
                Amount = transaction.Amount,
                Details = transaction.Details,
                PaymentMethod = transaction.PaymentMethod,
                Status = transaction.Status,
                PaymentDueDate = transaction.PaymentDueDate,
                PaymentDate = transaction.PaymentDate,
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
    }
}
