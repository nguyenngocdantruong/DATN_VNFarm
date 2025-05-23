using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;
using VNFarm.Mappers;
using VNFarm.DTOs.Filters;
using Microsoft.EntityFrameworkCore;
using VNFarm.Data;
using VNFarm.Repositories.Interfaces;
using VNFarm.Services.Interfaces;

namespace VNFarm.Services
{
    public class NotificationService : BaseService<Notification, NotificationRequestDTO, NotificationResponseDTO>, INotificationService
    {
        #region Fields & Constructor
        private readonly INotificationRepository _notificationRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<NotificationService> _logger;
        private readonly VNFarmContext _context;

        public NotificationService(
            INotificationRepository notificationRepository, 
            IUserRepository userRepository, 
            IStoreRepository storeRepository,
            ILogger<NotificationService> logger, 
            VNFarmContext context) : base(notificationRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _storeRepository = storeRepository;
            _logger = logger;
            _context = context;
        }
        #endregion

        #region Base Service Implementation
        protected override NotificationResponseDTO? MapToDTO(Notification? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override Notification? MapToEntity(NotificationRequestDTO dto)
        {
            return dto.ToEntity();
        }

        public override async Task<bool> UpdateAsync(NotificationRequestDTO dto)
        {
            var old = await _notificationRepository.GetByIdAsync(dto.Id);
            if (old == null) return false;

            old.UpdateFromRequestDto(dto);
            return await _notificationRepository.UpdateAsync(old);
        }

        public override async Task<IEnumerable<NotificationResponseDTO?>> QueryAsync(string query)
        {
            var notifications = await _notificationRepository.FindAsync(
                n => n.Content.Contains(query)
            );
            return notifications.Select(MapToDTO).ToList();
        }
        #endregion

        #region Query Methods
        public async Task<IEnumerable<NotificationResponseDTO?>> GetByUserIdAsync(int userId)
        {
            var notifications = await _notificationRepository.FindAsync(
                n => n.UserId == userId
            );
            return notifications.Select(MapToDTO).ToList();
        }
        #endregion

        #region Notification Operations
        public async Task<bool> SendToUserAsync(int userId, string content, NotificationType type)
        {
            return await _notificationRepository.SendToUserAsync(userId, content, type);
        }

        public async Task<bool> SendToAllUsersAsync(string content, NotificationType type)
        {
            return await _notificationRepository.SendToAllUsersAsync(content, type);
        }

        public async Task<bool> DeleteAllForUserAsync(int userId)
        {
            var notifications = await _notificationRepository.FindAsync(
                n => n.UserId == userId
            );
            if (!notifications.Any()) return false;
            
            return await _notificationRepository.DeleteRangeAsync(notifications) > 0;
        }

        public override async Task<IQueryable<Notification>> Query(IFilterCriteria filter)
        {
            var query = await _notificationRepository.GetQueryableAsync();
            // Apply search filter
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(n => n.Content.Contains(filter.SearchTerm));
            }
            if(filter is NotificationCriteriaFilter notificationFilter){
                if(notificationFilter.UserId != null){
                    query = query.Where(n => n.UserId == notificationFilter.UserId);
                }
                if(notificationFilter.StoreId != null){
                    var store = await _storeRepository.GetByIdAsync(notificationFilter.StoreId.Value);
                    int userId = store?.UserId ?? 0;
                    if(userId > 0){
                        query = query.Where(n => n.UserId == userId);
                    }
                }
            }
            return query;
        }

        public async override Task<IEnumerable<NotificationResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Notification> query, IFilterCriteria filter)
        {
            // Apply sorting
            switch(filter.SortBy){
                case SortType.Latest:
                    query = query.OrderByDescending(n => n.CreatedAt);
                    break;
                case SortType.Oldest:
                    query = query.OrderBy(n => n.CreatedAt);
                    break;
                default:
                    query = query.OrderByDescending(n => n.UpdatedAt);
                    break;
            }
            // Apply paging
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            //Mark all notifications in query as read
            List<Notification> notifications = [];
            foreach(var notification in query){
                if(notification.IsRead == false){
                    notifications.Add(notification);
                }
            }
            await _notificationRepository.UpdateRangeAsync(notifications);
            return (await query.ToListAsync()).Select(MapToDTO);
        }
        #endregion
    }
}