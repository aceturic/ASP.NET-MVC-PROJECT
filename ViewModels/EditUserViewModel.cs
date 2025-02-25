using System.ComponentModel.DataAnnotations;

namespace UsersApp.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        public string FullName { get; set; }
    }
}
