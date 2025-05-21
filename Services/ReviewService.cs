using Microsoft.EntityFrameworkCore;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Mappers;
using VNFarm.Repositories.Interfaces;
using VNFarm.Services.Interfaces;

namespace VNFarm.Services
{
    public class ReviewService : BaseService<Review, ReviewRequestDTO, ReviewResponseDTO>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(
            IReviewRepository reviewRepository, 
            IOrderRepository orderRepository, 
            IProductRepository productRepository,
            ILogger<ReviewService> logger) : base(reviewRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public override async Task<IEnumerable<ReviewResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Review> query, IFilterCriteria filter)
        {
            switch(filter.SortBy)
            {
                case SortType.Latest:
                    query = query.OrderByDescending(x => x.UpdatedAt);
                    break;
                case SortType.Oldest:
                    query = query.OrderBy(x => x.UpdatedAt);
                    break;
                default:
                    query = query.OrderByDescending(x => x.UpdatedAt);
                    break;
            }
            var result = await query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            return result.Select(MapToDTO);
        }

        public async Task<IEnumerable<ReviewResponseDTO?>> GetReviewByOrderIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if(order == null)
            {
                throw new Exception("Order not found");
            }
            var productIds = order.OrderItems.Select(x => x.ProductId).ToList();
            var queryable = await _reviewRepository.GetQueryableAsync();
            var reviews = queryable.Where(x => productIds.Contains(x.ProductId)).ToList();
            return reviews.Select(MapToDTO);
        }

        public async Task<IEnumerable<ReviewResponseDTO?>> GetReviewsByProductIdAsync(int productId)
        {
            var queryable = await _reviewRepository.GetQueryableAsync();
            var reviews = queryable.Where(x => x.ProductId == productId).ToList();
            return reviews.Select(MapToDTO);
        }

        public override async Task<IQueryable<Review>> Query(IFilterCriteria filter)
        {
            var query = await _reviewRepository.GetQueryableAsync();
            if(filter is ReviewFilterCriteria reviewFilter)
            {
                // Filter by product
                if(reviewFilter.ProductId.HasValue)
                {
                    query = query.Where(x => x.ProductId == reviewFilter.ProductId);
                }
                
                // Filter by user
                if(reviewFilter.UserId.HasValue)
                {
                    query = query.Where(x => x.UserId == reviewFilter.UserId);
                }
                
                // Filter by order
                if(reviewFilter.OrderId.HasValue)
                {
                    query = query.Where(x => x.OrderId == reviewFilter.OrderId);
                }
                
                // Filter by rating range
                if(reviewFilter.MinRating.HasValue)
                {
                    query = query.Where(x => x.Rating >= reviewFilter.MinRating.Value);
                }
                
                if(reviewFilter.MaxRating.HasValue)
                {
                    query = query.Where(x => x.Rating <= reviewFilter.MaxRating.Value);
                }
                
                // Filter by date range
                if(reviewFilter.StartDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt >= reviewFilter.StartDate.Value);
                }
                
                if(reviewFilter.EndDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt <= reviewFilter.EndDate.Value);
                }
                
                // Filter by has image
                if(reviewFilter.HasImage.HasValue)
                {
                    if(reviewFilter.HasImage.Value)
                    {
                        query = query.Where(x => !string.IsNullOrEmpty(x.ImageUrl));
                    }
                    else
                    {
                        query = query.Where(x => string.IsNullOrEmpty(x.ImageUrl));
                    }
                }
                
                // Filter by has content
                if(reviewFilter.HasContent.HasValue)
                {
                    if(reviewFilter.HasContent.Value)
                    {
                        query = query.Where(x => !string.IsNullOrEmpty(x.Content));
                    }
                    else
                    {
                        query = query.Where(x => string.IsNullOrEmpty(x.Content));
                    }
                }
                
                // Apply search term on content
                if(!string.IsNullOrEmpty(reviewFilter.SearchTerm))
                {
                    query = query.Where(x => x.Content.Contains(reviewFilter.SearchTerm));
                }
            }
            return query;
        }

        public override async Task<IEnumerable<ReviewResponseDTO?>> QueryAsync(string query)
        {
            var queryable = await _reviewRepository.GetQueryableAsync();
            var reviews = queryable.Where(x => x.Content.Contains(query)).ToList();
            return reviews.Select(MapToDTO);
        }

        public async Task<double> CalculateAverageRatingAsync(int productId, int rating)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if(product == null)
            {
                throw new Exception("Product not found");
            }
            var currentRating = product.AverageRating;
            var newRating = Math.Round((currentRating * product.ReviewCount + rating) / (product.ReviewCount + 1), 1);
            product.AverageRating = (double)newRating;
            product.ReviewCount++;
            await _productRepository.UpdateAsync(product);
            return (double)newRating;
        }

        public override async Task<bool> UpdateAsync(ReviewRequestDTO dto)
        {
            var review = await _reviewRepository.GetByIdAsync(dto.Id);
            if(review == null)
            {
                throw new Exception("Review not found");
            }
            return await _reviewRepository.UpdateAsync(review);
        }

        protected override ReviewResponseDTO? MapToDTO(Review? entity)
        {
            return entity?.ToResponseDTO();
        }

        protected override Review? MapToEntity(ReviewRequestDTO dto)
        {
            return dto.ToEntity();
        }

        #region Đánh giá đơn hàng theo OrderItem

        /// <summary>
        /// Kiểm tra xem người dùng có thể đánh giá sản phẩm trong đơn hàng hay không
        /// </summary>
        public async Task<bool> CanReviewOrderItemAsync(int orderId, int productId, int userId)
        {
            try
            {
                // Kiểm tra đơn hàng có tồn tại không
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    _logger.LogWarning("Không tìm thấy đơn hàng #{OrderId}", orderId);
                    return false;
                }

                // Kiểm tra người dùng có phải là người mua đơn hàng không
                if (order.BuyerId != userId)
                {
                    _logger.LogWarning("Người dùng #{UserId} không phải là người mua đơn hàng #{OrderId}", userId, orderId);
                    return false;
                }

                // Kiểm tra đơn hàng đã hoàn thành chưa
                if (order.Status != OrderStatus.Completed && order.Status != OrderStatus.Delivered)
                {
                    _logger.LogWarning("Đơn hàng #{OrderId} chưa hoàn thành, trạng thái hiện tại: {Status}", orderId, order.Status);
                    return false;
                }

                // Kiểm tra sản phẩm có trong đơn hàng không
                var orderItems = await _orderRepository.GetOrderItemsAsync(orderId);
                var orderItem = orderItems.FirstOrDefault(oi => oi.ProductId == productId);
                if (orderItem == null)
                {
                    _logger.LogWarning("Sản phẩm #{ProductId} không có trong đơn hàng #{OrderId}", productId, orderId);
                    return false;
                }

                // Kiểm tra xem người dùng đã đánh giá sản phẩm này trong đơn hàng chưa
                var queryable = await _reviewRepository.GetQueryableAsync();
                var existingReview = await queryable
                    .FirstOrDefaultAsync(r => r.OrderId == orderId && r.ProductId == productId && r.UserId == userId);

                if (existingReview != null)
                {
                    _logger.LogWarning("Người dùng #{UserId} đã đánh giá sản phẩm #{ProductId} trong đơn hàng #{OrderId}", userId, productId, orderId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi kiểm tra khả năng đánh giá sản phẩm trong đơn hàng");
                return false;
            }
        }

        /// <summary>
        /// Thêm đánh giá cho sản phẩm trong đơn hàng
        /// </summary>
        public async Task<ReviewResponseDTO?> AddReviewForOrderItemAsync(ReviewRequestDTO reviewRequest)
        {
            try
            {
                // Kiểm tra tính hợp lệ của đánh giá
                var canReview = await CanReviewOrderItemAsync(
                    reviewRequest.OrderId,
                    reviewRequest.ProductId,
                    reviewRequest.UserId);

                if (!canReview)
                {
                    _logger.LogWarning("Không thể đánh giá sản phẩm #{ProductId} trong đơn hàng #{OrderId}", 
                        reviewRequest.ProductId, reviewRequest.OrderId);
                    return null;
                }

                // Thêm đánh giá mới
                var review = MapToEntity(reviewRequest);
                if (review == null)
                {
                    _logger.LogWarning("Không thể chuyển đổi ReviewRequestDTO thành Review entity");
                    return null;
                }

                await _repository.AddAsync(review);

                // Cập nhật điểm đánh giá trung bình cho sản phẩm
                await CalculateAverageRatingAsync(reviewRequest.ProductId, reviewRequest.Rating);

                // Cập nhật thông tin ReviewStar cho sản phẩm
                await UpdateProductReviewStarsAsync(reviewRequest.ProductId, reviewRequest.Rating);

                return MapToDTO(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm đánh giá cho sản phẩm #{ProductId} trong đơn hàng #{OrderId}", 
                    reviewRequest.ProductId, reviewRequest.OrderId);
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách các sản phẩm trong đơn hàng mà người dùng có thể đánh giá
        /// </summary>
        public async Task<IEnumerable<OrderItemResponseDTO>> GetReviewableOrderItemsAsync(int orderId, int userId)
        {
            try
            {
                // Kiểm tra đơn hàng
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    _logger.LogWarning("Không tìm thấy đơn hàng #{OrderId}", orderId);
                    return Enumerable.Empty<OrderItemResponseDTO>();
                }

                // Kiểm tra người dùng có phải là người mua đơn hàng không
                if (order.BuyerId != userId)
                {
                    _logger.LogWarning("Người dùng #{UserId} không phải là người mua đơn hàng #{OrderId}", userId, orderId);
                    return Enumerable.Empty<OrderItemResponseDTO>();
                }

                // Kiểm tra đơn hàng đã hoàn thành chưa
                if (order.Status != OrderStatus.Completed && order.Status != OrderStatus.Delivered)
                {
                    _logger.LogWarning("Đơn hàng #{OrderId} chưa hoàn thành, trạng thái hiện tại: {Status}", orderId, order.Status);
                    return Enumerable.Empty<OrderItemResponseDTO>();
                }

                // Lấy danh sách sản phẩm trong đơn hàng
                var orderItems = await _orderRepository.GetOrderItemsAsync(orderId);
                var orderItemDTOs = orderItems.Select(item => item.ToResponseDTO()).ToList();

                // Lấy danh sách đánh giá đã có của người dùng cho đơn hàng này
                var queryable = await _reviewRepository.GetQueryableAsync();
                var existingReviews = await GetReviewsByUserIdAsync(userId);

                // Lọc ra các sản phẩm chưa được đánh giá
                var reviewableItems = orderItemDTOs
                    .Where(item => !existingReviews.Any(r => r.ProductId == item.ProductId))
                    .ToList();

                return reviewableItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách sản phẩm có thể đánh giá trong đơn hàng #{OrderId}", orderId);
                return Enumerable.Empty<OrderItemResponseDTO>();
            }
        }

        /// <summary>
        /// Lấy danh sách đánh giá của người dùng
        /// </summary>
        public async Task<IEnumerable<ReviewResponseDTO?>> GetReviewsByUserIdAsync(int userId)
        {
            try
            {
                var queryable = await _reviewRepository.GetQueryableAsync();
                var reviews = await queryable.Where(r => r.UserId == userId).ToListAsync();
                return reviews.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đánh giá của người dùng #{UserId}", userId);
                return Enumerable.Empty<ReviewResponseDTO>();
            }
        }

        /// <summary>
        /// Cập nhật thông tin ReviewStar cho sản phẩm
        /// </summary>
        private async Task UpdateProductReviewStarsAsync(int productId, int rating)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    return;
                }

                // Cập nhật số lượng đánh giá theo số sao
                switch (rating)
                {
                    case 1:
                        product.ReviewStar1Count++;
                        break;
                    case 2:
                        product.ReviewStar2Count++;
                        break;
                    case 3:
                        product.ReviewStar3Count++;
                        break;
                    case 4:
                        product.ReviewStar4Count++;
                        break;
                    case 5:
                        product.ReviewStar5Count++;
                        break;
                }

                await _productRepository.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin ReviewStar cho sản phẩm #{ProductId}", productId);
            }
        }

        #endregion
    }
}
