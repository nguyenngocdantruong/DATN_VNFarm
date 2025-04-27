using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Response;

namespace VNFarm.Controllers.ViewControllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("Error", new ResultView { Code = 404, Title = "Không tìm thấy", Message = "Trang bạn đang tìm kiếm không tồn tại" });
                case 401:
                    return View("Error", new ResultView { Code = 401, Title = "Không được phép", Message = "Bạn không có quyền truy cập vào trang này" });
                case 400:
                    return View("Error", new ResultView { Code = 400, Title = "Lỗi yêu cầu", Message = "Yêu cầu không hợp lệ" });
                default:
                    return View("Error", new ResultView { Code = 500, Title = "Lỗi server", Message = "Đã xảy ra lỗi trong quá trình xử lý" });
            }
        }
    }
}
