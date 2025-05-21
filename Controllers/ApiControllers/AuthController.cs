using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Request;
using VNFarm.Enums;
using VNFarm.Entities;
using VNFarm.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using VNFarm.Caching;
using VNFarm.Services.Interfaces;
using VNFarm.Services.External.Interfaces;

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
        private readonly IEmailService _emailService;
        private readonly MyOtpService _otpService;

        public AuthController(IJwtTokenService jwtTokenService, IUserService userService, IStoreService storeService, ILogger<AuthController> logger, IEmailService emailService, MyOtpService otpService)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _storeService = storeService;
            _logger = logger;
            _emailService = emailService;
            _otpService = otpService;
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
            if(!user.IsActive)
            {
                return Ok(new
                {
                    status = 401,
                    message = "Tài khoản của bạn không hoạt động lúc này. Vui lòng liên hệ quản trị viên để được hỗ trợ."
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
                case UserRole.Buyer:
                    role = "Buyer";
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
        public async Task<IActionResult> Register([FromForm] RegisterRequestDTO request)
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
                    IsActive = request.UserRole == UserRole.Buyer, // Buyer được kích hoạt ngay, Seller đợi duyệt
                    Role = request.UserRole, // Đặt role theo yêu cầu đăng ký
                    ImageUrl = "default.jpg"
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
                        message = request.UserRole == UserRole.Buyer
                            ? "Đăng ký thành công"
                            : "Đăng ký thành công. Vui lòng đợi quản trị viên duyệt tài khoản.",
                        token = token,
                        user = new
                        {
                            id = result.Id,
                            fullName = result.FullName,
                            email = result.Email,
                            role = request.UserRole,
                            isActive = request.UserRole == UserRole.Buyer
                        }
                    });
                }
                else
                {
                    if (request.UserRole == UserRole.Seller)
                    {
                        var logoUrl = "default.jpg";
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
                            UserId = result.Id,
                            LogoFile = request.LogoFile,
                            IsActive = true
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
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO request)
        {
            var user = (await _userService.FindAsync(x => x.Email == request.Email)).FirstOrDefault();
            if(user == null)
                return NotFound(new
                {
                    status = 404,
                    message = "Email không tồn tại"
                });
            
            var otp = new Random().Next(100000, 999999);
            _otpService.SetOtp(user.Email, otp);
            await _emailService.SendEmailAsync(
                user.Email, 
                "Đặt lại mật khẩu", 
                $"Chúng tôi nhận được yêu cầu đặt lại mật khẩu của bạn trên sàn TMĐT VnFarm. <br>Mã OTP để đặt lại mật khẩu của bạn là: {otp}. Mã có hiệu lực trong 15 phút. Vui lòng không chia sẻ với bất kì ai.", true);
            return Ok(new
            {
                status = 200,
                message = "Mã OTP đặt lại mật khẩu đã được gửi đến email của bạn !"
            });
        }
        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            var user = (await _userService.FindAsync(x => x.Email == request.Email)).FirstOrDefault();
            
            if(user == null)
                return BadRequest(new
                {
                    status = 400,
                    message = "Không tồn tại email này"
                });

            // Kiểm tra token
            int otpExpected = _otpService.GetOtp(user.Email);
            if (otpExpected == -1)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "OTP không hợp lệ hoặc quá hạn"
                });
            }
            else if (otpExpected != request.OTP)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "Mã OTP không chính xác"
                });
            }
            else
            {
                var userRequestDTO = new UserRequestDTO
                {
                    Id = user.Id,
                    PasswordNew = request.NewPassword,
                    Email = request.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                };

                await _userService.UpdateAsync(userRequestDTO);

                return Ok(new
                {
                    status = 200,
                    message = "Đặt lại mật khẩu thành công!"
                });
            }
        }
    }
}

