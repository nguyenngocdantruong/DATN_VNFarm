using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;

namespace VNFarm.Services.Interfaces
{
    public interface IChatRoomService : IService<ChatRoom, ChatRoomRequestDTO, ChatRoomResponseDTO>
    {
        Task<IEnumerable<ChatResponseDTO>> GetChatsByRoomIdAsync(int roomId, int take = 20, int skip = 0);
        Task<IEnumerable<ChatRoomResponseDTO>> GetUserChatListAsync(int userId);
        Task<bool> SendMessageAsync(ChatRequestDTO chat);
        Task<ChatRoomResponseDTO?> CreateChatRoomAsync(CreateChatRoomRequestDTO request);
    }
} 