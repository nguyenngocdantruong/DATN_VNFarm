using Microsoft.AspNetCore.Mvc;
using VNFarm.Helpers;

namespace VNFarm.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnumController : ControllerBase
    {
        [HttpGet("business-register-statuses")]
        public IActionResult GetBusinessRegister()
        {
            var businessRegister = BusinessRegisterUtils.GetEnumValues();
            return Ok(businessRegister);
        }
        [HttpGet("discount-types")]
        public IActionResult GetDiscountTypes()
        {
            var discount = DiscountUtils.GetDiscountsTypeForForm();
            return Ok(discount);
        }
        [HttpGet("discount-statuses")]
        public IActionResult GetDiscountStatuses()
        {
            var discountStatus = DiscountUtils.GetDiscountStatusForForm();
            return Ok(discountStatus);
        }
        [HttpGet("order-statuses")]
        public IActionResult GetOrderStatuses()
        {
            var orderStatus = OrderUtils.GetOrderStatusName();
            return Ok(orderStatus);
        }
        [HttpGet("order-event-types")]
        public IActionResult GetOrderEventTypes()
        {
            var orderEventType = OrderUtils.GetOrderEventTypeForForm();
            return Ok(orderEventType);
        }
        [HttpGet("order-timeline-statuses")]
        public IActionResult GetOrderTimelineStatuses()
        {
            var orderTimelineStatus = OrderUtils.GetOrderTimeLineStatus();
            return Ok(orderTimelineStatus);
        }
        
        [HttpGet("sort-entities")]
        public IActionResult GetSortEntities()
        {
            var sortEntities = SortUtils.GetSortEntities();
            return Ok(sortEntities);
        }
        [HttpGet("sort-by-entity")]
        public IActionResult GetSortTypes([FromQuery] string entityName)
        {
            var sortType = SortUtils.GetAvailableSortTypes(entityName);
            return Ok(sortType);
        }
        [HttpGet("unit-types")]
        public IActionResult GetUnitTypes()
        {
            var unitTypes = UnitUtils.GetUnitsForForm();
            return Ok(unitTypes);
        }
        [HttpGet("origin-types")]
        public IActionResult GetOriginTypes()
        {
            var originTypes = new List<string>
            {
                "An Giang",
                "Bà Rịa - Vũng Tàu",
                "Bắc Giang",
                "Bắc Kạn",
                "Bạc Liêu",
                "Bắc Ninh",
                "Bến Tre",
                "Bình Định",
                "Bình Dương",
                "Bình Phước",
                "Bình Thuận",
                "Cà Mau",
                "Cần Thơ",
                "Cao Bằng",
                "Đà Nẵng",
                "Đắk Lắk",
                "Đắk Nông",
                "Điện Biên",
                "Đồng Nai",
                "Đồng Tháp",
                "Gia Lai",
                "Hà Giang",
                "Hà Nam",
                "Hà Nội",
                "Hà Tĩnh",
                "Hải Dương",
                "Hải Phòng",
                "Hậu Giang",
                "Hòa Bình",
                "Hưng Yên",
                "Khánh Hòa",
                "Kiên Giang",
                "Kon Tum",
                "Lai Châu",
                "Lâm Đồng",
                "Lạng Sơn",
                "Lào Cai",
                "Long An",
                "Nam Định",
                "Nghệ An",
                "Ninh Bình",
                "Ninh Thuận",
                "Phú Thọ",
                "Phú Yên",
                "Quảng Bình",
                "Quảng Nam",
                "Quảng Ngãi",
                "Quảng Ninh",
                "Quảng Trị",
                "Sóc Trăng",
                "Sơn La",
                "Tây Ninh",
                "Thái Bình",
                "Thái Nguyên",
                "Thanh Hóa",
                "Thừa Thiên Huế",
                "Tiền Giang",
                "TP. Hồ Chí Minh",
                "Trà Vinh",
                "Tuyên Quang",
                "Vĩnh Long",
                "Vĩnh Phúc",
                "Yên Bái"
            };
            return Ok(originTypes);
        }
        [HttpGet("payment-methods")]
        public IActionResult GetPaymentMethods()
        {
            var paymentMethods = PaymentUtils.GetPaymentMethodsForForm();
            return Ok(new {
                success = true,
                data = paymentMethods
            });
        }
        [HttpGet("payment-statuses")]
        public IActionResult GetPaymentStatuses()
        {
            var paymentStatuses = PaymentUtils.GetPaymentStatusesForForm();
            return Ok(new {
                success = true,
                data = paymentStatuses
            });
        }
        [HttpGet("store-statuses")]
        public IActionResult GetStoreStatuses()
        {
            var storeStatuses = StoreUtils.GetStoreStatuses();
            return Ok(storeStatuses);
        }
        [HttpGet("store-types")]
        public IActionResult GetStoreTypes()
        {
            var storeTypes = StoreUtils.GetStoreTypes();
            return Ok(storeTypes);
        }
    }
}