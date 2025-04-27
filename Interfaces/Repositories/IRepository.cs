using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VNFarm.Entities;

namespace VNFarm.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IQueryable<T>> GetQueryableAsync(Expression<Func<T, bool>>? predicate = null);
        Task<T?> GetByIdAsync(int? id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> DeleteRangeAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> SaveChangesAsync();
    }
} 