using System;
using System.Linq;
using VNFarm.DTOs.Response;
using VNFarm.Helpers;
using VNFarm.DTOs.Request;
using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Helpers
{
    public static class RequestDtoToEntityMappingExtensions
    {
        public static void UpdateFromRequestDto(this Order order, AddressRequestDTO addressRequestDTO)
        {
            order.ShippingName = addressRequestDTO.ShippingName;
            order.ShippingPhone = addressRequestDTO.ShippingPhone;
            order.ShippingAddress = addressRequestDTO.ShippingAddress;
            order.ShippingProvince = addressRequestDTO.ShippingProvince;
            order.ShippingDistrict = addressRequestDTO.ShippingDistrict;
            order.ShippingWard = addressRequestDTO.ShippingWard;
        }
        public static void UpdateFromRequestDto(this BusinessRegistration businessRegistration, BusinessRegistrationRequestDTO businessRegistrationRequestDTO)
        {
            businessRegistration.UserId = businessRegistrationRequestDTO.UserId;
            businessRegistration.BusinessName = businessRegistrationRequestDTO.BusinessName;
            businessRegistration.BusinessType = businessRegistrationRequestDTO.BusinessType;
            businessRegistration.TaxCode = businessRegistrationRequestDTO.TaxCode;
            businessRegistration.BusinessLicenseUrl = businessRegistrationRequestDTO.BusinessLicenseUrl;
            businessRegistration.Address = businessRegistrationRequestDTO.Address;
        }
        public static void UpdateFromRequestDto(this Category category, CategoryRequestDTO categoryRequestDTO)
        {
            category.Name = categoryRequestDTO.Name;
            category.Description = categoryRequestDTO.Description;
            category.IconUrl = categoryRequestDTO.IconUrl;
        }
        public static void UpdateFromRequestDto(this ChatRoom chatRoom, ChatRoomRequestDTO chatRoomRequestDTO)
        {
            chatRoom.NameRoom = chatRoomRequestDTO.NameRoom;
            chatRoom.Description = chatRoomRequestDTO.Description;
            chatRoom.Status = chatRoomRequestDTO.Status;
            chatRoom.Type = chatRoomRequestDTO.Type;
            if(chatRoomRequestDTO.IsActive != null)
            {
                chatRoom.IsActive = chatRoomRequestDTO.IsActive.Value;
            }
            if(chatRoomRequestDTO.OrderId != null)
            {
                chatRoom.OrderId = chatRoomRequestDTO.OrderId.Value;
            }
        }
        public static void UpdateFromRequestDto(this Discount discount, DiscountRequestDTO discountRequestDTO)
        {
            discount.Code = discountRequestDTO.Code;
            discount.Description = discountRequestDTO.Description;
            discount.RemainingQuantity = discountRequestDTO.RemainingQuantity;
            discount.Status = discountRequestDTO.Status;
            discount.StartDate = discountRequestDTO.StartDate;
            discount.EndDate = discountRequestDTO.EndDate;
            discount.Type = discountRequestDTO.Type;
            if(discountRequestDTO.StoreId != null)
            {
                discount.StoreId = discountRequestDTO.StoreId.Value;
            }
            if(discountRequestDTO.UserId != null)
            {
                discount.UserId = discountRequestDTO.UserId.Value;
            }
            discount.DiscountAmount = discountRequestDTO.Type switch{
                DiscountType.Percentage => Math.Clamp(discountRequestDTO.DiscountAmount, 0, 100),
                DiscountType.FixedAmount => discountRequestDTO.DiscountAmount,
                DiscountType.FreeShipping => 0,
                _ => throw new ArgumentException("Loại giảm giá không hợp lệ")
            };
            discount.MinimumOrderAmount = discountRequestDTO.MinimumOrderAmount;
            discount.MaximumDiscountAmount = discountRequestDTO.MaximumDiscountAmount;
        }
        public static void UpdateFromRequestDto(this Notification notification, NotificationRequestDTO notificationRequestDTO)
        {
            notification.UserId = notificationRequestDTO.UserId;
            notification.Content = notificationRequestDTO.Content;
            notification.LinkUrl = notificationRequestDTO.LinkUrl;
            notification.Type = notificationRequestDTO.Type;
        }
        public static void UpdateFromRequestDto(this OrderDetail orderDetail, OrderDetailRequestDTO orderDetailRequestDTO)
        {
            if(orderDetailRequestDTO.OrderId != orderDetail.OrderId)
            {
                throw new ArgumentException("Mã đơn hàng không hợp lệ");
            }
            orderDetail.ProductId = orderDetailRequestDTO.ProductId;
            orderDetail.Quantity = orderDetailRequestDTO.Quantity;
        }
        public static void UpdateFromRequestDto(this Order order, OrderRequestDTO orderRequestDTO)
        {
            order.OrderCode = orderRequestDTO.OrderCode;
            order.Status = orderRequestDTO.Status;
            order.Notes = orderRequestDTO.Notes;
            order.PaymentMethod = orderRequestDTO.PaymentMethod;
            // order.BuyerId = orderRequestDTO.BuyerId;
            // order.StoreId = orderRequestDTO.StoreId;
        }
        public static void UpdateFromRequestDto(this OrderTimeline orderTimeline, OrderTimelineRequestDTO orderTimelineRequestDTO)
        {
            if(orderTimelineRequestDTO.OrderId != orderTimeline.OrderId)
            {
                throw new ArgumentException("Mã đơn hàng không hợp lệ");
            }
            orderTimeline.EventType = orderTimelineRequestDTO.EventType;
            orderTimeline.Status = orderTimelineRequestDTO.Status;
            orderTimeline.Description = orderTimelineRequestDTO.Description;
        }
        public static void UpdateFromRequestDto(this PaymentMethod paymentMethod, PaymentMethodRequestDTO paymentMethodRequestDTO)
        {
            paymentMethod.CardName = paymentMethodRequestDTO.CardName;
            paymentMethod.PaymentType = paymentMethodRequestDTO.PaymentType;
            paymentMethod.AccountNumber = paymentMethodRequestDTO.AccountNumber;
            paymentMethod.AccountHolderName = paymentMethodRequestDTO.AccountHolderName;
            paymentMethod.BankName = paymentMethodRequestDTO.BankName;
        }
        public static void UpdateFromRequestDto(this Product product, ProductRequestDTO productRequestDTO)
        {
            if(product.Id != productRequestDTO.Id)
            {
                throw new ArgumentException("Mã sản phẩm không hợp lệ");
            }
            product.Name = productRequestDTO.Name;
            product.Description = productRequestDTO.Description;
            product.Price = productRequestDTO.Price;
            product.StockQuantity = productRequestDTO.StockQuantity;
            product.Unit = productRequestDTO.Unit;
            product.CategoryId = productRequestDTO.CategoryId;
            product.Origin = productRequestDTO.Origin;
            if(productRequestDTO.IsActive != null)
            {
                product.IsActive = productRequestDTO.IsActive.Value;
            }
            if(productRequestDTO.ImageFile != null)
            {
                product.ImageUrl = productRequestDTO.ImageUrl;
            }
        }
        public static void UpdateFromRequestDto(this RegistrationApprovalResult registrationApprovalResult, RegistrationApprovalResultRequestDTO registrationApprovalResultRequestDTO)
        {
            if(registrationApprovalResult.RegistrationId != registrationApprovalResultRequestDTO.RegistrationId)
            {
                throw new ArgumentException("Mã đăng ký không hợp lệ");
            }
            registrationApprovalResult.ApprovalResult = registrationApprovalResultRequestDTO.ApprovalResult;
            registrationApprovalResult.Note = registrationApprovalResultRequestDTO.Note;
        }
        public static void UpdateFromRequestDto(this Order order, ShippingRequestDTO shippingRequestDTO)
        {
            if(shippingRequestDTO.OrderId != order.Id)
            {
                throw new ArgumentException("Mã đơn hàng không hợp lệ");
            }
            order.TrackingNumber = shippingRequestDTO.TrackingNumber;
            order.ShippingMethod = shippingRequestDTO.ShippingMethod;
            order.ShippingPartner = shippingRequestDTO.ShippingPartner;
            order.ShippedAt = shippingRequestDTO.ShippedAt;
            order.DeliveredAt = shippingRequestDTO.DeliveredAt;
            order.CancelledAt = shippingRequestDTO.CancelledAt;
        }
        public static void UpdateFromRequestDto(this Store store, StoreRequestDTO storeRequestDTO)
        {
            if(store.Id != storeRequestDTO.Id)
            {
                throw new ArgumentException("Mã cửa hàng không hợp lệ");
            }
            store.Name = storeRequestDTO.Name;
            store.Description = storeRequestDTO.Description;
            store.Address = storeRequestDTO.Address;
            store.PhoneNumber = storeRequestDTO.PhoneNumber;
            store.Email = storeRequestDTO.Email;
            if(storeRequestDTO.LogoUrl != null)
            {
                store.LogoUrl = storeRequestDTO.LogoUrl;
            }
        }
        public static void UpdateFromRequestDto(this Transaction transaction, TransactionRequestDTO transactionRequestDTO)
        {
            if(transaction.TransactionCode != transactionRequestDTO.TransactionCode)
            {
                throw new ArgumentException("Mã giao dịch không hợp lệ");
            }
            if(transaction.OrderId != transactionRequestDTO.OrderId)
            {
                throw new ArgumentException("Mã đơn hàng không hợp lệ");
            }
            transaction.Details = transactionRequestDTO.Details ?? "";
            transaction.PaymentMethod = transactionRequestDTO.PaymentMethod;
        }
        public static void UpdateFromRequestDto(this User user, UserRequestDTO userRequestDTO)
        {
            if(user.Id != userRequestDTO.Id)
            {
                throw new ArgumentException("Mã người dùng không hợp lệ");
            }
            user.FullName = userRequestDTO.FullName;
            user.Email = userRequestDTO.Email;
            user.PhoneNumber = userRequestDTO.PhoneNumber;
            if(userRequestDTO.PasswordNew != null)
            {
                user.PasswordHash = userRequestDTO.PasswordNew;
            }
            if(userRequestDTO.Address != null)
            {
                user.Address = userRequestDTO.Address;
            }
            if(userRequestDTO.ImageUrl != null)
            {
                user.ImageUrl = userRequestDTO.ImageUrl;
            }
            if(userRequestDTO.EmailNotificationsEnabled != null)
            {
                user.EmailNotificationsEnabled = userRequestDTO.EmailNotificationsEnabled.Value;
            }
            if(userRequestDTO.OrderStatusNotificationsEnabled != null)
            {
                user.OrderStatusNotificationsEnabled = userRequestDTO.OrderStatusNotificationsEnabled.Value;
            }
            if(userRequestDTO.DiscountNotificationsEnabled != null)
            {
                user.DiscountNotificationsEnabled = userRequestDTO.DiscountNotificationsEnabled.Value;
            }
            if(userRequestDTO.AdminNotificationsEnabled != null)
            {
                user.AdminNotificationsEnabled = userRequestDTO.AdminNotificationsEnabled.Value;
            }
        }
    }
}

