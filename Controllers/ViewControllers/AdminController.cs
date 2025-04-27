using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VNFarm.DTOs.Response;
using VNFarm.ViewModels.Common;
using VNFarm.Interfaces.Services;
using VNFarm.ViewModels.Admin;

namespace VNFarm.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;
        private readonly IBusinessRegistrationService _businessRegistrationService;

        public AdminController(ILogger<AdminController> logger, IUserService userService, IProductService productService, IOrderService orderService, IStoreService storeService, IBusinessRegistrationService businessRegistrationService)
        {
            _logger = logger;
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
            _storeService = storeService;
            _businessRegistrationService = businessRegistrationService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard", "Admin");
        }

        public async Task<IActionResult> Dashboard()
        {
            var productPageSize = 10;
            var orderPageSize = 10;

            var totalOrders = await _orderService.CountAsync();
            var totalUsers = await _userService.CountAsync();
            var totalProducts = await _productService.CountAsync();
            var totalRevenue = await _orderService.GetTotalRevenueByDateRangeAsync(DateTime.Now.AddYears(-5), DateTime.Now);

            var topProductsSold = await _productService.GetTopSellingProductsAsync(1, 10);
            var recentOrders = (await _orderService.GetOrdersByDateRangeAsync(DateTime.Now.AddDays(-30), DateTime.Now)).Take(10);

            AdminSummaryViewModel summaryAdminViewModel = new AdminSummaryViewModel(){
                TotalOrders = totalOrders,
                TotalUsers = totalUsers,
                TotalProducts = totalProducts,
                TotalRevenue = totalRevenue,
                ProductDTOs = topProductsSold.ToList(),
                OrderDTOs = recentOrders.ToList(),
                TotalProductPages = totalProducts / 10,
                ProductPageSize = productPageSize,
                TotalOrderPages = totalOrders / 10,
                OrderPageSize = orderPageSize
            };
            

            return View(summaryAdminViewModel);
        }
        public IActionResult Shop()
        {
            return View();
        }
        public async Task<IActionResult> ShopDetail(int id)
        {
            var shop = await _storeService.GetByIdAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            return View();
        }
        public IActionResult ShopRequest()
        {
            return View("RegisterShopList");
        }
        public async Task<IActionResult> ShopRequestDetail(int id)
        {
            var businessRegistration = await _businessRegistrationService.GetByIdAsync(id);
            if (businessRegistration == null)
            {
                return NotFound();
            }
            return View("RegisterShopDetail");
        }
        public IActionResult Product()
        {
            return View("ProductList");
        }
        
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View("ProductDetail");
        }
        public IActionResult ProductAdd()
        {
            return View("ProductAdd");
        }
        public async Task<IActionResult> ProductEdit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View("ProductEdit");
        }
        public IActionResult Order()
        {
            return View("OrderList");
        }
        public async Task<IActionResult> OrderDetail(string orderCode)
        {
            var order = await _orderService.FindAsync(o => o.OrderCode == orderCode || orderCode == o.Id.ToString());
            if (order == null)
            {
                return NotFound();
            }
            return View("OrderDetail");
        }
        public IActionResult Users()
        {
            return View("UserList");
        }
        public async Task<IActionResult> UserDetail(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View("UserDetail");
        }
        public IActionResult UserActivate()
        {
            return View("ActivationUser");
        }
        public IActionResult Dispute()
        {
            return View("ChatRoomList");
        }
        public async Task<IActionResult> ProductReview(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View("ProductReviewList");
        }
        public IActionResult Settings()
        {
            return View("Settings");
        }
    }
}