using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;
using VNFarm.Interfaces.Repositories;
using VNFarm.Interfaces.Services;
using VNFarm.Mappers;

namespace VNFarm.Services
{
    public class OrderService : BaseService<Order, OrderRequestDTO, OrderResponseDTO>, IOrderService
    {
        #region Fields & Constructor
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IDiscountRepository _discountRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(
            IRepository<Order> repository,
            IOrderRepository orderRepository,
            IDiscountRepository discountRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository,
            ILogger<OrderService> logger) : base(repository)
        {
            _orderRepository = orderRepository;
            _discountRepository = discountRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
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
                // if (orderCriteriaFilter.StoreId.HasValue)
                //     query = query.Where(o => o.StoreId == orderCriteriaFilter.StoreId.Value);

                if (orderCriteriaFilter.UserId.HasValue)
                    query = query.Where(o => o.BuyerId == orderCriteriaFilter.UserId.Value);

                if (orderCriteriaFilter.StoreId.HasValue)
                    query = query.Where(o => o.OrderItems.Any(item => item.Product != null && item.Product.StoreId == orderCriteriaFilter.StoreId.Value));

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
                o => o.BuyerId == userId
            );
            return orders.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<OrderResponseDTO?>> GetOrdersByStoreIdAsync(int storeId)
        {
            var orders = await _repository.FindAsync(o => o.OrderItems.Any(item => item.Product != null && item.Product.StoreId == storeId));
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

        #region Order Items Management
        public async Task<OrderItemResponseDTO?> AddOrderItemAsync(int orderId, OrderItemRequestDTO orderItemRequest)
        {
            if (orderId != orderItemRequest.OrderId) return null;
            var product = await _productRepository.GetByIdAsync(orderItemRequest.ProductId);
            if (product == null) return null;
            
            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = orderItemRequest.ProductId,
                Quantity = orderItemRequest.Quantity,
                UnitPrice = orderItemRequest.UnitPrice,
                ShopId = orderItemRequest.ShopId,
                Unit = product.Unit,
                ShippingFee = 0,
                TaxAmount = orderItemRequest.UnitPrice * orderItemRequest.Quantity * 0.1m,
                Subtotal = orderItemRequest.UnitPrice * orderItemRequest.Quantity * 1.1m,
                PackagingStatus = orderItemRequest.PackagingStatus,
                Product = product
            };
            
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return null;
            
            order.OrderItems.Add(orderItem);
            await _repository.UpdateAsync(order);
            
            return orderItem.ToResponseDTO();
        }

        public async Task<IEnumerable<OrderItemResponseDTO>> GetOrderItemsAsync(int orderId)
        {
            var orderItems = await _orderRepository.GetOrderItemsAsync(orderId);
            return orderItems.Select(e => e.ToResponseDTO()).ToList();
        }

        public async Task<bool> UpdateOrderItemStatusAsync(int orderId, int productId, OrderItemStatus status)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return false;
            
            var orderItem = order.OrderItems.FirstOrDefault(e => e.ProductId == productId);
            if (orderItem == null) return false;
            
            orderItem.PackagingStatus = status;
            return await _repository.UpdateAsync(order);
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
            // Lấy tất cả timeline hiện tại của đơn hàng
            var existingTimelines = await _orderRepository.GetOrderTimelineAsync(orderId);
            
            // Cập nhật tất cả timeline trước đó thành trạng thái đã hoàn thành
            foreach (var timeline in existingTimelines)
            {
                timeline.Status = OrderTimelineStatus.Completed;
            }
            // Thêm timeline mới
            var orderTimeline = orderTimelineRequestDTO.ToEntity();
            orderTimeline.OrderId = orderId;
            await _orderRepository.AddOrderTimelineAsync(orderId, orderTimeline);
            await _orderRepository.SaveChangesAsync();
            
            return orderTimeline.ToResponseDTO();
        }
        #endregion

