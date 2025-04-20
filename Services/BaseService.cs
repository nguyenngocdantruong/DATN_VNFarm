using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm_FinalFinal.Interfaces.Services;

namespace VNFarm.Infrastructure.Services
{
    public abstract class BaseService<TEntity, TReq, TRes> : IService<TEntity, TReq, TRes> where TEntity : BaseEntity where TReq : BaseRequestDTO where TRes : BaseResponseDTO
    {
        protected readonly IRepository<TEntity> _repository;
        
        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        #region Mapping Methods
        protected abstract TRes? MapToDTO(TEntity? entity);
        protected abstract TEntity? MapToEntity(TReq dto);
        #endregion

        #region CRUD Operations
        public virtual async Task<TRes?> AddAsync(TReq? dto)
        {
            if (dto == null) return default;
            var entity = MapToEntity(dto);
            if (entity == null)
                return default;
                
            var addedEntity = await _repository.AddAsync(entity);
            return MapToDTO(addedEntity);
        }

        public virtual async Task<TRes?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return default;
                
            return MapToDTO(entity);
        }

        public virtual async Task<IEnumerable<TRes?>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDTO);
        }
        
        public abstract Task<bool> UpdateAsync(TReq dto);
        
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return false;
                
            await _repository.DeleteAsync(entity);
            return true;
        }
        #endregion

        #region Query & Filter Operations
        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _repository.ExistsAsync(id);
        }

        public abstract Task<IEnumerable<TRes?>> QueryAsync(string query);

        public virtual async Task<IEnumerable<TRes?>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _repository.FindAsync(predicate);
            return entities.Select(MapToDTO);
        }
        public abstract Task<IQueryable<TEntity>> Query(IFilterCriteria filter);
        
        #endregion

        #region Paging & Counting
        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return _repository.CountAsync(predicate);
        }

        public abstract Task<IEnumerable<TRes?>> ApplyPagingAndSortingAsync(IQueryable<TEntity> query, IFilterCriteria filter);
        #endregion
    }
} 