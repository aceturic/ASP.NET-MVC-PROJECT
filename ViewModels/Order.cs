using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UsersApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; } 

        [Required] 
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public virtual Product Product { get; set; } 

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string FirstName { get; set; } 
        [Required]
        public string LastName { get; set; }

        [Required]
        public string DeliveryAddress { get; set; } 

        [Required]
        public string City { get; set; } 

        [Required]
        public string Country { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [ValidateNever]
        public string UserEmail { get; set; } 

        public DateTime OrderDate { get; set; } = DateTime.Now; 
    }
}
