using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;

namespace VNFarm_FinalFinal.Interfaces.Services
{
    public interface IChatRoomService : IService<ChatRoom, ChatRoomRequestDTO, ChatRoomResponseDTO>
    {
        Task<IEnumerable<Chat>> GetChatsByRoomIdAsync(int roomId, int take = 20, int skip = 0);
        Task<IEnumerable<ChatRoomResponseDTO>> GetUserChatListAsync(int userId);
        Task<bool> SendMessageAsync(ChatRequestDTO chat);
    }
} 