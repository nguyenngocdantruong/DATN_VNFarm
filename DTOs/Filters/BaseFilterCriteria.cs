using System.ComponentModel.DataAnnotations;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.DTOs.Filters
{
    public abstract class BaseFilterCriteria : IFilterCriteria
    {
        private string searchTerm = string.Empty;
        public string SearchTerm { get => searchTerm; set => searchTerm = value ?? string.Empty; }
        [EnumDataType(typeof(SortType))]
        public SortType SortBy { get; set; } = SortType.Ascending;
        private int _page = 1;
        public int Page { get => _page; set => _page = Math.Clamp(value, 1, int.MaxValue); }
        private int _pageSize = 10;
        public int PageSize { get => Math.Min(_pageSize, MaxPageSize); set => _pageSize = Math.Clamp(value, 1, MaxPageSize); }
        private int MaxPageSize => 15;
    }
}
