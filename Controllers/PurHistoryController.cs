using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using UsersApp.Services;
using UsersApp.Models;
using System.Collections.Generic;

namespace UsersApp.Controllers
{
    [Authorize]
    [Route("PurchaseHistory")] // ✅ Ensures route is correctly mapped
    public class PurHistoryController : Controller
    {
        private readonly OrderService _orderService;

        public PurHistoryController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")] // ✅ /PurchaseHistory or /PurchaseHistory/Index
        public async Task<IActionResult> Index()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var orders = await _orderService.GetUserOrdersAsync(userEmail);
            if (orders == null || orders.Count == 0)
            {
                ViewBag.Message = "You have no purchase history.";
            }

            return View(orders);
        }
    }
}
