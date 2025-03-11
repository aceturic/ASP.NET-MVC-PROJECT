using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UsersApp.Data;
using UsersApp.Models;
using UsersApp.ViewModels;

namespace UsersApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<Users> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> CancelRequests()
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .Where(o => o.Status == OrderStatus.PendingCancellation)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveCancellation(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = OrderStatus.Cancelled;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("CancelRequests");
        }

        [HttpPost]
        public async Task<IActionResult> RejectCancellation(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = OrderStatus.Rejected;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("CancelRequests");
        }

        public IActionResult RegisteredUsers()
        {
            var users = _userManager.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.FullName
            }).ToList();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.FullName = model.FullName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("RegisteredUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
