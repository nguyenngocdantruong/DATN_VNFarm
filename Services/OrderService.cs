using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Helpers;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm_FinalFinal.Interfaces.Services;
using VNFarm_FinalFinal.Mappers;

namespace VNFarm.Infrastructure.Services
{
    public class OrderService : BaseService<Order, OrderRequestDTO, OrderResponseDTO>, IOrderService
    {
        #region Fields & Constructor
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IRepository<Order> repository,
            IOrderRepository orderRepository,
            IDiscountRepository discountRepository,
            IProductRepository productRepository,
            ILogger<OrderService> logger) : base(repository)
        {
            _orderRepository = orderRepository;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
            _logger = logger;
        }
        #endregion

        #region Base Service Implementation
        protected override OrderResponseDTO? MapToDTO(Order? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override Order? MapToEntity(OrderRequestDTO dto)
        {
            return dto.ToEntity();
        }

        public override async Task<bool> UpdateAsync(OrderRequestDTO dto)
        {
            var old = await _repository.GetByIdAsync(dto.Id);
            if (old == null) return false;

            // Check if the order is already paid
            if (old.PaymentStatus == PaymentStatus.Paid)
            {
                _logger.LogWarning($"Order {dto.Id} is already paid. Cannot update.");
                return false;
            }

            // Check if the order is already shipped
            if (old.Status == OrderStatus.Delivered)
            {
                _logger.LogWarning($"Order {dto.Id} is already shipped. Cannot update.");
                return false;
            }

            // Check if the order is already cancelled
            if (old.Status == OrderStatus.Cancelled)
            {
                _logger.LogWarning($"Order {dto.Id} is already cancelled. Cannot update.");
                return false;
            }

            // Check if the order is already completed
            if (old.Status == OrderStatus.Completed)
            {
                _logger.LogWarning($"Order {dto.Id} is already completed. Cannot update.");
                return false;
            }

            old.UpdateFromRequestDto(dto);
            await _repository.UpdateAsync(old);
            return true;
        }

        public override async Task<IEnumerable<OrderResponseDTO?>> QueryAsync(string query)
        {
            var orders = await _repository.FindAsync(
                o => o.Notes.Contains(query) || o.OrderCode.Contains(query)
            );
            return orders.Select(MapToDTO).ToList();
        }
        #endregion

        #region Order Query Methods
        public async override Task<IQueryable<Order>> Query(IFilterCriteria filter)
        {
            var query = await _repository.GetQueryableAsync();
            if (filter is OrderCriteriaFilter orderCriteriaFilter)
            {
                if (!string.IsNullOrEmpty(orderCriteriaFilter.SearchTerm))
                    query = query.Where(o => o.Notes.Contains(orderCriteriaFilter.SearchTerm) || o.OrderCode.Contains(orderCriteriaFilter.SearchTerm));

                if (orderCriteriaFilter.Status != OrderStatus.All)
                {
                    query = query.Where(o => o.Status == orderCriteriaFilter.Status);
                }
                if (orderCriteriaFilter.PaymentStatus != PaymentStatus.All)
                {
                    query = query.Where(o => o.PaymentStatus == orderCriteriaFilter.PaymentStatus);
                }

                if (orderCriteriaFilter.PaymentMethod != PaymentMethodEnum.All)
                {
                    query = query.Where(m => m.PaymentMethod == orderCriteriaFilter.PaymentMethod);
                }
                if (orderCriteriaFilter.MinTotal.HasValue)
                    query = query.Where(o => o.TotalAmount >= orderCriteriaFilter.MinTotal.Value);
                if (orderCriteriaFilter.MaxTotal.HasValue)
                    query = query.Where(o => o.TotalAmount <= orderCriteriaFilter.MaxTotal.Value);
                if (orderCriteriaFilter.StartDate.HasValue)
                    query = query.Where(o => o.CreatedAt >= orderCriteriaFilter.StartDate.Value);
                if (orderCriteriaFilter.EndDate.HasValue)
                    query = query.Where(o => o.CreatedAt <= orderCriteriaFilter.EndDate.Value);
            }
            return query;
        }

        public async override Task<IEnumerable<OrderResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Order> query, IFilterCriteria filter)
        {
            if (filter is OrderCriteriaFilter orderCriteriaFilter)
            {
                query = orderCriteriaFilter.SortBy == SortType.Ascending
                    ? query.OrderBy(o => o.CreatedAt)
                : query.OrderByDescending(o => o.CreatedAt);

                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            }
            else
            {
                throw new ArgumentException("Filter truyền vào không phải là OrderCriteriaFilter");
            }
            return (await query.ToListAsync()).Select(MapToDTO);
        }

