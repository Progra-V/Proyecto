using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Services;
using Newtonsoft.Json;
using Supabase.Gotrue;

namespace Proyecto.Controllers
{
    public class TicketController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(2);

            var tickets = await TicketService.getAll();

            return View(tickets);
        }


        public async Task<IActionResult> Detail(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            Session? session = JsonConvert.DeserializeObject<Session>(
                HttpContext.Session.GetString("session"));

            Ticket? detail = await TicketService.getTicketById(id);

            if (detail == null)
                return NotFound();

            detail.ActiveSessionUserId = session.User.Id;

            return View(detail);
        }


        [HttpPost]
        public async Task<IActionResult> PostComment(string ticketId, string commentText)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            Session? session = JsonConvert.DeserializeObject<Session>(
                HttpContext.Session.GetString("session"));

            Comment comment = new Comment
            {
                CommentText = commentText,
                TicketId = Convert.ToInt32(ticketId),
                CreatedBy = session.User.Id,
                CreatedAt = DateTime.Now
            };

            await TicketService.postComment(comment);

            return Redirect("Detail?id=" + ticketId);
        }


        public async Task<IActionResult> DeleteComment(string ticketId, int commentId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await TicketService.deleteComment(commentId);

            return Redirect("Detail?id=" + ticketId);
        }
    }
}