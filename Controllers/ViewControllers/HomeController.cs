using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Response;
using System.Security.Claims;
using VNFarm.DTOs.Request;
using VNFarm.Enums;
using VNFarm.Interfaces.Repositories;
using VNFarm.Interfaces.Services;

namespace VNFarm.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserRepository _userRepository;

        public HomeController(IJwtTokenService jwtTokenService, IUserRepository userRepository)
        {
            _jwtTokenService = jwtTokenService;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            if (IsLogin)
            {
                if (IsAdmin)
                    return RedirectToAction("Dashboard", "Admin");
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        public IActionResult Login()
        {
            if (IsLogin)
            {
                if (IsAdmin)
                    return RedirectToAction("Dashboard", "Admin");
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin";
                return View();
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng";
                return View();
            }

            // TODO: Kiểm tra password hash
            if (request.Password != "123")
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng";
                return View();
            }

            var tokenResult = _jwtTokenService.GenerateToken(user.Id, user.Email, user.Role.ToString());
            
            // Lưu token vào cookie
            Response.Cookies.Append("jwt", tokenResult.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = tokenResult.Expiration
            });

            if (user.Role == UserRole.Admin)
                return RedirectToAction("Dashboard", "Admin");
            return RedirectToAction("Index", "User");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Index");
        }
    }
} 