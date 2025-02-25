namespace UsersApp.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; } // If your custom user has a separate name property; otherwise, use UserName.
    }
}
