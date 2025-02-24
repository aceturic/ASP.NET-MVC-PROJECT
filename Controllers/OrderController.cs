using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using UsersApp.Data;
using UsersApp.ViewModels;

public class OrderController : Controller
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult OrderDetails(int productId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Register", "Account");
        }

        var product = _context.Products.Find(productId);
        if (product == null)
        {
            return NotFound();
        }

        var model = new OrderDetailsViewModel
        {
            Email = User.Identity.Name,
            ProductId = product.Id,
            ProductName = product.Name
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult OrderDetails(OrderDetailsViewModel model)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Register", "Account");
        }

        if (!ModelState.IsValid)
        {           
            return View(model);
        }

        var product = _context.Products.Find(model.ProductId);
        if (product == null)
        {
            ModelState.AddModelError("", "Product not found.");
            return View(model);
        }

        string purchaseId = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();

        var purchaseOrder = new PurchaseOrder
        {
            PurchaseId = purchaseId,
            Email = model.Email,
            DeliveryAddress = model.DeliveryAddress,
            City = model.City,
            ZipCode = model.ZipCode,
            OrderDate = DateTime.Now,
            ProductId = product.Id
        };

        _context.PurchaseOrders.Add(purchaseOrder);
        _context.SaveChanges();

        return RedirectToAction("OrderConfirmation", new { id = purchaseOrder.Id });
    }

    public IActionResult OrderConfirmation(int id)
    {
        var order = _context.PurchaseOrders
            .Include(o => o.Product)
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        var model = new OrderConfirmationViewModel
        {
            PurchaseId = order.PurchaseId,
            Email = order.Email,
            ProductName = order.Product?.Name,
            DeliveryAddress = order.DeliveryAddress,
            City = order.City,
            ZipCode = order.ZipCode
        };

        return View(model);
    }
    [HttpPost]
    public IActionResult PlaceOrder(OrderDetailsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("OrderDetails", model);
        }

        var newOrder = new PurchaseOrder
        {
            PurchaseId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(), // Generate random Purchase ID
            ProductId = model.ProductId,
            Email = User.Identity.Name, // Assuming user is logged in
            DeliveryAddress = model.DeliveryAddress,
            City = model.City,
            ZipCode = model.ZipCode
        };

        _context.PurchaseOrders.Add(newOrder);
        _context.SaveChanges();

        return RedirectToAction("OrderConfirmation", new { id = newOrder.Id });
    }
}
