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
    public class CategoryService : BaseService<Category, CategoryRequestDTO, CategoryResponseDTO>, ICategoryService
    {
        #region Fields & Constructor
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository repository) : base(repository)
        {
            _categoryRepository = repository;
        }
        #endregion

        #region Base Service Implementation
        protected override CategoryResponseDTO? MapToDTO(Category? entity)
        {
            return entity?.ToResponseDTO();
        }

        protected override Category MapToEntity(CategoryRequestDTO dto)
        {
            return dto.ToEntity();
        }

        public override async Task<bool> UpdateAsync(CategoryRequestDTO dto)
        {
            var entity = await _categoryRepository.GetByIdAsync(dto.Id);
            if (entity == null)
                return false;

            if (dto.IconFile != null)
            {
                var fileUrl = await FileUpload.UploadFile(dto.IconFile, FileUpload.CategoryFolder);
                entity.IconUrl = fileUrl;
            }
            
            entity.UpdateFromRequestDto(dto);
            return await _categoryRepository.UpdateAsync(entity);
        }

        public override async Task<IEnumerable<CategoryResponseDTO?>> QueryAsync(string query)
        {
            var entities = await _categoryRepository.FindAsync(
                c => c.Name.Contains(query) || c.Description.Contains(query)
            );
            return entities.Select(MapToDTO);
        }
        #endregion

        #region Query Methods
        public override async Task<IQueryable<Category>> Query(IFilterCriteria filter)
        {
            var query = await _categoryRepository.GetQueryableAsync();
            if(filter is CategoryCriteriaFilter categoryCriteriaFilter)
            {
                // Apply search filter
                if (!string.IsNullOrEmpty(categoryCriteriaFilter.SearchTerm))
                {
                    query = query.Where(c => c.Name.Contains(categoryCriteriaFilter.SearchTerm));
                }

                // Apply price filters
                if (categoryCriteriaFilter?.MinPrice > 0)
                {
                    query = query.Where(c => c.MinPrice >= categoryCriteriaFilter.MinPrice);
                }

                if (categoryCriteriaFilter?.MaxPrice != decimal.MaxValue)
                {
                    query = query.Where(c => c.MaxPrice <= categoryCriteriaFilter.MaxPrice && c.MaxPrice >= categoryCriteriaFilter.MinPrice);
                }

                
            }
            else{
                throw new ArgumentException("Filter truyền vào không phải là CategoryCriteriaFilter");
            }
            return query;
        }

        public override async Task<IEnumerable<CategoryResponseDTO?>> ApplyPagingAndSortingAsync(IQueryable<Category> query, IFilterCriteria filter)
        {
            if(filter is CategoryCriteriaFilter categoryCriteriaFilter)
            {
                // Apply sorting
                switch(categoryCriteriaFilter.SortBy)
                {
                    case SortType.Ascending:
                        query = query.OrderBy(c => c.Name);
                        break;
                    case SortType.Descending:
                        query = query.OrderByDescending(c => c.Name);
                        break;
                    case SortType.Latest:
                        query = query.OrderByDescending(c => c.CreatedAt);
                        break;
                    case SortType.Oldest:
                        query = query.OrderBy(c => c.CreatedAt);
                        break;
                }
            }
            else{
                throw new ArgumentException("Filter truyền vào không phải là CategoryCriteriaFilter");
            }
            // Apply paging
            query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
            return (await query.ToListAsync()).Select(e => e.ToResponseDTO());
        }
        
        #endregion
    }
} 