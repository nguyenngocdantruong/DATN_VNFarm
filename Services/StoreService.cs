using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.DTOs.Filters;
using VNFarm_FinalFinal.Entities;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm_FinalFinal.Interfaces.Services;
using VNFarm_FinalFinal.Enums;
using VNFarm_FinalFinal.Helpers;
using VNFarm_FinalFinal.Mappers;

namespace VNFarm.Infrastructure.Services
{
    public class StoreService : BaseService<Store, StoreRequestDTO, StoreResponseDTO>, IStoreService
    {
        #region Fields & Constructor
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<StoreService> _logger;

        public StoreService(
            IStoreRepository storeRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            ILogger<StoreService> logger) : base(storeRepository)
        {
            _storeRepository = storeRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _logger = logger;
        }
        #endregion

        #region Base Service Implementation
        protected override StoreResponseDTO? MapToDTO(Store? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override Store? MapToEntity(StoreRequestDTO dto)
        {
            if (dto == null) return null;
            return dto.ToEntity(-1);
        }

        public override async Task<bool> UpdateAsync(StoreRequestDTO storeDto)
        {
            try
            {
                var store = await _storeRepository.GetByIdAsync(storeDto.Id);
                if (store == null)
                    return false;

                if (storeDto.LogoFile != null)
                {
                    storeDto.LogoUrl = await FileUpload.UploadFile(storeDto.LogoFile, FileUpload.StoreFolder);
                }

                store.UpdateFromRequestDto(storeDto);
                return await _storeRepository.UpdateAsync(store);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật cửa hàng ID: {storeDto.Id}");
                return false;
            }
        }

        public override async Task<IEnumerable<StoreResponseDTO?>> QueryAsync(string query)
        {
            var stores = await _storeRepository.FindAsync(s =>
                s.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                s.Description.Contains(query, StringComparison.OrdinalIgnoreCase)
            );
            return stores.Select(MapToDTO);
        }
        #endregion

        #region Store Query Methods
        public async Task<StoreResponseDTO?> GetStoreByUserIdAsync(int userId)
        {
            try
            {
                var store = await _storeRepository.GetStoreByUserIdAsync(userId);
                return MapToDTO(store);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin cửa hàng của người dùng ID: {userId}");
                return null;
            }
        }

        public async Task<IEnumerable<StoreResponseDTO?>> GetRecentlyAddedStoresAsync(int count)
        {
            try
            {
                var stores = await _storeRepository.GetRecentlyAddedStoresAsync(count);
                return stores.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy danh sách cửa hàng mới thêm, số lượng: {count}");
                return new List<StoreResponseDTO>();
            }
        }

        public async Task<IEnumerable<StoreResponseDTO?>> GetStoresByVerificationStatusAsync(StoreStatus status)
        {
            try
            {
                var stores = await _storeRepository.GetStoresByVerificationStatusAsync(status);
                return stores.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy danh sách cửa hàng theo trạng thái: {status}");
                return new List<StoreResponseDTO>();
            }
        }

        public async override Task<IQueryable<Store>> Query(IFilterCriteria filter)
        {
            var query = await _storeRepository.GetQueryableAsync();
            if (filter is StoreCriteriaFilter storeCriteriaFilter)
            {
                // Apply search filter
                if (!string.IsNullOrEmpty(storeCriteriaFilter.SearchTerm))
                {
                    query = query.Where(s =>
                        s.Name.Contains(storeCriteriaFilter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        s.Description.Contains(storeCriteriaFilter.SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                // Apply status storeCriteriaFilter
                if (storeCriteriaFilter.Status.HasValue && storeCriteriaFilter.Status.Value != StoreStatus.All)
                {
                    query = query.Where(s => s.VerificationStatus == storeCriteriaFilter.Status.Value);
                }

                // Apply type storeCriteriaFilter
                if (storeCriteriaFilter.Type.HasValue && storeCriteriaFilter.Type.Value != StoreType.All)
                {
                    query = query.Where(s => s.BusinessType == storeCriteriaFilter.Type.Value);
                }

                // Apply rating storeCriteriaFilters
                if (storeCriteriaFilter.MinRating.HasValue)
                {
                    query = query.Where(s => s.AverageRating >= storeCriteriaFilter.MinRating.Value);
                }
                if (storeCriteriaFilter.MaxRating.HasValue)
                {
                    query = query.Where(s => s.AverageRating <= storeCriteriaFilter.MaxRating.Value);
                }
            }
            return query;
        }

        public async override Task<IEnumerable<StoreResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Store> query, IFilterCriteria filter)
        {
            // Apply sorting
            query = filter.SortBy switch
            {
                SortType.Latest => query.OrderByDescending(s => s.UpdatedAt),
                SortType.Oldest => query.OrderBy(s => s.UpdatedAt),
                SortType.Ascending => query.OrderBy(s => s.Name),
                SortType.Descending => query.OrderByDescending(s => s.Name),
                _ => query.OrderByDescending(s => s.UpdatedAt)
            };

            // Apply paging
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);

            return (await query.ToListAsync()).Select(MapToDTO);
        }
        #endregion

        #region Store Management
        public async Task<bool> VerifyStoreAsync(int storeId)
        {
            try
            {
                var store = await _storeRepository.GetByIdAsync(storeId);
                if (store == null)
                    return false;

                store.VerificationStatus = StoreStatus.Verified;
                store.UpdatedAt = DateTime.UtcNow;
                await _storeRepository.UpdateAsync(store);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xác thực cửa hàng ID: {storeId}");
                return false;
            }
        }

        public async Task<bool> RejectStoreAsync(int storeId, string reason)
        {
            try
            {
                var store = await _storeRepository.GetByIdAsync(storeId);
                if (store == null)
                    return false;

                store.VerificationStatus = StoreStatus.Rejected;
                store.UpdatedAt = DateTime.UtcNow;
                await _storeRepository.UpdateAsync(store);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi từ chối cửa hàng ID: {storeId}");
                return false;
            }
        }

        public async Task<bool> SetStoreStatusAsync(int storeId, StoreStatus status)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
                return false;

            store.VerificationStatus = status;
            return await _storeRepository.UpdateAsync(store);
        }

        public async Task<bool> SetStoreActiveAsync(int storeId, bool isActive)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
                return false;

            store.IsActive = isActive;
            return await _storeRepository.UpdateAsync(store);
        }


        #endregion
    }
}