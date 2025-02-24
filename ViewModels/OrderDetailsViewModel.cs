using System.ComponentModel.DataAnnotations;

namespace UsersApp.ViewModels
{
    public class OrderDetailsViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Delivery Address is required.")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Zip Code is required.")]
        public string ZipCode { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

    }

}
