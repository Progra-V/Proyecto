using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class TicketController : BaseController
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

            List<User> allUsers =
                await UserService.GetAll();

            // Resolver nombres de comentarios
            foreach (var c in comments)
            {
                c.CreatedByName = allUsers
                    .Where(u => u.Id == c.CreatedBy)
                    .Select(u => $"{u.FirstName} {u.LastName}")
                    .FirstOrDefault();
            }

            // Resolver nombres del ticket
            var creador = allUsers.FirstOrDefault(u => u.Id == ticket.CreatedBy);

            ticket.CreatedByName =
                creador != null
                    ? $"{creador.FirstName} {creador.LastName}"
                    : null;

            var asignado = ticket.AssignedTo.HasValue
                ? allUsers.FirstOrDefault(u => u.Id == ticket.AssignedTo.Value)
                : null;

            ticket.AssignedToName =
                asignado != null
                    ? $"{asignado.FirstName} {asignado.LastName}"
                    : null;

            ticket.DepartmentName =
                departments
                .FirstOrDefault(x => x.Id == ticket.DepartmentId)
                ?.Name;

            List<CategoryViewModel> categories =
                await CategoryService.GetAll();

            ticket.CategoryName =
                categories
                .FirstOrDefault(x => x.Id == ticket.CategoryId)
                ?.Name;

            TicketViewModels model = new TicketViewModels
            {
                Ticket = ticket,
                Comments = comments,
                ActiveSessionUserId = currentUser.Id,
                DepartmentName = ticket.DepartmentName,
                Departments = departments,
                Categories = categories,
                CurrentUser = currentUser,
            };

            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            List<Department> departments = await DepartmentService.GetAll();

            TicketViewModels model = new TicketViewModels
            {
                Ticket = new Ticket(),
                Departments = departments
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModels model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Index", "Login");

            User currentUser = JsonConvert.DeserializeObject<User>(userJson)!;

            model.Ticket.Title = model.Ticket.Title?.Trim() ?? string.Empty;
            model.Ticket.Description = model.Ticket.Description?.Trim();
            model.Ticket.Justification = model.Ticket.Justification?.Trim();

            if (string.IsNullOrEmpty(model.Ticket.Title))
            {
                ModelState.AddModelError(nameof(model.Ticket.Title), "El asunto es obligatorio.");
            }

            if (!ModelState.IsValid)
            {
                model.Departments = await DepartmentService.GetAll();
                return View(model);
            }

            model.Ticket.CreatedBy = currentUser.Id;
            model.Ticket.Status = "Pendiente";

            var created = await TicketService.CreateTicket(model.Ticket);

            TempData["SuccessMessage"] = "El ticket fue creado correctamente.";

            return RedirectToAction("Detail", new { id = created.Id });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModels model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Index", "Login");

            User currentUser = JsonConvert.DeserializeObject<User>(userJson)!;

            model.Ticket.Title = model.Ticket.Title?.Trim() ?? string.Empty;
            model.Ticket.Description = model.Ticket.Description?.Trim();
            model.Ticket.Justification = model.Ticket.Justification?.Trim();

            if (string.IsNullOrEmpty(model.Ticket.Title))
            {
                ModelState.AddModelError(nameof(model.Ticket.Title), "El asunto es obligatorio.");
            }

            if (!ModelState.IsValid)
            {
                model.Departments = await DepartmentService.GetAll();
                return View(model);
            }

            model.Ticket.CreatedBy = currentUser.Id;
            model.Ticket.Status = "Pendiente";

            var created = await TicketService.CreateTicket(model.Ticket);

            TempData["SuccessMessage"] = "El ticket fue creado correctamente.";

            return RedirectToAction("Detail", new { id = created.Id });
        }


        public async Task<IActionResult> Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Index", "Login");

            User currentUser = JsonConvert.DeserializeObject<User>(userJson)!;

            TicketViewModels model = new TicketViewModels
            {
                Ticket = new Ticket(),
                CurrentUser = currentUser,
                Departments = await DepartmentService.GetAll(),
                Categories = new List<CategoryViewModel>(),
            };

            return View(model);
        }





        public async Task<IActionResult> Edit(long id)
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

            List<Department> departments =
                await DepartmentService.GetAll();

            List<CategoryViewModel> categories =
                await CategoryService.GetByDepartment(ticket.DepartmentId);

            TicketViewModels model = new TicketViewModels
            {
                Ticket = ticket,
                Departments = departments,
                Categories = categories,
                CurrentUser = currentUser,
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


        public async Task<IActionResult> DeleteComment(string ticketId, long commentId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await CommentService.Delete(commentId);

            return RedirectToAction("Detail", new { id = ticketId });
        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatus(
            long ticketId,
            string newStatus)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            try
            {
                await TicketService.UpdateStatus(
                    ticketId,
                    newStatus
                );

                TempData["Success"] = "El estado del ticket se actualizó correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

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
            long categoryId,
            int departmentId,
            int? assignedTo,
            DateTime? dueDate)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return RedirectToAction("Index", "Login");

            User currentUser = JsonConvert.DeserializeObject<User>(userJson)!;

            try
            {
                await TicketService.UpdateTicket(
                    ticketId,
                    status,
                    priority,
                    risk,
                    categoryId,
                    departmentId,
                    assignedTo,
                    currentUser
                );

                TempData["Success"] = "El ticket se actualizó correctamente.";

                return RedirectToAction(
                    "Detail",
                    new { id = ticketId }
                );
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(
                    "Edit",
                    new { id = ticketId }
                );
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories(int departmentId)
        {
            var categories = await CategoryService.GetByDepartment(departmentId);

            var result = categories.Select(c => new
            {
                id = c.Id,
                name = c.Name
            });

            return Json(result);
        }
    }
}