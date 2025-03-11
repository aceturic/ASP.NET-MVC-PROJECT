using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApp.Data;
using UsersApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace UsersApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public OrderController(AppDbContext context, UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Checkout(int productId)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            if (product.Quantity <= 0)
            {
                TempData["ErrorMessage"] = "This product is out of stock.";
                return RedirectToAction("Index", "Store");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user?.Email == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            var order = new Order
            {
                ProductId = product.Id,
                Product = product,
                Price = product.Price,
                UserEmail = user.Email 
            };

            return View(order);
        }

    
        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                order.Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == order.ProductId);
                return View("Checkout", order);
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == order.ProductId);
            if (product == null)
            {
                return NotFound();
            }

           
            if (product.Quantity <= 0)
            {
                TempData["ErrorMessage"] = "This product is out of stock.";
                return RedirectToAction("Index", "Store");
            }

           
            product.Quantity -= 1;

            
            _context.Products.Update(product);

            var user = await _userManager.GetUserAsync(User);
            if (user?.Email == null)
            {
                return RedirectToAction("Login", "Account");
            }

            order.Product = product;
            order.Price = product.Price;
            order.UserEmail = user.Email;
            order.OrderDate = DateTime.Now;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your order has been placed successfully!";
            return RedirectToAction("OrderSuccess");
        }

        public async Task<IActionResult> PurchaseHistory()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var orders = await _context.Orders
                .Include(o => o.Product) 
                .Where(o => o.UserEmail == userEmail)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> CancelRequest(int id)
        {
            var order = await _context.Orders.Include(o => o.Product)
                                             .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || order.UserEmail != User.FindFirstValue(ClaimTypes.Email))
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCancelRequest(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null || order.UserEmail != User.FindFirstValue(ClaimTypes.Email))
            {
                return NotFound();
            }

            // Mark order as "Pending Cancellation"
            order.Status = OrderStatus.PendingCancellation;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your cancellation request has been sent for approval.";
            return RedirectToAction("PurchaseHistory");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
