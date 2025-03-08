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

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
