using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApp.Data;
using UsersApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using UsersApp.Extensions;
using UsersApp.ViewModels;

namespace UsersApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;
        private const string CartSessionKey = "Cart";

        public OrderController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey);
            if (cart == null || !cart.Any())
            {
                TempData["ErrorMessage"] = "Количката е празна.";
                return RedirectToAction("Index", "Cart");
            }

            var viewModel = new CartViewModel
            {
                Items = cart.Select(item => new CartItemViewModel
                {
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    ImageFileName = item.Product.ImageFileName
                }).ToList()
            };

            return View("Checkout", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(CartViewModel model)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey);
            if (cart == null || !cart.Any())
            {
                TempData["ErrorMessage"] = "Количката е празна.";
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                model.Items = cart.Select(item => new CartItemViewModel
                {
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    ImageFileName = item.Product.ImageFileName
                }).ToList();
                return View("Checkout", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var order = new Order
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DeliveryAddress = model.DeliveryAddress,
                City = model.City,
                Country = model.Country,
                ZipCode = model.ZipCode,
                PhoneNumber = model.PhoneNumber,
                UserEmail = user.Email,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Processing
            };

            foreach (var item in cart)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.Product.Id);
                if (product == null || product.Quantity < item.Quantity)
                {
                    TempData["ErrorMessage"] = $"Продуктът {item.Product.Name} не е наличен в достатъчно количество.";
                    return RedirectToAction("Index", "Cart");
                }

                product.Quantity -= item.Quantity;
                _context.Products.Update(product);

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = product.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartSessionKey);
            TempData["SuccessMessage"] = "Поръчката е успешно направена!";
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
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Where(o => o.UserEmail == userEmail)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> CancelRequest(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
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
