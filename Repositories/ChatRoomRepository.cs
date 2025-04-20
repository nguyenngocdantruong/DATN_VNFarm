using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm.Infrastructure.Persistence.Context;
using VNFarm_FinalFinal.Helpers;
using VNFarm_FinalFinal.Mappers;

namespace VNFarm.Infrastructure.Repositories
{
    public class ChatRoomRepository : BaseRepository<ChatRoom>, IChatRoomRepository
    {
        private readonly DbSet<Chat> _chatSet;
        
        public ChatRoomRepository(VNFarmContext context) : base(context)
        {
            _chatSet = context.Set<Chat>();
        }
        
        public async Task<IEnumerable<Chat>> GetChatsByRoomIdAsync(int roomId, int take = 20, int skip = 0)
        {
            return await _chatSet
                .Where(c => c.ChatRoomId == roomId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatRoom>> GetUserChatListAsync(int userId)
        {
            var rooms = await _dbSet
                .Where(r => (r.BuyerId == userId || r.SellerId == userId) && !r.IsDeleted)
                .OrderByDescending(r => r.LastMessageTime)
                .ToListAsync();
            return rooms;
        }

        public async Task<bool> SendMessageAsync(Chat chat)
        {
            try
            {
                chat.CreatedAt = DateTime.Now;
                await _chatSet.AddAsync(chat);
                
                var room = await _dbSet.FindAsync(chat.ChatRoomId);
                if (room != null)
                {
                    room.LastMessageTime = chat.CreatedAt;
                    room.LastMessage = chat.Content;
                    room.UpdatedAt = DateTime.Now;
                    _context.Entry(room).State = EntityState.Modified;
                }
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 