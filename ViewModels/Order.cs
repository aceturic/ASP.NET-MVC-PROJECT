// ✅ MODELS
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UsersApp.Models
{
    public enum OrderStatus
    {
        Processing,
        PendingCancellation,
        Cancelled,
        Rejected
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string DeliveryAddress { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string ZipCode { get; set; }
        [Required] public string PhoneNumber { get; set; }

        [ValidateNever] public string UserEmail { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Processing;

        // 🔁 Свързани продукти
        [ValidateNever]
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

}