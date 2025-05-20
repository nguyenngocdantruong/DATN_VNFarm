namespace VNFarm.Interfaces.Services
{
    public interface IUserConnectionService
    {
        void AddConnection(string userId, string connectionId);
        void RemoveConnection(string connectionId);
        bool IsUserOnline(string userId);
    }
}
