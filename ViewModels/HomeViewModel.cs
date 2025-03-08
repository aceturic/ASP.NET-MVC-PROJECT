namespace UsersApp.ViewModels
{
    public class HomeViewModel
    {
        public UserViewModel RegisteredUser { get; set; }
        public UsersApp.Models.Order Order { get; set; }
        public UsersApp.Models.Review Review { get; set; }
    }
}
