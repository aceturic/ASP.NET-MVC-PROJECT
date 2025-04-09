using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApp.Models;
using System.Linq;
using System.Threading.Tasks;
using UsersApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace UsersApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PurchaseHistoryAdminController : Controller
    {
        private readonly AppDbContext _context;

        public PurchaseHistoryAdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseHistoryAdmin
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: PurchaseHistoryAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: PurchaseHistoryAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order updatedOrder)
        {
            if (id != updatedOrder.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(updatedOrder);

            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null) return NotFound();

                order.FirstName = updatedOrder.FirstName;
                order.LastName = updatedOrder.LastName;
                order.DeliveryAddress = updatedOrder.DeliveryAddress;
                order.City = updatedOrder.City;
                order.Country = updatedOrder.Country;
                order.ZipCode = updatedOrder.ZipCode;
                order.PhoneNumber = updatedOrder.PhoneNumber;

                _context.Update(order);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Order updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(updatedOrder.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PurchaseHistoryAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: PurchaseHistoryAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            _context.OrderItems.RemoveRange(order.Items);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        // POST: PurchaseHistoryAdmin/AddProductToOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductToOrder(int orderId, int newProductId, int quantity = 1) // Added quantity parameter, defaulting to 1
        {
            // Basic validation
            if (newProductId <= 0 || quantity <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Product ID or Quantity.";
                return RedirectToAction(nameof(Edit), new { id = orderId });
            }

            var order = await _context.Orders
                .Include(o => o.Items) // Include items to check for existing ones
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return NotFound();

            // Check if product exists
            var product = await _context.Products.FindAsync(newProductId);
            if (product == null)
            {
                TempData["ErrorMessage"] = $"Product with ID {newProductId} not found.";
                return RedirectToAction(nameof(Edit), new { id = orderId });
            }

            // Check if product is already in the order
            var existingItem = order.Items.FirstOrDefault(i => i.ProductId == newProductId);
            if (existingItem != null)
            {
                // Option 1: Update quantity if it already exists
                existingItem.Quantity += quantity;
                _context.Update(existingItem);
                TempData["SuccessMessage"] = $"Quantity updated for product {product.Name}.";

                // Option 2: Show an error if you don't want duplicates/updates here
                // TempData["ErrorMessage"] = $"Product {product.Name} is already in the order.";
                // return RedirectToAction(nameof(Edit), new { id = orderId });
            }
            else
            {
                // Create and add the new item
                var newItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = newProductId,
                    Quantity = quantity,
                    Price = product.Price // Get price from the product
                                          // You might need to fetch the Product from DB first to get its price
                };
                _context.OrderItems.Add(newItem); // Add directly to context or order.Items.Add(newItem);
                TempData["SuccessMessage"] = $"Product {product.Name} added to the order.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id = orderId });
        }


        // POST: PurchaseHistoryAdmin/RemoveProductFromOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProductFromOrder(int orderId, int productId)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.ProductId == productId);

            if (orderItem == null)
            {
                TempData["ErrorMessage"] = "Item not found in this order.";
                // Don't return NotFound() directly, redirect back to Edit page
                return RedirectToAction(nameof(Edit), new { id = orderId });
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Item removed successfully!";
            return RedirectToAction(nameof(Edit), new { id = orderId });
        }

        // Helper method to get Product Price (if needed above)
        // You might already have this logic elsewhere
        private async Task<decimal> GetProductPriceAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product?.Price ?? 0; // Handle case where product might not be found
        }
    }
}