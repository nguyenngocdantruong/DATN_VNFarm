using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Request;
using VNFarm.DTOs.Response;
using VNFarm.Entities;
using VNFarm.Interfaces.Services;
using VNFarm.Helpers;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ResourceController: ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;
        public ResourceController(ILogger<ResourceController> logger)
        {
            _logger = logger;
        }
        [HttpGet("get-image")]
        public IActionResult GetImage([FromQuery] string fileName, [FromQuery] string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", folderName);
            var filePath = Path.Combine(folderPath, fileName);

            if(FileUpload.IsAllowedExtension(fileName) == false)
                return BadRequest(new { success = false, message = "Định dạng file không hợp lệ" });
            _logger.LogWarning($"File path: {filePath}");
            if (!System.IO.File.Exists(filePath))
                return NotFound(new { success = false, message = "File không tồn tại" });

            var mimeType = "image/" + Path.GetExtension(fileName).Trim('.'); // image/png, image/jpeg,...
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, mimeType);
        }
    }
}