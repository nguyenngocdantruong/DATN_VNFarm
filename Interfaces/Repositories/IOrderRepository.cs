using VNFarm.Entities;
using VNFarm.Enums;

namespace VNFarm.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByStoreIdAsync(int storeId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<decimal> GetTotalRevenueByStoreIdAsync(int storeId);
        Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<OrderTimeline>> GetOrderTimelineAsync(int orderId);
        Task<OrderTimeline> AddOrderTimelineAsync(int orderId, OrderTimeline orderTimeline);
        #region Order Detail
        Task<OrderDetail> AddOrderDetailAsync(int orderId, OrderDetail orderDetail);
        Task<IEnumerable<OrderDetail>> GetOrderDetailAsync(int orderId);
        Task<bool> UpdateOrderDetailAsync(int orderId, OrderDetail orderDetail);
        #endregion
        #region Order Item
        Task<OrderItem> AddOrderItemAsync(int orderId, OrderItem orderItem);
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId);
        Task<bool> UpdateOrderItemAsync(int orderId, OrderItem orderItem);
        #endregion
    }
} 