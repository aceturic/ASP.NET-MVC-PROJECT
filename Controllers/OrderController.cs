using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using UsersApp.Data;
using UsersApp.Models;
using UsersApp.ViewModels;

namespace UsersApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Order/OrderDetails?productId=1
        [HttpGet]
        public IActionResult OrderDetails(int productId)
        {
            // Retrieve product details from the database
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Create and return the view model
            var model = new OrderViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price  // Assuming Price is already a decimal
            };

            return View(model);
        }

        // POST: /Order/OrderDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderDetails(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // If there are validation errors, redisplay the form
                return View(model);
            }

            // Generate a unique PurchaseID (using a GUID substring)
            string purchaseId = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();

            // Map the view model to an Order entity
            var order = new Order
            {
                PurchaseId = purchaseId,
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                Price = model.Price,
                DeliveryAddress = model.DeliveryAddress,
                City = model.City,
                ZipCode = model.ZipCode,
                OrderDate = DateTime.Now
            };

            // Save the order into the database
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Pass the PurchaseID to the confirmation page
            TempData["PurchaseId"] = purchaseId;
            return RedirectToAction("OrderConfirmation");
        }

        // GET: /Order/OrderConfirmation
        public IActionResult OrderConfirmation()
        {
            ViewBag.PurchaseId = TempData["PurchaseId"];
            return View();
        }
    }
}
