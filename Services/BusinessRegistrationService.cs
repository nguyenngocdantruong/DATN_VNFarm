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
    public class BusinessRegistrationService : BaseService<BusinessRegistration, BusinessRegistrationRequestDTO, BusinessRegistrationResponseDTO>, IBusinessRegistrationService
    {
        #region Fields & Constructor
        private readonly IBusinessRegistrationRepository _businessRegistrationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ILogger<BusinessRegistrationService> _logger;

        public BusinessRegistrationService(
            IBusinessRegistrationRepository businessRegistrationRepository,
            IUserRepository userRepository,
            IStoreRepository storeRepository,
            ILogger<BusinessRegistrationService> logger) : base(businessRegistrationRepository)
        {
            _businessRegistrationRepository = businessRegistrationRepository;
            _userRepository = userRepository;
            _storeRepository = storeRepository;
            _logger = logger;
        }
        #endregion

        #region Base Service Implementation
        protected override BusinessRegistration? MapToEntity(BusinessRegistrationRequestDTO dto)
        {
            return dto.ToEntity();
        }

        protected override BusinessRegistrationResponseDTO? MapToDTO(BusinessRegistration? entity)
        {
            if (entity == null) return null;
            return entity.ToResponseDTO();
        }

        private IEnumerable<BusinessRegistrationResponseDTO?> MapToDTO(IEnumerable<BusinessRegistration> entities)
        {
            if (entities == null) return new List<BusinessRegistrationResponseDTO>();
            return entities.Select(e => MapToDTO(e));
        }

        public override async Task<BusinessRegistrationResponseDTO?> AddAsync(BusinessRegistrationRequestDTO? dto)
        {
            if (dto == null) return null;
            if (dto.BusinessLicenseFile != null)
            {
                var fileUrl = await FileUpload.UploadFile(dto.BusinessLicenseFile, FileUpload.BusinessLicenseFolder);
                dto.BusinessLicenseUrl = fileUrl;
            }
            return await base.AddAsync(dto);
        }

        public override async Task<bool> UpdateAsync(BusinessRegistrationRequestDTO dto)
        {
            var entity = await _businessRegistrationRepository.GetByIdAsync(dto.Id);
            if (entity == null) return false;
            if (dto.BusinessLicenseFile != null)
            {
                var fileUrl = await FileUpload.UploadFile(dto.BusinessLicenseFile, FileUpload.BusinessLicenseFolder);
                entity.BusinessLicenseUrl = fileUrl;
            }
            entity.UpdateFromRequestDto(dto);
            return await _businessRegistrationRepository.UpdateAsync(entity);
        }

        public override async Task<IEnumerable<BusinessRegistrationResponseDTO?>> QueryAsync(string query)
        {
            var entities = await _businessRegistrationRepository
                .FindAsync(e => e.BusinessName.Contains(query) || e.TaxCode.Contains(query));
            return MapToDTO(entities);
        }
        #endregion

        #region Query Methods
        public async Task<BusinessRegistrationResponseDTO?> GetByUserIdAsync(int userId)
        {
            var entity = await _businessRegistrationRepository.GetByUserIdAsync(userId);
            return MapToDTO(entity);
        }
        public async override Task<IQueryable<BusinessRegistration>> Query(IFilterCriteria filter)
        {
            // Lấy tất cả đăng ký
            var query = await _businessRegistrationRepository.GetQueryableAsync();
            if(filter is BusinessRegistrationCriteriaFilter businessRegistrationCriteriaFilter)
            {
                // Áp dụng bộ lọc
                if (!string.IsNullOrEmpty(businessRegistrationCriteriaFilter.SearchTerm))
                {
                    query = query.Where(r => 
                        r.BusinessName.Contains(businessRegistrationCriteriaFilter.SearchTerm, StringComparison.OrdinalIgnoreCase) || 
                        r.TaxCode.Contains(businessRegistrationCriteriaFilter.SearchTerm, StringComparison.OrdinalIgnoreCase));
                }

                if (businessRegistrationCriteriaFilter.RegistrationStatus != RegistrationStatus.All)
                {
                    query = query.Where(r => r.RegistrationStatus == businessRegistrationCriteriaFilter.RegistrationStatus);
                }

                if(businessRegistrationCriteriaFilter.BusinessType != StoreType.All)
                {
                    query = query.Where(r => r.BusinessType == businessRegistrationCriteriaFilter.BusinessType);
                }
                
            }
            return query;
        }

        public async override Task<IEnumerable<BusinessRegistrationResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<BusinessRegistration> query, IFilterCriteria filter)
        {
            if(filter is BusinessRegistrationCriteriaFilter businessRegistrationCriteriaFilter)
            {
                switch(businessRegistrationCriteriaFilter.SortBy)
                {
                    case SortType.Latest:
                        query = query.OrderByDescending(r => r.UpdatedAt);
                        break;
                    case SortType.Oldest:
                        query = query.OrderBy(r => r.UpdatedAt);
                        break;
                }
            }
            else {
                throw new ArgumentException("Filter truyền vào không phải là BusinessRegistrationCriteriaFilter");
            }
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            return (await query.ToListAsync()).Select(e => e.ToResponseDTO());
        }
        #endregion

        #region Business Registration Process
        public async Task<bool> VerifyRegistrationAsync(int registrationId, RegistrationStatus status, string notes)
        {
            try
            {
                return await _businessRegistrationRepository.VerifyRegistrationAsync(registrationId, status, notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xác thực đăng ký kinh doanh ID: {registrationId}");
                return false;
            }
        }

        public async Task<bool> ApproveRegistrationAsync(int registrationId, int adminId)
        {
            try
            {
                var result = await VerifyRegistrationAsync(registrationId, RegistrationStatus.Approved, "Đã duyệt");
                var registration = await _businessRegistrationRepository.GetByIdAsync(registrationId);
                if (result && registration != null)
                {
                    
                    // Cập nhật vai trò người dùng
                    var user = await _userRepository.GetByIdAsync(registration.UserId);
                    if (user != null)
                    {
                        user.Role = UserRole.Seller;
                        await _userRepository.UpdateAsync(user);
                    }
                    
                    // Tạo cửa hàng mới
                    var store = new Store
                    {
                        UserId = registration.UserId,
                        Name = registration.BusinessName,
                        LogoUrl = "",
                        Description = "",
                        Address = registration.Address,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        BusinessType = registration.BusinessType,
                        IsActive = true,
                        VerificationStatus = StoreStatus.Verified,
                        AverageRating = 5,
                        ReviewCount = 0
                    };
                    
                    await _storeRepository.AddAsync(store);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi phê duyệt đăng ký kinh doanh ID: {registrationId}");
                return false;
            }
        }

        public async Task<bool> RejectRegistrationAsync(int registrationId, int adminId, string rejectionReason)
        {
            try
            {
                return await VerifyRegistrationAsync(registrationId, RegistrationStatus.Rejected, rejectionReason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi từ chối đăng ký kinh doanh ID: {registrationId}");
                return false;
            }
        }
        #endregion

        #region Registration Approval Results
        public async Task<IEnumerable<RegistrationApprovalResultResponseDTO>> GetRegistrationApprovalResultsAsync(int registrationId)
        {
            try
            {
                var results = await _businessRegistrationRepository.GetRegistrationApprovalResultsAsync(registrationId);
                return results.Select(r => r.ToResponseDTO());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy kết quả phê duyệt đăng ký ID: {registrationId}");
                return new List<RegistrationApprovalResultResponseDTO>();
            }
        }

        public async Task<RegistrationApprovalResultResponseDTO?> AddRegistrationApprovalResultAsync(RegistrationApprovalResultRequestDTO resultDto, int adminId)
        {
            try
            {
                var approvalResult = resultDto.ToEntity(adminId);
                var result = await _businessRegistrationRepository.AddRegistrationApprovalResultAsync(approvalResult);
                return result?.ToResponseDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm kết quả phê duyệt đăng ký");
                return null;
            }
        }

        

        #endregion
    }
} 