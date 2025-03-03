using UsersApp.Models;

namespace UsersApp.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
