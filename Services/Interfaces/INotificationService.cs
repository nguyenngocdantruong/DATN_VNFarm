using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Services.Interfaces
{
    public interface INotificationService : IService<Notification, NotificationRequestDTO, NotificationResponseDTO>
    {
        Task<IEnumerable<NotificationResponseDTO?>> GetByUserIdAsync(int userId);
        Task<bool> SendToUserAsync(int userId, string content, NotificationType type);
        Task<bool> SendToAllUsersAsync(string content, NotificationType type);
        Task<bool> DeleteAllForUserAsync(int userId);
    }
} 