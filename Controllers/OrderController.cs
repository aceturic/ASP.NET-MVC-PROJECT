using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApp.Data;
using UsersApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: Checkout
        public async Task<IActionResult> Checkout(int productId)
        {
            // ✅ Redirect to login if user is not authenticated
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Login", "Account");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            // ✅ Check stock before proceeding to checkout
            if (product.Quantity <= 0)
            {
                TempData["ErrorMessage"] = "This product is out of stock.";
                return RedirectToAction("Index", "Store");
            }

            // ✅ Get the current logged-in user's email
            var user = await _userManager.GetUserAsync(User);
            if (user?.Email == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if no email
            }

            var order = new Order
            {
                ProductId = product.Id,
                Product = product,
                Price = product.Price,
                UserEmail = user.Email // ✅ Automatically set user email
            };

            return View(order);
        }

        // POST: Confirm Order (Reduces Stock)
        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                order.Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == order.ProductId);
                return View("Checkout", order);
            }

            // Ensure the product exists before saving
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == order.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            // ✅ Prevent purchase if the stock is insufficient
            if (product.Quantity <= 0)
            {
                TempData["ErrorMessage"] = "This product is out of stock.";
                return RedirectToAction("Index", "Store");
            }

            // ✅ Deduct 1 from the product quantity
            product.Quantity -= 1;

            // ✅ Save the updated stock
            _context.Products.Update(product);

            // ✅ Check if the user is logged in before saving the order
            var user = await _userManager.GetUserAsync(User);
            if (user?.Email == null)
            {
                return RedirectToAction("Login", "Account");
            }

            order.Product = product;
            order.Price = product.Price;
            order.UserEmail = user.Email; // ✅ Store the user's email
            order.OrderDate = DateTime.Now;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your order has been placed successfully!";
            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
