using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;
using VNFarm.Mappers;
using VNFarm.Repositories.Interfaces;
using VNFarm.Services.Interfaces;

namespace VNFarm.Services
{
    public class ChatRoomService : BaseService<ChatRoom, ChatRoomRequestDTO, ChatRoomResponseDTO>, IChatRoomService
    {
        #region Fields & Constructor
        private readonly IChatRoomRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<ChatRoom> _chatRoomRepository;
        private readonly ILogger<ChatRoomService> _logger;

        public ChatRoomService(
            IChatRoomRepository chatRepository,
            IUserRepository userRepository,
            IRepository<ChatRoom> chatRoomRepository,
            ILogger<ChatRoomService> logger) : base(chatRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _chatRoomRepository = chatRoomRepository;
            _logger = logger;
        }
        #endregion

        #region Base Service Implementation
        protected override ChatRoomResponseDTO? MapToDTO(ChatRoom? entity)
        {
            return entity == null ? null : entity.ToResponseDTO();
        }

        protected override ChatRoom? MapToEntity(ChatRoomRequestDTO dto)
        {
            return dto == null ? null : dto.ToEntity();
        }

        private Chat ToChatEntity(ChatRequestDTO chatDto)
        {
            return chatDto.ToEntity();
        }

        public override async Task<bool> UpdateAsync(ChatRoomRequestDTO dto)
        {
            var old = await _chatRoomRepository.GetByIdAsync(dto.Id);
            if (old == null) return false;

            try
            {
                old.UpdateFromRequestDto(dto);
                return await _chatRoomRepository.UpdateAsync(old);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating chat room: {Message}", ex.Message);
                return false;
            }
        }

        public override Task<IEnumerable<ChatRoomResponseDTO?>> QueryAsync(string query)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Chat Operations
        public async Task<bool> SendMessageAsync(ChatRequestDTO message)
        {
            try
            {
                var chatMessage = ToChatEntity(message);
                return await _chatRepository.SendMessageAsync(chatMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<ChatResponseDTO?>> GetChatsByRoomIdAsync(int roomId, int take = 20, int skip = 0)
        {
            var chatHistories = await _chatRepository.GetChatsByRoomIdAsync(roomId, take, skip);
            return chatHistories.Select(c => c.ToResponseDTO());
        }
        
        // Từ ChatService
        public async Task<IEnumerable<Chat>> GetChatHistoryAsync(string roomId, int take = 50)
        {
            try
            {
                if (!int.TryParse(roomId, out int roomIdInt))
                {
                    _logger.LogError("Invalid roomId format: {roomId}", roomId);
                    return Enumerable.Empty<Chat>();
                }

                return await _chatRepository.GetChatsByRoomIdAsync(roomIdInt, take);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chat history: {Message}", ex.Message);
                return Enumerable.Empty<Chat>();
            }
        }
        
        // Phương thức mới - tạo phòng chat
        public async Task<ChatRoomResponseDTO?> CreateChatRoomAsync(CreateChatRoomRequestDTO request)
        {
            try
            {
                // Kiểm tra người dùng tồn tại
                var user1 = await _userRepository.GetByIdAsync(request.TargetUserId);
                var user2 = await _userRepository.GetByIdAsync(request.CurrentUserId);
                if (user1 == null)
                {
                    _logger.LogError("User not found. User1: {UserId1}", request.TargetUserId);
                    return null;
                }
                
                // Kiểm tra vai trò, ai là buyer, ai là seller
                bool user1IsSeller = user1.Role == UserRole.Seller;
                
                int buyerId, sellerId;
                if (user1IsSeller)
                {
                    sellerId = request.TargetUserId;
                    buyerId = request.CurrentUserId;
                }
                else
                {
                    buyerId = request.TargetUserId;
                    sellerId = request.CurrentUserId;
                }
                
                
                // Kiểm tra xem đã tồn tại phòng chat giữa 2 người này chưa
                var existingChatRooms = await _chatRoomRepository.FindAsync(
                    r => ((r.BuyerId == buyerId && r.SellerId == sellerId) || 
                        (r.BuyerId == sellerId && r.SellerId == buyerId)) && 
                        !r.IsDeleted);
                        
                if (existingChatRooms.Any())
                {
                    // Trả về phòng chat đã tồn tại
                    return existingChatRooms.First().ToResponseDTO();
                }
                
                // Tạo phòng chat mới
                var chatRoom = new ChatRoom
                {
                    NameRoom = string.IsNullOrEmpty(request.RoomName) ? $"Chat {user1.FullName} - {user2.FullName}" : request.RoomName,
                    Description = request.Description,
                    BuyerId = buyerId,
                    SellerId = sellerId,
                    Type = ChatRoomType.ChatNormal,
                    Status = ChatRoomStatus.InProgress,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true
                };
                
                await _chatRoomRepository.AddAsync(chatRoom);
                return chatRoom.ToResponseDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat room: {Message}", ex.Message);
                return null;
            }
        }
        #endregion

        #region ChatRoom Query Methods
        public async Task<IEnumerable<ChatRoomResponseDTO>> GetUserChatListAsync(int userId)
        {
            try
            {
                var chatList = await _chatRepository.GetUserChatListAsync(userId);
                return chatList.Select(c => c.ToResponseDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user chat list: {Message}", ex.Message);
                return new List<ChatRoomResponseDTO>();
            }
        }

        public async override Task<IQueryable<ChatRoom>> Query(IFilterCriteria filter)
        {
            var query = await _chatRoomRepository.GetQueryableAsync();
            if (filter is ChatRoomCriteriaFilter chatRoomCriteriaFilter)
            {
                // Apply search filter
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    query = query.Where(c =>
                        c.NameRoom.Contains(filter.SearchTerm) ||
                        c.Description.Contains(filter.SearchTerm)
                    );
                }

                // Apply type filter
                if (chatRoomCriteriaFilter.Type != null)
                {
                    query = query.Where(c => c.Type == chatRoomCriteriaFilter.Type);
                }

                // Apply status filter
                if (chatRoomCriteriaFilter.Status != null)
                {
                    query = query.Where(c => c.Status == chatRoomCriteriaFilter.Status);
                }

                // Apply date filters
                if (chatRoomCriteriaFilter.StartDate != null)
                {
                    query = query.Where(c => c.UpdatedAt >= chatRoomCriteriaFilter.StartDate);
                }
                if (chatRoomCriteriaFilter.EndDate != null)
                {
                    query = query.Where(c => c.UpdatedAt <= chatRoomCriteriaFilter.EndDate);
                }

                // Apply user filter
                if (chatRoomCriteriaFilter.UserId != null)
                {
                    query = query.Where(c => c.BuyerId == chatRoomCriteriaFilter.UserId || c.SellerId == chatRoomCriteriaFilter.UserId);
                }
            }
            return query;
        }

        public async override Task<IEnumerable<ChatRoomResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<ChatRoom> query, IFilterCriteria filter)
        {
            if (filter is ChatRoomCriteriaFilter chatRoomCriteriaFilter)
            {
                // Apply sorting
                switch (chatRoomCriteriaFilter.SortBy)
                {
                    case SortType.Ascending:
                        query = query.OrderBy(c => c.UpdatedAt);
                        break;
                    case SortType.Descending:
                        query = query.OrderByDescending(c => c.UpdatedAt);
                        break;
                }

            }
            // Apply paging
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            return (await query.ToListAsync()).Select(MapToDTO);
        }

        #endregion
    }
}