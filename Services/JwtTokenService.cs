using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VNFarm.DTOs.Response;
using VNFarm.Services.Interfaces;

namespace VNFarm.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResultDTO GenerateToken(int userId, string email, string role)
        {
            var secretKey = _configuration["Jwt:Key"] ?? throw new Exception("Missing JWT Key");
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expires = DateTime.UtcNow.AddYears(2);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new TokenResultDTO
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expires
            };
        }
        
        public int? GetUserIdFromToken(ClaimsPrincipal claimsPrincipal)
        {
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            
            return null;
        }
        
        public int? GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new Exception("Missing JWT Key"));
                
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Giải mã token 🔑
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                
                // Lấy thông tin user id từ claims 👤
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
                {
                    return userId;
                }
                
                return null;
            }
            catch (Exception)
            {
                // Trả về null nếu token không hợp lệ hoặc có lỗi xảy ra 🚫
                return null;
            }
        }
        
        public string? GetRoleFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new Exception("Missing JWT Key"));
                
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Giải mã token 🔑
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                
                // Lấy thông tin role từ claims 👑
                var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;
                
                return roleClaim?.Trim();
            }
            catch (Exception)
            {
                // Trả về null nếu token không hợp lệ hoặc có lỗi xảy ra 🚫
                return null;
            }
        }
    }
}
