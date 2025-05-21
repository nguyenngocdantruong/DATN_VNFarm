using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.DTOs.Filters;
using VNFarm.Entities;
using VNFarm.Enums;
using VNFarm.Helpers;
using VNFarm.Mappers;
using VNFarm.Repositories.Interfaces;
using VNFarm.Services.Interfaces;
using VNFarm.Services.External.Interfaces;

namespace VNFarm.Services
{
    public class UserService : BaseService<User, UserRequestDTO, UserResponseDTO>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IEmailService _emailService;
        public UserService(
            IUserRepository userRepository,
            IEmailService emailService,
            ILogger<UserService> logger) : base(userRepository)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<UserResponseDTO?> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                return MapToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin người dùng theo email: {email}");
                return null;
            }
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            try
            {
                return await _userRepository.IsEmailUniqueAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi kiểm tra tính duy nhất của email: {email}");
                return false;
            }
        }

        public override async Task<IQueryable<User>> Query(IFilterCriteria filter)
        {
            var query = await _userRepository.GetQueryableAsync();
            if (filter is UserCriteriaFilter userCriteriaFilter)
            {
                // Áp dụng bộ lọc
                if (!string.IsNullOrEmpty(userCriteriaFilter.SearchTerm))
                {
                    userCriteriaFilter.SearchTerm = userCriteriaFilter.SearchTerm.Trim().ToLower();
                    query = query.Where(u =>
                        u.FullName.ToLower().Contains(userCriteriaFilter.SearchTerm) ||
                        u.Email.ToLower().Contains(userCriteriaFilter.SearchTerm) ||
                        u.PhoneNumber.ToLower().Contains(userCriteriaFilter.SearchTerm));
                }

                // Lọc theo vai trò người dùng
                if (userCriteriaFilter.Role != UserRole.All)
                {
                    query = query.Where(u => u.Role == userCriteriaFilter.Role);
                }

                // Lọc theo trạng thái hoạt động
                if (userCriteriaFilter.IsActive.HasValue)
                {
                    query = query.Where(u => u.IsActive == userCriteriaFilter.IsActive.Value);
                }

                // Lọc theo trạng thái xác thực email
                // if (userCriteriaFilter.EmailVerified.HasValue)
                // {
                //     query = query.Where(u => u.EmailVerified == userCriteriaFilter.EmailVerified.Value);
                // }
            }
            return query;
        }

        public override async Task<IEnumerable<UserResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<User> query, IFilterCriteria filter)
        {
            // Sắp xếp
            switch (filter.SortBy)
            {
                case SortType.Latest:
                    query = query.OrderByDescending(u => u.CreatedAt);
                    break;
                case SortType.Oldest:
                    query = query.OrderBy(u => u.CreatedAt);
                    break;
                case SortType.Ascending:
                    query = query.OrderBy(u => u.FullName);
                    break;
                case SortType.Descending:
                    query = query.OrderByDescending(u => u.FullName);
                    break;
                default:
                    query = query.OrderByDescending(u => u.CreatedAt);
                    break;
            }
            // Phân trang
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            var users = await query.ToListAsync();
            return users.Select(MapToDTO);
        }

        public override async Task<UserResponseDTO?> AddAsync(UserRequestDTO? dto)
        {
            if(dto != null && !string.IsNullOrEmpty(dto.PasswordNew))
            {
                dto.PasswordNew = AuthUtils.GenerateMd5Hash(dto.PasswordNew);
            }
            var result = await base.AddAsync(dto);
            if(result != null)
            {
                await _emailService.SendWelcomeEmailAsync(result.Email, result.FullName);
            }
            return result;
        }


        public override async Task<bool> UpdateAsync(UserRequestDTO? dto)
        {
            try
            {
                if (dto == null)
                    return false;
                var entity = await _userRepository.GetByIdAsync(dto.Id);
                if (entity == null)
                    return false;
                if (dto.ImageFile != null)
                {
                    entity.ImageUrl = await FileUpload.UploadFile(dto.ImageFile, FileUpload.UserFolder);
                }
                entity.UpdateFromRequestDto(dto);
                await _userRepository.UpdateAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật thông tin người dùng ID: {dto?.Id}");
                return false;
            }
        }

        protected override UserResponseDTO? MapToDTO(User? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        protected override User? MapToEntity(UserRequestDTO dto)
        {
            if (dto == null) return null;
            return dto.ToEntity();
        }

        public async Task<bool> SetUserActiveAsync(int userId, bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;
            user.IsActive = isActive;
            bool result = await _userRepository.UpdateAsync(user);
            // Gửi email
            await _emailService.SendUserActiveEmailAsync(user.Email, user.FullName, isActive);
            return result;
        }

        public override async Task<IEnumerable<UserResponseDTO?>> QueryAsync(string query)
        {
            query = query.Trim().ToLower();
            var users = await _userRepository.FindAsync(u =>
                u.FullName.ToLower().Contains(query) ||
                u.Email.ToLower().Contains(query) ||
                u.PhoneNumber.ToLower().Contains(query));
            return users.Select(MapToDTO);
        }
    }
}