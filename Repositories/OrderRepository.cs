using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm.Data;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Repositories.Interfaces;

namespace VNFarm.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(VNFarmContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(o => o.BuyerId == userId && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStoreIdAsync(int storeId)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .Where(o => 
                    o.Status == OrderStatus.Completed || o.Status == OrderStatus.Delivered &&
                    o.PaymentStatus == PaymentStatus.Paid &&
                    o.OrderItems.Any(item => item.Product != null && item.Product.StoreId == storeId) && 
                    !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _dbSet
                .Where(o => o.Status == status && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _dbSet.FindAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            order.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> GetTotalRevenueByStoreIdAsync(int storeId)
        {
            return await _dbSet
                .Where(o => o.OrderItems.Any(item => item.Product != null && item.Product.StoreId == storeId) && 
                       o.Status == OrderStatus.Completed && 
                       !o.IsDeleted)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(o => o.CreatedAt >= startDate && 
                       o.CreatedAt <= endDate && 
                       o.Status == OrderStatus.Completed && 
                       !o.IsDeleted)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<IEnumerable<OrderTimeline>> GetOrderTimelineAsync(int orderId)
        {
            return await _context.OrderTimelines
                .Where(t => t.OrderId == orderId)
                .OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        public async Task<OrderTimeline> AddOrderTimelineAsync(int orderId, OrderTimeline orderTimeline)
        {
            orderTimeline.OrderId = orderId;
            await _context.OrderTimelines.AddAsync(orderTimeline);
            await _context.SaveChangesAsync();
            return orderTimeline;
        }

        //public async Task<OrderDetail> AddOrderDetailAsync(int orderId, OrderDetail orderDetail)
        //{
        //    orderDetail.OrderId = orderId;
        //    await _context.OrderDetails.AddAsync(orderDetail);
        //    await _context.SaveChangesAsync();
        //    return orderDetail;
        //}

        //public async Task<IEnumerable<OrderDetail>> GetOrderDetailAsync(int orderId)
        //{
        //    return await _context.OrderDetails
        //        .Where(o => o.OrderId == orderId)
        //        .ToListAsync();
        //}

        //public async Task<bool> UpdateOrderDetailAsync(int orderId, OrderDetail orderDetail)
        //{
        //    orderDetail.OrderId = orderId;
        //    _context.OrderDetails.Update(orderDetail);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
        
        #region Order Item
        public async Task<OrderItem> AddOrderItemAsync(int orderId, OrderItem orderItem)
        {
            orderItem.OrderId = orderId;
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Shop)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderItemAsync(int orderId, OrderItem orderItem)
        {
            orderItem.OrderId = orderId;
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
} 