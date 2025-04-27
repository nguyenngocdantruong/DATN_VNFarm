using Microsoft.AspNetCore.Mvc;

namespace VNFarm.Controllers
{
    public class UserController : BaseController
    {
        public UserController()
        {
        }

        public IActionResult Index()
        {
            if (!IsLogin)
                return RedirectToAction("Login", "Home");
            return View();
        }

        public IActionResult Profile()
        {
            if (!IsLogin)
                return RedirectToAction("Login", "Home");
            return View();
        }
    }
} 