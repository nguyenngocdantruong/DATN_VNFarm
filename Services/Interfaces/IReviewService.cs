using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;

namespace VNFarm.Services.Interfaces
{
    public interface IReviewService: IService<Review, ReviewRequestDTO, ReviewResponseDTO>
    {
        Task<IEnumerable<ReviewResponseDTO?>> GetReviewByOrderIdAsync(int orderId);
        Task<IEnumerable<ReviewResponseDTO?>> GetReviewsByProductIdAsync(int productId);
        Task<double> CalculateAverageRatingAsync(int productId, int rating);
        
        // Các phương thức mới để hỗ trợ đánh giá đơn hàng dựa trên OrderItem
        Task<bool> CanReviewOrderItemAsync(int orderId, int productId, int userId);
        Task<ReviewResponseDTO?> AddReviewForOrderItemAsync(ReviewRequestDTO reviewRequest);
        Task<IEnumerable<OrderItemResponseDTO>> GetReviewableOrderItemsAsync(int orderId, int userId);
        Task<IEnumerable<ReviewResponseDTO?>> GetReviewsByUserIdAsync(int userId);
    }
}