        public async Task<IEnumerable<OrderResponseDTO?>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _repository.FindAsync(
                o => o.BuyerId == userId || o.StoreId == userId
            );
            return orders.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<OrderResponseDTO?>> GetOrdersByStoreIdAsync(int storeId)
        {
            var orders = await _repository.FindAsync(o => o.StoreId == storeId);
            return orders.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<OrderResponseDTO?>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _repository.FindAsync(o => o.Status == status);
            return orders.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<OrderResponseDTO?>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _repository.FindAsync(
                o => o.CreatedAt >= startDate && o.CreatedAt <= endDate
            );
            return orders.Select(MapToDTO).ToList();
        }
        #endregion

        #region Order Status & Updates
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> UpdateOrderPaymentStatusAsync(int orderId, PaymentStatus paymentStatus)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.PaymentStatus = paymentStatus;
            return await _repository.UpdateAsync(order);
        }

        public async Task<bool> UpdateOrderAddressAsync(int orderId, AddressRequestDTO addressRequest)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.UpdateFromRequestDto(addressRequest);
            return await _repository.UpdateAsync(order);
        }

        public async Task<bool> UpdateOrderShippingAsync(int orderId, ShippingRequestDTO shippingRequest)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.UpdateFromRequestDto(shippingRequest);
            return await _repository.UpdateAsync(order);
        }
        #endregion

        #region Order Details Management
        public async Task<OrderDetailResponseDTO?> AddOrderDetailAsync(int orderId, OrderDetailRequestDTO orderDetailRequest)
        {
            if (orderId != orderDetailRequest.OrderId) return null;
            var product = await _productRepository.GetByIdAsync(orderDetailRequest.ProductId);
            if (product == null) return null;
            var orderDetail = orderDetailRequest.ToEntity(product.ToResponseDTO());
            orderDetail.OrderId = orderId;
            await _orderRepository.AddOrderDetailAsync(orderId, orderDetail);
            return orderDetail.ToResponseDTO();
        }

        public async Task<IEnumerable<OrderDetailResponseDTO>> GetOrderDetailAsync(int orderId)
        {
            var orderDetails = await _orderRepository.GetOrderDetailAsync(orderId);
            return orderDetails.Select(e => e.ToResponseDTO()).ToList();
        }

        public async Task<bool> UpdateOrderDetailStatusAsync(int orderId, int productId, OrderDetailStatus status)
        {
            var orderDetails = await _orderRepository.GetOrderDetailAsync(orderId);
            if (orderDetails == null) return false;
            var orderDetail = orderDetails.FirstOrDefault(e => e.ProductId == productId);
            if (orderDetail == null) return false;
            orderDetail.PackagingStatus = status;
            return await _orderRepository.SaveChangesAsync();
        }
        #endregion

        #region Order Timeline
        public async Task<IEnumerable<OrderTimelineResponseDTO?>> GetOrderTimelineAsync(int orderId)
        {
            var orderTimelines = await _orderRepository.GetOrderTimelineAsync(orderId);
            return orderTimelines.Select(e => e.ToResponseDTO()).ToList();
        }

        public async Task<OrderTimelineResponseDTO?> AddOrderTimelineAsync(int orderId, OrderTimelineRequestDTO orderTimelineRequestDTO)
        {
            var orderTimeline = orderTimelineRequestDTO.ToEntity();
            orderTimeline.OrderId = orderId;
            await _orderRepository.AddOrderTimelineAsync(orderId, orderTimeline);
            return orderTimeline.ToResponseDTO();
        }
        #endregion

        #region Order Calculations
        public async Task<decimal> GetTotalRevenueByStoreIdAsync(int storeId)
        {
            var orders = await _repository.FindAsync(o => o.StoreId == storeId);
            return orders.Sum(o => o.TotalAmount);
        }

        public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _repository.FindAsync(
                o => o.CreatedAt >= startDate && o.CreatedAt <= endDate
            );
            return orders.Sum(o => o.TotalAmount);
        }

        public async Task<decimal> CalculateOrderTotalAmountAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return 0;
            var orderDetails = await _orderRepository.GetOrderDetailAsync(orderId);
            var totalAmount = orderDetails.Sum(od => od.Subtotal);
            return totalAmount;
        }

        public async Task<decimal> CalculateOrderShippingFeeAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return 0;
            var orderDetails = await _orderRepository.GetOrderDetailAsync(orderId);
            var shippingFee = orderDetails.Sum(od => od.ShippingFee);
            return shippingFee;
        }

        public async Task<decimal> CalculateOrderTaxAmountAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return 0;
            var orderDetails = await _orderRepository.GetOrderDetailAsync(orderId);
            var taxAmount = orderDetails.Sum(od => od.TaxAmount);
            return taxAmount;
        }

        public async Task<decimal> CalculateOrderDiscountAmountAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            var discountVoucher = await _discountRepository.GetByIdAsync(order?.DiscountId);
            if (order == null || discountVoucher == null || discountVoucher.Status == DiscountStatus.Inactive) return 0;
            var discountAmount = discountVoucher.DiscountAmount;
            switch (discountVoucher.Type)
            {
                case DiscountType.Percentage:
                    discountAmount = order.TotalAmount * discountAmount / 100;
                    break;
                case DiscountType.FixedAmount:
                    break;
                case DiscountType.FreeShipping:
                    discountAmount = order.ShippingFee;
                    break;
                default:
                    discountAmount = 0;
                    break;
            }
            return discountAmount;
        }

        public async Task<decimal> CalculateOrderFinalAmountAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return 0;
            var orderDetails = await _orderRepository.GetOrderDetailAsync(orderId);
            var totalAmount = orderDetails.Sum(od => od.Subtotal);
            var discountAmount = await CalculateOrderDiscountAmountAsync(orderId);
            var finalAmount = totalAmount - discountAmount;
            return finalAmount;
        }
        #endregion

        #region Discount Management
        public async Task<bool> AddDiscountToOrderAsync(int orderId, string discountCode)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;
            var discount = (await _discountRepository.FindAsync(d => d.Code == discountCode)).FirstOrDefault();
            if (discount == null || discount.Status == DiscountStatus.Inactive) return false;
            order.DiscountId = discount.Id;
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> RemoveDiscountFromOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;
            order.DiscountId = null;
            return await _orderRepository.UpdateAsync(order);
        }
        #endregion

        #region Review Management
        public async Task<bool> AddOrderReviewAsync(int orderId, int productId, ReviewRequestDTO reviewRequest)
        {
            if (reviewRequest.ImageFile != null)
            {
                var imageUrl = await FileUpload.UploadFile(reviewRequest.ImageFile, FileUpload.ReviewFolder);
                reviewRequest.ImageUrl = imageUrl;
            }
            var review = reviewRequest.ToEntity();
            review.ProductId = productId;
            await _productRepository.AddReviewAsync(review);
            return true;
        }
        #endregion
    }
}