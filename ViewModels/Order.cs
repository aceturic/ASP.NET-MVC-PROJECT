using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UsersApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; } // Unique order ID

        [Required] // Ensure this field is populated
        public int ProductId { get; set; } // Foreign key to Product

        [ForeignKey("ProductId")]
        [ValidateNever]
        public virtual Product Product { get; set; } // Navigation property

        [Required]
        public decimal Price { get; set; } // Product Price

        [Required]
        public string DeliveryAddress { get; set; } // Shipping Address

        [Required]
        public string City { get; set; } // City

        [Required]
        public string ZipCode { get; set; } // Zip Code

        [ValidateNever]
        public string UserEmail { get; set; } // ✅ Store the user's email

        public DateTime OrderDate { get; set; } = DateTime.Now; // Auto-filled date
    }
}
