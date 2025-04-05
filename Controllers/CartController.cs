using Microsoft.AspNetCore.Mvc;
using UsersApp.Data; 
using UsersApp.Models;
using UsersApp.ViewModels;
using UsersApp.Extensions;

public class CartController : Controller
{
    private readonly AppDbContext _context;
    private const string CartSessionKey = "Cart";

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    private List<CartItem> GetCart()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey);
        if (cart == null)
        {
            cart = new List<CartItem>();
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
        }
        return cart;
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
    }

    [HttpPost]
    public IActionResult UpdateCart(Dictionary<int, int> Quantities)
    {
        var cart = GetCart();

        foreach (var item in cart)
        {
            if (Quantities.TryGetValue(item.Product.Id, out int newQty) && newQty > 0)
            {
                item.Quantity = newQty;
            }
        }

        SaveCart(cart);
        return RedirectToAction("Index");

    }

    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

        var viewModel = new CartViewModel
        {
            Items = cart.Select(c => new CartItemViewModel
            {
                ProductId = c.Product.Id,
                ProductName = c.Product.Name,
                Price = c.Product.Price,
                Quantity = c.Quantity,
                ImageFileName = c.Product.ImageFileName
            }).ToList()
        };

        return View(viewModel);
    }

    public IActionResult AddToCart(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.Product.Id == id);
        if (item != null)
        {
            item.Quantity++;
        }
        else
        {
            cart.Add(new CartItem { Product = product, Quantity = 1 });
        }

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.Product.Id == id);
        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }

        return RedirectToAction("Index");
    }

    public IActionResult ClearCart()
    {
        SaveCart(new List<CartItem>());
        return RedirectToAction("Index");
    }
}
