using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Interfaces.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<bool> SendToUserAsync(int userId, string content, NotificationType type);
        Task<bool> SendToAllUsersAsync(string content, NotificationType type);
        Task<bool> DeleteAllForUserAsync(int userId);
    }
} 