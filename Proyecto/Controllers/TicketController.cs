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

            Proyecto.Models.User? currentUser =
                JsonConvert.DeserializeObject<Proyecto.Models.User>(userJson);

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(currentUser.Rol);

            var tickets = await TicketService.GetAll();

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = departments.ToDictionary(
                x => x.Id,
                x => x.Nombre
            );

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

            Ticket? detail = await TicketService.GetByTicketId(id);

            if (detail == null)
                return NotFound();

            if (detail.DepartamentoId.HasValue)
            {
                var department = await DepartmentService.GetById(
                    detail.DepartamentoId.Value
                );

                ViewBag.DepartmentName =
                    department?.Nombre ?? "Departamento no encontrado";
            }
            else
            {
                ViewBag.DepartmentName = "Sin departamento";
            }

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

            if (session?.User == null)
                return RedirectToAction("Index", "Login");

            Comment comment = new Comment
            {
                CommentText = commentText,
                TicketId = Convert.ToInt32(ticketId),
                CreatedBy = session.User.Id,
                CreatedAt = DateTime.Now
            };

            await TicketService.CreateComment(comment);

            return Redirect("Detail?id=" + ticketId);
        }


        public async Task<IActionResult> DeleteComment(string ticketId, int commentId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await TicketService.DeleteComment(commentId);

            return Redirect("Detail?id=" + ticketId);
        }
    }
}