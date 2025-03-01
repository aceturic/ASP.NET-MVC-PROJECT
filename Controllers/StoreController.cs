using Microsoft.AspNetCore.Mvc;
using UsersApp.Data;
using UsersApp.Models;
using System.Linq;

namespace UsersApp.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, string category, string priceSort)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            if (priceSort == "asc")
            {
                products = products.OrderBy(p => p.Price);
            }
            else if (priceSort == "desc")
            {
                products = products.OrderByDescending(p => p.Price);
            }
            else
            {
                products = products.OrderBy(p => p.Name);
            }

            ViewBag.Categories = _context.Products.Select(p => p.Category).Distinct().ToList();

            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategory = category;
            ViewBag.PriceSort = priceSort;

            return View(products.ToList());
        }
    }
}
