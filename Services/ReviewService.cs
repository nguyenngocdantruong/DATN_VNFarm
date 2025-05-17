using Microsoft.EntityFrameworkCore;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Interfaces.Repositories;
using VNFarm.Interfaces.Services;
using VNFarm.Mappers;

namespace VNFarm.Services
{
    public class ReviewService : BaseService<Review, ReviewRequestDTO, ReviewResponseDTO>, IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public ReviewService(IReviewRepository reviewRepository, IOrderRepository orderRepository, IProductRepository productRepository) : base(reviewRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
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
            if(filter is ReviewFilterCriteria reviewFilterCriteria)
            {
                if(reviewFilterCriteria.ProductId.HasValue)
                {
                    query = query.Where(x => x.ProductId == reviewFilterCriteria.ProductId);
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
            product.AverageRating = newRating;
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
    }
}
