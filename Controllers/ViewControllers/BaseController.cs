using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNFarm_FinalFinal.Enums;

namespace VNFarm_FinalFinal.Controllers
{
    public class BaseController : Controller
    {
        protected int? UserId
        {
            get
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                              User.FindFirst("userId")?.Value;

                return int.TryParse(idClaim, out var id) ? id : null;
            }
        }

        protected UserRole? Role
        {
            get
            {
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value ??
                                User.FindFirst("role")?.Value;

                return Enum.TryParse<UserRole>(roleClaim, out var role) ? role : null;
            }
        }

        protected bool IsLogin => UserId.HasValue;
        protected bool IsAdmin => Role == UserRole.Admin;
        protected bool IsSeller => Role == UserRole.Seller;
        protected bool IsBuyer => Role == UserRole.Buyer;
    }
}
