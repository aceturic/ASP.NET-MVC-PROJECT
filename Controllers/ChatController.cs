using DiscordRPC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersApp.Models;
using UsersApp.Services;

namespace UsersApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;
        private readonly TicketService _ticketService;

        public ChatController(ChatService chatService, TicketService ticketService)
        {
            _chatService = chatService;
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int ticketId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction("Chat", "Ticket", new { id = ticketId });
            }

            var ticket = await _ticketService.GetTicketByIdAsync(ticketId);
            if (ticket == null) return NotFound();

            var message = new Message
            {
                TicketId = ticketId,
                SenderId = User.Identity.Name,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            await _chatService.SendMessageAsync(message);

            return RedirectToAction("Chat", "Ticket", new { id = ticketId });
        }
    }
}
