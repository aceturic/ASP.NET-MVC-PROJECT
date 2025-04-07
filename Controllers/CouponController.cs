using Microsoft.AspNetCore.Mvc;
using UsersApp.Models;
using UsersApp.Services;

namespace UsersApp.Controllers
{
    public class CouponController : Controller
    {
        private readonly CouponService _couponService;

        public CouponController(CouponService couponService)
        {
            _couponService = couponService;
        }

        // View all existing coupons
        public async Task<IActionResult> Index()
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            return View(coupons);
        }

        // GET: Create coupon form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Handle coupon creation
        [HttpPost]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                await _couponService.CreateCouponAsync(coupon);
                return RedirectToAction("Index");
            }

            return View(coupon);
        }
    }
}
