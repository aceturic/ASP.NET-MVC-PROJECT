using System;
using System.ComponentModel.DataAnnotations;

namespace UsersApp.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required, MaxLength(100)]
        public string Author { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
