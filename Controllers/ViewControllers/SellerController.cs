using Microsoft.AspNetCore.Mvc;

namespace VNFarm.Controllers
{
    public class SellerController : BaseController
    {
        public IActionResult Index()
        {
            if (!IsSeller)
                return RedirectToAction("Login", "Home");
            return View();
        }
    }
}

