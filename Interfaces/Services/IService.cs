using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;

namespace VNFarm_FinalFinal.Interfaces.Services
{
    public interface IService<TEntity, TReq, TRes> where TEntity : BaseEntity where TReq : BaseRequestDTO where TRes : BaseResponseDTO
    {
        Task<IEnumerable<TRes?>> GetAllAsync();
        Task<TRes?> GetByIdAsync(int id);
        Task<TRes?> AddAsync(TReq dto);
        Task<bool> UpdateAsync(TReq dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<IEnumerable<TRes?>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> Query(IFilterCriteria filter);
        Task<IEnumerable<TRes?>> ApplyPagingAndSortingAsync(IQueryable<TEntity> query, IFilterCriteria filter);
        Task<IEnumerable<TRes?>> QueryAsync(string query);
    }
} 