using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;

namespace VNFarm.Mappers
{
    public static class RequestDtoToEntityMappingExtensions
    {
        public static BusinessRegistration ToEntity(this BusinessRegistrationRequestDTO requestDto)
        {
            return new BusinessRegistration
            {
                UserId = requestDto.UserId,
                BusinessName = requestDto.BusinessName,
                BusinessType = requestDto.BusinessType,
                TaxCode = requestDto.TaxCode,
                BusinessLicenseUrl = requestDto.BusinessLicenseFile != null ? requestDto.BusinessLicenseUrl: "",
                Address = requestDto.Address,
                Notes = "",
                RegistrationStatus = RegistrationStatus.Pending,
            };
        }
        public static Category ToEntity(this CategoryRequestDTO requestDto)
        {
            return new Category
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                IconUrl = requestDto.IconFile != null ? requestDto.IconUrl : "",
                MinPrice = 0,
                MaxPrice = 0,
            };
        }
        public static Chat ToEntity(this ChatRequestDTO requestDto)
        {
            return new Chat
            {
                ChatRoomId = requestDto.ChatRoomId,
                SenderId = requestDto.SenderId,
                Content = requestDto.Content,
                ImageUrl = requestDto.ImageFile != null ? requestDto.ImageUrl : "",
                Type = requestDto.ImageFile != null ? ChatMessageType.Image : ChatMessageType.Text,
            };
        }
        public static ChatRoom ToEntity(this ChatRoomRequestDTO requestDto)
        {
            return new ChatRoom
            {
                NameRoom = requestDto.NameRoom,
                Description = requestDto.Description,
                Status = ChatRoomStatus.InProgress,
                Type = requestDto.Type,
                BuyerId = requestDto.BuyerId,
                SellerId = requestDto.SellerId,
                OrderId = requestDto.OrderId,
                LastMessage = "Phòng chat đã được tạo",
                LastMessageTime = DateTime.Now,
                IsActive = true,
            };
        }
        public static Discount ToEntity(this DiscountRequestDTO requestDto)
        {
            return new Discount
            {
                Code = requestDto.Code,
                Description = requestDto.Description,
                RemainingQuantity = requestDto.RemainingQuantity,
                Status = requestDto.Status,
                StartDate = requestDto.StartDate,
                EndDate = requestDto.EndDate,
                Type = requestDto.Type,
                DiscountAmount = requestDto.DiscountAmount,
                MinimumOrderAmount = requestDto.MinimumOrderAmount,
                MaximumDiscountAmount = requestDto.MaximumDiscountAmount,
                StoreId = requestDto.StoreId,
                UserId = requestDto.UserId,
            };
        }
        public static Notification ToEntity(this NotificationRequestDTO requestDto)
        {
            return new Notification
            {
                UserId = requestDto.UserId,
                Content = requestDto.Content,
                LinkUrl = requestDto.LinkUrl,
                Type = requestDto.Type,
                IsRead = false
            };
        }
        public static OrderDetail ToEntity(this OrderDetailRequestDTO requestDto, ProductResponseDTO productResponseDTO)
        {
            return new OrderDetail
            {
                OrderId = requestDto.OrderId,
                ProductId = requestDto.ProductId,
                Quantity = requestDto.Quantity,
                UnitPrice = productResponseDTO.Price,
                ShippingFee = 20000,
                TaxAmount = productResponseDTO.Price * 0.1M,
                Subtotal = productResponseDTO.Price * requestDto.Quantity + 20000 + productResponseDTO.Price * 0.1M,
                PackagingStatus = OrderDetailStatus.Pending,
                Unit = productResponseDTO.Unit,
            };
        }
        public static Order ToEntity(this OrderRequestDTO requestDto)
        {
            return new Order
            {
                OrderCode = requestDto.OrderCode,
                Status = requestDto.Status,
                Notes = requestDto.Notes,
                TotalAmount = 0,
                ShippingFee = 0,
                TaxAmount = 0,
                DiscountAmount = 0,
                FinalAmount = 0,
                PaymentMethod = requestDto.PaymentMethod,
                PaymentStatus = PaymentStatus.Unpaid,
                TrackingNumber = "",
                ShippingMethod = "",
                ShippingPartner = "",
                ShippedAt = null,
                DeliveredAt = null,
                CancelledAt = null,
                ShippingName = "",
                ShippingPhone = "",
                ShippingAddress = "",
                ShippingProvince = "",
                ShippingDistrict = "",
                ShippingWard = "",
                BuyerId = requestDto.BuyerId,
            };
        }
        public static OrderTimeline ToEntity(this OrderTimelineRequestDTO requestDto)
        {
            return new OrderTimeline
            {
                OrderId = requestDto.OrderId,
                EventType = requestDto.EventType,
                Status = requestDto.Status,
                Description = requestDto.Description,
            };
        }
        public static Entities.PaymentMethod ToEntity(this PaymentMethodRequestDTO requestDto)
        {
            return new Entities.PaymentMethod
            {
                CardName = requestDto.CardName,
                PaymentType = requestDto.PaymentType,
                AccountNumber = requestDto.AccountNumber,
                AccountHolderName = requestDto.AccountHolderName,
                BankName = requestDto.BankName,
            };
        }
        public static Product ToEntity(this ProductRequestDTO requestDto)
        {
            var product = new Product
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                Price = requestDto.Price,
                StockQuantity = requestDto.StockQuantity,
                Unit = requestDto.Unit,
                StoreId = requestDto.StoreId,
                CategoryId = requestDto.CategoryId,
                Origin = requestDto.Origin,
                IsActive = true,
                SoldQuantity = 0,
                AverageRating = 0,
                TotalSoldQuantity = 0,
                ReviewCount = 0,
            };
            if (requestDto.ImageFile != null)
            {
                product.ImageUrl = requestDto.ImageUrl;
            }
            return product;
        }
        public static RegistrationApprovalResult ToEntity(this RegistrationApprovalResultRequestDTO requestDto, int adminId)
        {
            return new RegistrationApprovalResult
            {
                RegistrationId = requestDto.RegistrationId,
                AdminId = adminId,
                ApprovalResult = requestDto.ApprovalResult,
                Note = requestDto.Note,
            };
        }
        public static Review ToEntity(this ReviewRequestDTO requestDto)
        {
            return new Review
            {
                UserId = requestDto.UserId,
                ProductId = requestDto.ProductId,
                Rating = requestDto.Rating,
                Content = requestDto.Content ?? "",
                ShopResponse = "",
                ImageUrl = requestDto.ImageFile != null ? requestDto.ImageUrl : "",
            };
        }
        public static Store ToEntity(this StoreRequestDTO requestDto)
        {
            return new Store
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                LogoUrl = requestDto.LogoFile != null ? requestDto.LogoUrl : "",
                Address = requestDto.Address,
                PhoneNumber = requestDto.PhoneNumber,
                Email = requestDto.Email,
                BusinessType = requestDto.BusinessType ?? StoreType.Farmer,
                IsActive = true,
                VerificationStatus = StoreStatus.Pending,
                AverageRating = 0,
                ReviewCount = 0,
                UserId = requestDto.UserId,
            };
        }
        public static Transaction ToEntity(this TransactionRequestDTO requestDto, OrderResponseDTO? orderResponseDTO)
        {
            return new Transaction
            {
                TransactionCode = Generator.GenerateTransactionCode(),
                OrderId = requestDto.OrderId,
                BuyerId = requestDto.BuyerId,
                Amount = orderResponseDTO.FinalAmount,
                PaymentMethod = requestDto.PaymentMethod,
                Status = TransactionStatus.Pending,
                PaymentDueDate = DateTime.Now.AddDays(7),
                PaymentDate = null,
                Details = requestDto.Details ?? "",
            };
        }
        public static User ToEntity(this UserRequestDTO requestDto)
        {
            return new User
            {
                FullName = requestDto.FullName,
                Email = requestDto.Email,
                PhoneNumber = requestDto.PhoneNumber ?? "",
                Address = requestDto.Address ?? "",
                IsActive = requestDto.IsActive ?? true,
                ImageUrl = requestDto.ImageFile != null ? requestDto.ImageUrl : "",
                Role = requestDto.Role ?? UserRole.User,
                PasswordHash = requestDto.PasswordNew,
                EmailVerified = requestDto.IsActive ?? true,
                EmailNotificationsEnabled = requestDto.EmailNotificationsEnabled ?? true,
                OrderStatusNotificationsEnabled = requestDto.OrderStatusNotificationsEnabled ?? true,
                DiscountNotificationsEnabled = requestDto.DiscountNotificationsEnabled ?? true,
                AdminNotificationsEnabled = requestDto.AdminNotificationsEnabled ?? true,
            };
        }
        public static Cart ToEntity(this CartRequestDTO requestDto)
        {
            return new Cart
            {
                UserId = requestDto.UserId
            };
        }
        public static ShopCart ToEntity(this ShopCartRequestDTO requestDto)
        {
            return new ShopCart
            {
                ShopId = requestDto.ShopId,
                CartId = requestDto.CartId
            };
        }
        public static CartItem ToEntity(this CartItemRequestDTO requestDto)
        {
            return new CartItem
            {
                ProductId = requestDto.ProductId,
                Quantity = requestDto.Quantity,
                ShopCartId = requestDto.ShopCartId
            };
        }
        public static ContactRequest ToEntity(this ContactRequestDTO requestDto)
        {
            return new ContactRequest
            {
                Id = requestDto.Id,
                FullName = requestDto.FullName,
                Email = requestDto.Email,
                ServiceType = requestDto.ServiceType,
                PhoneNumber = requestDto.PhoneNumber,
                Message = requestDto.Message
            };
        }
    }
}
