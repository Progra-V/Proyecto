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

            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Index", "Login");

            Proyecto.Models.User currentUser =
                JsonConvert.DeserializeObject<Proyecto.Models.User>(userJson);

            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(currentUser.Rol);

            var tickets = await TicketService.GetAll();

            return View(tickets);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");


            Session? session = JsonConvert.DeserializeObject<Session>(
                HttpContext.Session.GetString("session"));


            if (session?.User == null)
                return RedirectToAction("Index", "Login");


            Ticket? ticket = await TicketService.GetByTicketId(id);


            if (ticket == null)
                return NotFound();



            List<Comment> comments = await CommentService.GetByTicketId(id);



            string departmentName = "Sin departamento";

            if (ticket.DepartmentId.HasValue)
            {
                var department = await DepartmentService.GetById(
                    ticket.DepartmentId.Value
                );

                departmentName = department?.Nombre ?? "Departamento no encontrado";
            }



            TicketViewModels model = new TicketViewModels
            {
                Ticket = ticket,
                Comments = comments,
                ActiveSessionUserId = session.User.Id,
                DepartmentName = departmentName
            };


            return View(model);
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

            await CommentService.Create(comment);

            return Redirect("Detail?id=" + ticketId);
        }

        public async Task<IActionResult> DeleteComment(string ticketId, int commentId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await CommentService.Delete(commentId);

            return Redirect("Detail?id=" + ticketId);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int ticketId, string newStatus)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await TicketService.UpdateStatus(ticketId, newStatus);

            return RedirectToAction("Detail", new { id = ticketId });
        }
    }
}