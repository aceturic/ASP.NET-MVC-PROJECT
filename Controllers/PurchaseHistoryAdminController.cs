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
    }
}