        #region Order Calculations
        public async Task<decimal> GetTotalRevenueByStoreIdAsync(int storeId)
        {
            var orders = await _repository.FindAsync(o => o.OrderItems.Any(item => item.Product != null && item.Product.StoreId == storeId));
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

        #region Create Order
        public async Task<OrderResponseDTO?> CreateOrderFromCheckoutAsync(int userId, CheckoutRequestDTO checkoutRequest)
        {
            try
            {
                // 1. Lấy thông tin giỏ hàng của người dùng
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    throw new Exception("Không tìm thấy giỏ hàng");
                }

                // 2. Tạo đơn hàng mới
                var order = new Order
                {
                    OrderCode = GenerateOrderCode(),
                    Status = OrderStatus.Pending,
                    Notes = checkoutRequest.Notes,
                    BuyerId = userId,
                    ShippingName = checkoutRequest.Address.ShippingName,
                    ShippingPhone = checkoutRequest.Address.ShippingPhone,
                    ShippingAddress = checkoutRequest.Address.ShippingAddress,
                    ShippingProvince = checkoutRequest.Address.ShippingProvince,
                    ShippingDistrict = checkoutRequest.Address.ShippingDistrict,
                    ShippingWard = checkoutRequest.Address.ShippingWard,
                    PaymentMethod = checkoutRequest.PaymentMethod,
                    PaymentStatus = PaymentStatus.Unpaid,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // 3. Xử lý mã giảm giá nếu có
                if (!string.IsNullOrEmpty(checkoutRequest.DiscountCode))
                {
                    var discount = (await _discountRepository.FindAsync(d => d.Code == checkoutRequest.DiscountCode)).FirstOrDefault();
                    if (discount != null && discount.Status == DiscountStatus.Active)
                    {
                        order.DiscountId = discount.Id;
                    }
                }

                // 4. Thêm đơn hàng vào database
                await _repository.AddAsync(order);

                // 5. Tạo chi tiết đơn hàng từ các mục trong giỏ hàng
                decimal totalAmount = 0;
                decimal shippingFee = 0;
                decimal taxAmount = 0;

                // Lấy danh sách CartItem được chọn
                var selectedCartItems = new List<CartItem>();
                var shopCarts = new Dictionary<int, ShopCart>();

                foreach (var shopCart in cart.ShopCarts)
                {
                    foreach (var cartItem in shopCart.CartItems)
                    {
                        // Thêm tất cả sản phẩm vào đơn hàng không cần kiểm tra
                        selectedCartItems.Add(cartItem);
                        
                        // Thêm ShopCart vào danh sách nếu chưa có
                        if (!shopCarts.ContainsKey(shopCart.Id))
                        {
                            shopCarts[shopCart.Id] = shopCart;
                            // Tính phí vận chuyển: mỗi cửa hàng 50,000 VND
                            shippingFee += 50000;
                        }

                        // Tạo chi tiết đơn hàng
                        var product = cartItem.Product;
                        if (product != null)
                        {
                            decimal itemPrice = product.Price * cartItem.Quantity;
                            decimal itemTax = itemPrice * 0.1m; // Thuế 10%
                            
                            var orderItem = new OrderItem
                            {
                                OrderId = order.Id,
                                ProductId = cartItem.ProductId,
                                Quantity = cartItem.Quantity,
                                Unit = product.Unit,
                                UnitPrice = product.Price,
                                ShippingFee = 0, // Phí vận chuyển tính theo cửa hàng
                                TaxAmount = itemTax,
                                Subtotal = itemPrice + itemTax,
                                PackagingStatus = OrderItemStatus.Pending,
                                ShopId = shopCart.ShopId,
                                Product = product
                            };
                            
                            await _orderRepository.AddOrderItemAsync(order.Id, orderItem);
                            
                            // Cập nhật tổng tiền
                            totalAmount += itemPrice;
                            taxAmount += itemTax;
                        }
                    }
                }

                // 6. Cập nhật thông tin giá cả cho đơn hàng
                order.TotalAmount = totalAmount;
                order.ShippingFee = shippingFee;
                order.TaxAmount = taxAmount;
                
                // Tính giảm giá nếu có
                decimal discountAmount = 0;
                if (order.DiscountId.HasValue)
                {
                    discountAmount = await CalculateOrderDiscountAmountAsync(order.Id);
                }
                
                order.DiscountAmount = discountAmount;
                order.FinalAmount = totalAmount + shippingFee + taxAmount - discountAmount;
                
                await _repository.UpdateAsync(order);

                // 7. Tạo timeline cho đơn hàng
                var orderTimeline = new OrderTimeline
                {
                    OrderId = order.Id,
                    Status = OrderTimelineStatus.Pending,
                    Description = "Đơn hàng đã được tạo",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                
                await _orderRepository.AddOrderTimelineAsync(order.Id, orderTimeline);

                // 8. Xóa các CartItem đã được chọn khỏi giỏ hàng
                foreach (var cartItem in selectedCartItems)
                {
                    var shopCart = shopCarts[cartItem.ShopCartId];
                    shopCart.CartItems.Remove(cartItem);
                    
                    // Xóa ShopCart nếu không còn CartItem nào
                    if (!shopCart.CartItems.Any())
                    {
                        cart.ShopCarts.Remove(shopCart);
                    }
                }
                
                await _orderRepository.SaveChangesAsync();

                // 9. Trả về OrderResponseDTO
                return MapToDTO(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo đơn hàng từ checkout");
                return null;
            }
        }

        // Phương thức tạo mã đơn hàng
        private string GenerateOrderCode()
        {
            // Format: VNF-yyyyMMdd-randomNumber
            string dateCode = DateTime.Now.ToString("yyyyMMdd");
            string randomCode = new Random().Next(1000, 9999).ToString();
            return $"VNF-{dateCode}-{randomCode}";
        }

        public async Task<bool?> SetOrderPaymentIdAsync(int orderId, long orderPaymentId)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null) return null;
            order.OrderPaymentId = orderPaymentId;
            return await _repository.UpdateAsync(order);
        }

        public async Task<bool?> SetOrderPaymentStatusAsync(int id, PaymentStatus paymentStatus)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) return null;
            order.PaymentStatus = paymentStatus;
            return await _repository.UpdateAsync(order);
        }

        public async Task<IEnumerable<OrderItemResponseDTO>> GetOrderItemsByOrderIdAndStoreIdAsync(int orderId, int storeId)
        {
            var orderItems = await _orderRepository.GetOrderItemsAsync(orderId);
            return orderItems.Where(e => e.ShopId == storeId).Select(e => e.ToResponseDTO()).ToList();
        }

        #endregion
    }
}