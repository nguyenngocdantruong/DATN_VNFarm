using Microsoft.EntityFrameworkCore;
using VNFarm.Infrastructure.Persistence.Context;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Interfaces.Repositories;

namespace VNFarm.Infrastructure.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(VNFarmContext context) : base(context)
        {
            
        }
        
        public async Task<bool> DeleteAllForUserAsync(int userId)
        {
            var notifications = _context.Notifications.Where(n => n.UserId == userId);
            _context.Notifications.RemoveRange(notifications);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId || n.UserId == -1).ToListAsync();
        }

        public async Task<bool> SendToAllUsersAsync(string content, NotificationType type)
        {
            var notification = new Notification
            {
                UserId = -1, 
                Content = content,
                Type = type,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SendToUserAsync(int userId, string content, NotificationType type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Content = content,
                Type = type,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}