using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VNFarm_FinalFinal.DTOs.Request
{
    public abstract class BaseRequestDTO
    {
        [ReadOnly(true)]
        public int Id { get; set; }
    }
}