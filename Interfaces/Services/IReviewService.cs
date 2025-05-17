using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;

namespace VNFarm.Interfaces.Services
{
    public interface IReviewService: IService<Review, ReviewRequestDTO, ReviewResponseDTO>
    {
        Task<IEnumerable<ReviewResponseDTO?>> GetReviewByOrderIdAsync(int orderId);
        Task<IEnumerable<ReviewResponseDTO?>> GetReviewsByProductIdAsync(int productId);
        Task<double> CalculateAverageRatingAsync(int productId, int rating);
    }
}
