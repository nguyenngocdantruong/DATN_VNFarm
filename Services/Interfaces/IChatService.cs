using System.Threading.Tasks;
using VNFarm.DTOs.Request;
using VNFarm.Entities;

namespace VNFarm.Services.Interfaces
{
    public interface IChatService
    {
        Task<bool> SaveMessageAsync(ChatRequestDTO chatMessage);
        Task<IEnumerable<Chat>> GetChatHistoryAsync(string roomId, int take = 50);
    }
} 