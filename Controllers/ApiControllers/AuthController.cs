using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Request;
using VNFarm.Enums;
using VNFarm.Interfaces.Services;
using VNFarm.Entities;
using VNFarm.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserService _userService;
        private readonly IStoreService _storeService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IJwtTokenService jwtTokenService, IUserService userService, IStoreService storeService, ILogger<AuthController> logger)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _storeService = storeService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var password =  AuthUtils.GenerateMd5Hash(request.Password);
            _logger.LogInformation("Password: {password}", password);
            var user = (await _userService.FindAsync(x => x.Email == request.Email && x.PasswordHash == password)).FirstOrDefault();
            if (user == null)
            {
                return Ok(new
                {
                    status = 401,
                    message = "Thông tin đăng nhập không chính xác"
                });
            }
            string role = "User";
            switch (user.Role)
            {
                case UserRole.Admin:
                    role = "Admin";
                    break;
                case UserRole.User:
                    role = "User";
                    break;
                case UserRole.Seller:
                    role = "Seller";
                    break;
            }
            var token = _jwtTokenService.GenerateToken(user.Id, user.Email, role);
            var store = await _storeService.FindAsync(x => x.UserId == user.Id);
            return Ok(new
            {
                message = "Login successful",
                status = 200,
                token = token,
                user = user,
                store = store.FirstOrDefault(),
                role = role
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            try
            {
                // Kiểm tra email đã tồn tại chưa
                var isEmailUnique = await _userService.IsEmailUniqueAsync(request.Email);
                if (!isEmailUnique || request.UserRole == UserRole.Admin)
                {
                    return BadRequest(new
                    {
                        status = 400,
                        message = "Email đã được sử dụng"
                    });
                }

                // Tạo user request DTO
                var userDto = new UserRequestDTO
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordNew = request.Password, // Sẽ được hash trong service
                    IsActive = request.UserRole == UserRole.User, // User được kích hoạt ngay, Seller đợi duyệt
                    Role = request.UserRole // Đặt role theo yêu cầu đăng ký
                };

                // Thêm user vào database
                var result = await _userService.AddAsync(userDto);
                if (result == null)
                {
                    return StatusCode(500, new
                    {
                        status = 500,
                        message = "Đăng ký thất bại. Vui lòng thử lại sau."
                    });
                }

                if (result.IsActive)
                {
                    // Tạo token JWT
                    var token = _jwtTokenService.GenerateToken(result.Id, result.Email, request.UserRole.ToString());

                    return Ok(new
                    {
                        status = 200,
                        message = request.UserRole == UserRole.User
                            ? "Đăng ký thành công"
                            : "Đăng ký thành công. Vui lòng đợi quản trị viên duyệt tài khoản.",
                        token = token,
                        user = new
                        {
                            id = result.Id,
                            fullName = result.FullName,
                            email = result.Email,
                            role = request.UserRole,
                            isActive = request.UserRole == UserRole.User
                        }
                    });
                }
                else
                {
                    if (request.UserRole == UserRole.Seller)
                    {
                        var logoUrl = "no-image.webp";
                        if(request.LogoFile != null)
                        {
                            logoUrl = await FileUpload.UploadFile(request.LogoFile, FileUpload.StoreFolder);
                        }

                        // Thêm cửa hàng vào database
                        await _storeService.AddAsync(new StoreRequestDTO
                        {
                            Name = request.StoreName ?? "",
                            Description = request.StoreDescription ?? "",
                            LogoUrl = logoUrl,
                            Address = request.StoreAddress ?? "",
                            PhoneNumber = request.StorePhoneNumber ?? "",
                            Email = request.StoreEmail ?? "",
                            BusinessType = request.BusinessType,
                            VerificationStatus = StoreStatus.Pending,
                            UserId = result.Id
                        });
                    }
                    return Ok(new
                    {
                        status = 200,
                        message = "Đăng ký thành công. Vui lòng đợi quản trị viên duyệt tài khoản.",
                        user = userDto
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đăng ký tài khoản");
                return StatusCode(500, new
                {
                    status = 500,
                    message = "Đã có lỗi xảy ra khi đăng ký tài khoản"
                });
            }
        }
    }
}

