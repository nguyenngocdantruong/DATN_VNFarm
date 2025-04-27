using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;
using VNFarm.Interfaces.Repositories;
using VNFarm.Interfaces.Services;
using VNFarm.Mappers;

namespace VNFarm.Services
{
    public class DiscountService : BaseService<Discount, DiscountRequestDTO, DiscountResponseDTO>, IDiscountService
    {
        #region Fields & Constructor
        private readonly IDiscountRepository _discountRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;

        public DiscountService(
            IDiscountRepository _repository,
            IStoreRepository storeRepository,
            IUserRepository userRepository) : base(_repository)
        {
            _discountRepository = _repository;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region Base Service Implementation
        protected override DiscountResponseDTO? MapToDTO(Discount? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override Discount? MapToEntity(DiscountRequestDTO dto)
        {
            return dto.ToEntity();
        }

        public override async Task<bool> UpdateAsync(DiscountRequestDTO dto)
        {
            var old = await _discountRepository.GetByIdAsync(dto.Id);
            if (old == null) return false;
            old.UpdateFromRequestDto(dto);
            return await _discountRepository.UpdateAsync(old);
        }

        public override async Task<IEnumerable<DiscountResponseDTO?>> QueryAsync(string query)
        {
            var discounts = await _discountRepository.FindAsync(
                d => d.Code.Contains(query) || d.Description.Contains(query)
            );
            return discounts.Select(MapToDTO).ToList();
        }
        #endregion

        #region Discount Query Methods
        public async Task<DiscountResponseDTO?> GetByCodeAsync(string code)
        {
            var discount = await _discountRepository.GetByCodeAsync(code);
            return MapToDTO(discount);
        }

        public async Task<IEnumerable<DiscountResponseDTO?>> GetDiscountsByStatusAsync(DiscountStatus status)
        {
            var discounts = await _discountRepository.FindAsync(d => d.Status == status);
            return discounts.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<DiscountResponseDTO?>> GetDiscountsByStoreIdAsync(int storeId)
        {
            var discounts = await _discountRepository.FindAsync(d => d.StoreId == storeId);
            return discounts.Select(MapToDTO).ToList();
        }

        public async Task<IEnumerable<DiscountResponseDTO?>> GetExpiredDiscountsAsync()
        {
            var now = DateTime.UtcNow;
            var discounts = await _discountRepository.FindAsync(d => d.EndDate < now);
            return discounts.Select(MapToDTO).ToList();
        }

        public async override Task<IQueryable<Discount>> Query(IFilterCriteria filter)
        {
            var query = await _discountRepository.GetQueryableAsync();

            if (filter is DiscountCriteriaFilter discountCriteriaFilter)
            {
                // Apply status filter
                if (discountCriteriaFilter.Status != DiscountStatus.All)
                {
                    query = query.Where(d => d.Status == discountCriteriaFilter.Status);
                }

                // Apply type filter
                if (discountCriteriaFilter.Type != DiscountType.All)
                {
                    query = query.Where(d => d.Type == discountCriteriaFilter.Type);
                }

                // Apply date discountCriteriaFilters
                if (discountCriteriaFilter.StartDate.HasValue)
                {
                    query = query.Where(d => d.StartDate >= discountCriteriaFilter.StartDate.Value);
                }
                if (discountCriteriaFilter.EndDate.HasValue)
                {
                    query = query.Where(d => d.EndDate <= discountCriteriaFilter.EndDate.Value);
                }
            }
            // Apply search filter
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(d =>
                    d.Code.Contains(filter.SearchTerm) ||
                    d.Description.Contains(filter.SearchTerm)
                );
            }
            return query;
        }

        public override async Task<IEnumerable<DiscountResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Discount> query, IFilterCriteria filter)
        {
            if (filter is DiscountCriteriaFilter discountCriteriaFilter)
            {
                // Apply sorting
                query = discountCriteriaFilter.SortBy == SortType.Descending
                    ? query.OrderByDescending(d => d.StartDate)
                    : query.OrderBy(d => d.StartDate);
            }
            else
            {
                throw new ArgumentException("Filter truyền vào không phải là DiscountCriteriaFilter");
            }
            // Apply paging
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            return (await query.ToListAsync()).Select(MapToDTO);
        }
        #endregion

        #region Discount Operations
        public async Task<bool> DecrementQuantityAsync(int discountId)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);
            if (discount == null) return false;

            discount.RemainingQuantity--;
            await _discountRepository.UpdateAsync(discount);
            return true;
        }

        public async Task<bool> ToggleStatusAsync(int discountId, DiscountStatus status)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);
            if (discount == null) return false;

            discount.Status = status;
            discount.UpdatedAt = DateTime.UtcNow;

            await _discountRepository.UpdateAsync(discount);
            return true;
        }

        public async Task<bool> IsDiscountValidAsync(string code, int? userId, int? storeId)
        {
            var discount = await _discountRepository.GetByCodeAsync(code);
            if (discount == null || discount.RemainingQuantity <= 0) return false;

            var now = DateTime.UtcNow;
            if (discount.StartDate > now || discount.EndDate < now) return false;
            if (storeId.HasValue && discount.StoreId != storeId.Value) return false;

            return true;
        }
        #endregion
    }
}