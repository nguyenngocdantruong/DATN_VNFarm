using Microsoft.AspNetCore.Mvc;
using VNFarm_FinalFinal.DTOs.Response;
using VNFarm_FinalFinal.Interfaces.Services;
using VNFarm_FinalFinal.ViewModels.Admin;
using VNFarm_FinalFinal.ViewModels.Common;

namespace VNFarm_FinalFinal.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;

        public AdminController(ILogger<AdminController> logger, IUserService userService, IProductService productService, IOrderService orderService, IStoreService storeService)
        {
            _logger = logger;
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
            _storeService = storeService;
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
            

            return View("Index", summaryAdminViewModel);
        }
        public IActionResult Shop()
        {
            ShopListViewModel shopListViewModel = new ShopListViewModel();
            return View();
        }
    }
}