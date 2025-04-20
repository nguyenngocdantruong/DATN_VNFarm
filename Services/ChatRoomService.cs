using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Helpers;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm_FinalFinal.Interfaces.Services;
using VNFarm_FinalFinal.Mappers;

namespace VNFarm.Infrastructure.Services
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

        public async Task<IEnumerable<Chat>> GetChatsByRoomIdAsync(int roomId, int take = 20, int skip = 0)
        {
            var chatHistories = await _chatRepository.GetChatsByRoomIdAsync(roomId, take, skip);
            return chatHistories;
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

                // Apply date filters
                if (chatRoomCriteriaFilter.StartDate != null)
                {
                    query = query.Where(c => c.UpdatedAt >= chatRoomCriteriaFilter.StartDate);
                }
                if (chatRoomCriteriaFilter.EndDate != null)
                {
                    query = query.Where(c => c.UpdatedAt <= chatRoomCriteriaFilter.EndDate);
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