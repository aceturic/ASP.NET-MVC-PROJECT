using UsersApp.Models;

namespace UsersApp.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public Users FullName { get; set; }
        public Review Review { get; set; } = new Review();
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
