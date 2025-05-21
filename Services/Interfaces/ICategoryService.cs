using VNFarm.DTOs.Filters;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;

namespace VNFarm.Services.Interfaces
{
    public interface ICategoryService : IService<Category, CategoryRequestDTO, CategoryResponseDTO>
    {
        
    }
} 