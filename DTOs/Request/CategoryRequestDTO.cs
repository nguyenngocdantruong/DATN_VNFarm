using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace VNFarm.DTOs.Request
{
    public class CategoryRequestDTO : BaseRequestDTO
    {
        
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự")]
        public string Name { get; set; } = "";
        
        [StringLength(255, ErrorMessage = "Mô tả danh mục không được vượt quá 255 ký tự")]
        public string Description { get; set; } = "";
        public string IconUrl { get; set; } = "";
        public IFormFile? IconFile { get; set; }
    }
}