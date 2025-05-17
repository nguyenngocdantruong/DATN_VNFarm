using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Interfaces.Services
{
    public interface IOrderService : IService<Order, OrderRequestDTO, OrderResponseDTO>
    {
        #region Get Orders
        Task<IEnumerable<OrderResponseDTO?>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderResponseDTO?>> GetOrdersByStoreIdAsync(int storeId);
        Task<IEnumerable<OrderResponseDTO?>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<OrderResponseDTO?>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        #endregion

        #region Order information
        Task<bool> UpdateOrderAddressAsync(int orderId, AddressRequestDTO addressRequest);
        Task<bool> UpdateOrderShippingAsync(int orderId, ShippingRequestDTO shippingRequest);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> UpdateOrderPaymentStatusAsync(int orderId, PaymentStatus paymentStatus);
        #endregion
        #region Order Calculation
        Task<decimal> CalculateOrderTotalAmountAsync(int orderId);
        Task<decimal> CalculateOrderShippingFeeAsync(int orderId);
        Task<decimal> CalculateOrderTaxAmountAsync(int orderId);
        Task<decimal> CalculateOrderDiscountAmountAsync(int orderId);
        Task<decimal> CalculateOrderFinalAmountAsync(int orderId);
        #endregion

        #region Order Review
        Task<bool> AddOrderReviewAsync(int orderId, int productId, ReviewRequestDTO reviewRequest);
        #endregion

        #region Revenue
        Task<decimal> GetTotalRevenueByStoreIdAsync(int storeId);
        Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        #endregion

        #region Discount
        Task<bool> AddDiscountToOrderAsync(int orderId, string discountCode);
        Task<bool> RemoveDiscountFromOrderAsync(int orderId);
        #endregion

        #region Order Timeline
        Task<OrderTimelineResponseDTO?> AddOrderTimelineAsync(int orderId, OrderTimelineRequestDTO orderTimelineRequest);
        Task<IEnumerable<OrderTimelineResponseDTO?>> GetOrderTimelineAsync(int orderId);
        #endregion

        #region Order Detail
        Task<OrderItemResponseDTO?> AddOrderItemAsync(int orderId, OrderItemRequestDTO orderItemRequest);
        Task<IEnumerable<OrderItemResponseDTO>> GetOrderItemsAsync(int orderId);
        Task<bool> UpdateOrderItemStatusAsync(int orderId, int productId, OrderItemStatus status);
        Task<IEnumerable<OrderItemResponseDTO>> GetOrderItemsByOrderIdAndStoreIdAsync(int orderId, int storeId);
        #endregion
        
        #region Create Order
        Task<OrderResponseDTO?> CreateOrderFromCheckoutAsync(int userId, CheckoutRequestDTO checkoutRequest);
        #endregion

        #region Payment
        Task<bool?> SetOrderPaymentIdAsync(int orderId, long orderPaymentId);
        Task<bool?> SetOrderPaymentStatusAsync(int id, PaymentStatus paymentStatus);
        #endregion
    }
} 