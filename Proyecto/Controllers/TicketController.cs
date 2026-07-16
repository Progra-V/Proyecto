using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class TicketController : Controller
    {
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Index", "Login");

            User currentUser = JsonConvert.DeserializeObject<User>(userJson)!;

            ViewData["CustomNavMenu"] =
                NavigationService.GetMenuPages(currentUser.RoleId);

            var tickets = await TicketService.GetAll();

            return View(tickets);
        }


        public async Task<IActionResult> Detail(long id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Index", "Login");

            User currentUser = JsonConvert.DeserializeObject<User>(userJson)!;

            Ticket? ticket = await TicketService.GetByTicketId(id);

            if (ticket == null)
                return NotFound();

            List<Comment> comments =
                await CommentService.GetByTicketId(id);

            List<Department> departments =
                await DepartmentService.GetAll();

            List<User> technicians =
                (await UserService.GetAll())
                .Where(x => x.RoleId == 2 && x.IsActive)
                .ToList();

            TicketViewModels model = new TicketViewModels
            {
                Ticket = ticket,
                Comments = comments,
                ActiveSessionUserId = currentUser.Id,
                DepartmentName = ticket.DepartmentName,
                Departments = departments,
                Technicians = technicians
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> PostComment(
            string ticketId,
            string commentText)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Index", "Login");

            User currentUser = JsonConvert.DeserializeObject<User>(userJson)!;

            Comment comment = new Comment
            {
                TicketId = Convert.ToInt64(ticketId),
                CommentText = commentText,
                CreatedBy = currentUser.Id,
                CreatedAt = DateTime.UtcNow
            };

            await CommentService.Create(comment);

            return RedirectToAction(
                "Detail",
                new { id = ticketId }
            );
        }


        public async Task<IActionResult> DeleteComment(
            string ticketId,
            long commentId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            return RedirectToAction(
                "Detail",
                new { id = ticketId }
            );
        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatus(
            long ticketId,
            string newStatus)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await TicketService.UpdateStatus(
                ticketId,
                newStatus
            );

            return RedirectToAction(
                "Detail",
                new { id = ticketId }
            );
        }



        [HttpPost]
        public async Task<IActionResult> UpdateTicket(
    long ticketId,
    string status,
    string priority,
    string risk,
    string category,
    int departmentId,
    int? assignedTo)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await TicketService.UpdateTicket(
                ticketId,
                status,
                priority,
                risk,
                category,
                departmentId,
                assignedTo
            );

            return RedirectToAction(
                "Detail",
                new { id = ticketId }
            );
        }
    }
    
}