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
            // Start with all products.
            var products = _context.Products.AsQueryable();

            // Filter products based on search string.
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
            }

            // Filter products by selected category.
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            // Sort products based on price if priceSort is provided.
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
                // Default ordering (you can change this if needed).
                products = products.OrderBy(p => p.Name);
            }

            // Populate a list of distinct categories for the dropdown.
            ViewBag.Categories = _context.Products.Select(p => p.Category).Distinct().ToList();

            // Persist the current filters/sort options.
            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategory = category;
            ViewBag.PriceSort = priceSort;

            return View(products.ToList());
        }
    }
}
