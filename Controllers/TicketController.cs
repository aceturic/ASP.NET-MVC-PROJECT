using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsersApp.Models;
using UsersApp.Services;
using UsersApp.Models;
using UsersApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace UsersApp.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Chat()
        {
            string userId = User.Identity.Name;

            var activeTicket = await _ticketService.GetUserActiveTicketAsync(userId);
            if (activeTicket == null)
            {
                return RedirectToAction("Create");
            }

            return View(activeTicket);
        }

        public async Task<IActionResult> Create()
        {
            string userId = User.Identity.Name;

            var activeTicket = await _ticketService.GetUserActiveTicketAsync(userId);
            if (activeTicket != null)
            {
                return RedirectToAction("Chat");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            string userId = User.Identity.Name;
            ticket.UserId = userId;

            await _ticketService.CreateTicketAsync(ticket);
            return RedirectToAction("Chat");
        }
    }
}
