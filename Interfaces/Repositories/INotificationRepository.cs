using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<bool> SendToUserAsync(int userId, string content, NotificationType type);
        Task<bool> SendToAllUsersAsync(string content, NotificationType type);
        Task<bool> DeleteAllForUserAsync(int userId);
    }
} 