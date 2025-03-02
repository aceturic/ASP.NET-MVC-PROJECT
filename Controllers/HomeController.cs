using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UsersApp.Models;

namespace UsersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

  

        public ActionResult Index()
        {
            // Sample data; replace with your data retrieval logic
            var locations = new List<Location>
        {
            new Location { Name = "Location 1", Latitude = 40.712776, Longitude = -74.005974, Description = "Description for Location 1" },
            new Location { Name = "Location 2", Latitude = 34.052235, Longitude = -118.243683, Description = "Description for Location 2" },
            new Location { Name = "Location 3", Latitude = 51.507351, Longitude = -0.127758, Description = "Description for Location 3" }
        };

            return View(locations);
        }

        [Authorize]
        public IActionResult Forum()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {


                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
        }
    }
}
