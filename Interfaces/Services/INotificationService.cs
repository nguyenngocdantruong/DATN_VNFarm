using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Interfaces.Services
{
    public interface INotificationService : IService<Notification, NotificationRequestDTO, NotificationResponseDTO>
    {
        Task<IEnumerable<NotificationResponseDTO?>> GetByUserIdAsync(int userId);
        Task<bool> SendToUserAsync(int userId, string content, NotificationType type);
        Task<bool> SendToAllUsersAsync(string content, NotificationType type);
        Task<bool> DeleteAllForUserAsync(int userId);
    }
} 