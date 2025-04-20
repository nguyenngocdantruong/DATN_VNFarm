using VNFarm_FinalFinal.Entities;

namespace VNFarm_FinalFinal.Interfaces.Repositories
{
    public interface IChatRoomRepository : IRepository<ChatRoom>
    {
        Task<IEnumerable<Chat>> GetChatsByRoomIdAsync(int roomId, int take = 20, int skip = 0);
        Task<IEnumerable<ChatRoom>> GetUserChatListAsync(int userId);
        Task<bool> SendMessageAsync(Chat chat);
    }
} 