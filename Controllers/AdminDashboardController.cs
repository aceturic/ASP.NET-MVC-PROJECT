using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }

        public IActionResult Products()
        {
            return RedirectToAction("Index", "Products");
        }

        public IActionResult RegisteredUsers()
        {
            return RedirectToAction("RegisteredUsers", "Admin");
        }

        public IActionResult PurchaseControl()
        {
            return RedirectToAction("Index", "PurchaseHistoryAdmin");
        }
    }
}
