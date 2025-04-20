using Microsoft.AspNetCore.Mvc;
using VNFarm_FinalFinal.DTOs.Request;
using VNFarm_FinalFinal.Interfaces.Services;

namespace VNFarm_FinalFinal.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserService _userService;

        public AuthController(IJwtTokenService jwtTokenService, IUserService userService)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var user = await _userService.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized();
            }
            var token = _jwtTokenService.GenerateToken(user.Id, user.Email, "Admin");
            return Ok(token);
        }
    }
}

