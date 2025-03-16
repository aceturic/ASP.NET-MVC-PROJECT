using System;
using System.ComponentModel.DataAnnotations;
using UsersApp.Models;

namespace UsersApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [Required]
        public string SenderId { get; set; } // User or Admin

        [Required]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
