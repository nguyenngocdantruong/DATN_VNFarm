using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Repositories;
using VNFarm.Interfaces.Services;
using VNFarm.Mappers;

namespace VNFarm.Services
{
    public class ContactRequestService : BaseService<ContactRequest, ContactRequestDTO, ContactRequestResponseDTO>, IContactRequestService
    {
        private readonly IContactRequestRepository _contactRequestRepository;
        private readonly ILogger<ContactRequestService> _logger;

        public ContactRequestService(
            IContactRequestRepository contactRequestRepository,
            ILogger<ContactRequestService> logger) : base(contactRequestRepository)
        {
            _contactRequestRepository = contactRequestRepository;
            _logger = logger;
        }

        protected override ContactRequestResponseDTO? MapToDTO(ContactRequest? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override ContactRequest? MapToEntity(ContactRequestDTO dto)
        {
            if (dto == null) return null;
            return dto.ToEntity();
        }

        public override async Task<bool> UpdateAsync(ContactRequestDTO dto)
        {
            try
            {
                var entity = await _contactRequestRepository.GetByIdAsync(dto.Id);
                if (entity == null) return false;

                entity.FullName = dto.FullName;
                entity.Email = dto.Email;
                entity.ServiceType = dto.ServiceType;
                entity.PhoneNumber = dto.PhoneNumber;
                entity.Message = dto.Message;

                await _contactRequestRepository.UpdateAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật thông tin yêu cầu liên hệ ID: {dto.Id}");
                return false;
            }
        }

        public override async Task<IQueryable<ContactRequest>> Query(IFilterCriteria filter)
        {
            var query = await _contactRequestRepository.GetQueryableAsync();
            
            if (filter is ContactRequestCriteriaFilter contactFilter)
            {
                // Áp dụng filter nếu có
                if (!string.IsNullOrEmpty(contactFilter.SearchTerm))
                {
                    query = query.Where(c => 
                        c.FullName.Contains(contactFilter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        c.Email.Contains(contactFilter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        c.PhoneNumber.Contains(contactFilter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        c.Message.Contains(contactFilter.SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(contactFilter.ServiceType))
                {
                    query = query.Where(c => c.ServiceType == contactFilter.ServiceType);
                }
            }
            
            return query;
        }

        public override async Task<IEnumerable<ContactRequestResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<ContactRequest> query, IFilterCriteria filter)
        {
            // Sắp xếp
            switch (filter.SortBy)
            {
                case Enums.SortType.Latest:
                    query = query.OrderByDescending(c => c.CreatedAt);
                    break;
                case Enums.SortType.Oldest:
                    query = query.OrderBy(c => c.CreatedAt);
                    break;
                case Enums.SortType.Ascending:
                    query = query.OrderBy(c => c.FullName);
                    break;
                case Enums.SortType.Descending:
                    query = query.OrderByDescending(c => c.FullName);
                    break;
                default:
                    query = query.OrderByDescending(c => c.CreatedAt);
                    break;
            }

            // Phân trang
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var contactRequests = await query.ToListAsync();
            return contactRequests.Select(MapToDTO);
        }

        public override async Task<IEnumerable<ContactRequestResponseDTO?>> QueryAsync(string query)
        {
            try
            {
                var contactRequests = await _contactRequestRepository.FindAsync(c =>
                    c.FullName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    c.PhoneNumber.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    c.Message.Contains(query, StringComparison.OrdinalIgnoreCase));

                return contactRequests.Select(MapToDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tìm kiếm yêu cầu liên hệ với query: {query}");
                return Enumerable.Empty<ContactRequestResponseDTO>();
            }
        }
    }
} 