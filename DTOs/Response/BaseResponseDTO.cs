using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VNFarm.DTOs.Response
{
    public abstract class BaseResponseDTO
    {
        [ReadOnly(true)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}