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
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            var tickets = await TicketService.GetAll();

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = departments.ToDictionary(
                x => x.Id,
                x => x.Name
            );

            return View(tickets);
        }


        public async Task<IActionResult> Detail(long id)
        {
            // Validar sesión usando BaseController
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            // Obtener usuario usando BaseController
            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");


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


            // Resolver nombre del creador del ticket
            var creador = allUsers
                .FirstOrDefault(u => u.Id == ticket.CreatedBy);

            ticket.CreatedByName =
                creador != null
                    ? $"{creador.FirstName} {creador.LastName}"
                    : null;


            // Resolver nombre del técnico asignado
            var asignado = ticket.AssignedTo.HasValue
                ? allUsers.FirstOrDefault(
                    u => u.Id == ticket.AssignedTo.Value
                )
                : null;

            ticket.AssignedToName =
                asignado != null
                    ? $"{asignado.FirstName} {asignado.LastName}"
                    : null;


            // Resolver nombre del departamento
            ticket.DepartmentName =
                departments
                    .FirstOrDefault(
                        x => x.Id == ticket.DepartmentId
                    )
                    ?.Name;


            // Cargar categorías
            List<CategoryViewModel> categories =
                await CategoryService.GetAll();


            // Resolver nombre de la categoría
            ticket.CategoryName =
                categories
                    .FirstOrDefault(
                        x => x.Id == ticket.CategoryId
                    )
                    ?.Name;


            TicketViewModels model =
                new TicketViewModels
                {
                    Ticket = ticket,
                    Comments = comments,
                    ActiveSessionUserId = currentUser.Id,
                    DepartmentName = ticket.DepartmentName,
                    Departments = departments,
                    Categories = categories,
                    CurrentUser = currentUser
                };


            return View(model);
        }
        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModels model)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            model.Ticket.Title =
                model.Ticket.Title?.Trim() ?? string.Empty;

            model.Ticket.Description =
                model.Ticket.Description?.Trim();

            model.Ticket.Justification =
                model.Ticket.Justification?.Trim();

            if (string.IsNullOrEmpty(model.Ticket.Title))
            {
                ModelState.AddModelError(nameof(model.Ticket.Title), "El asunto es obligatorio.");
            }

            if (!model.Ticket.CategoryId.HasValue || model.Ticket.CategoryId.Value <= 0)
            {
                ModelState.AddModelError("Ticket.CategoryId", "Debe seleccionar una categoría.");
            }

            if (!ModelState.IsValid)
            {
                model.Departments =
                    await DepartmentService.GetAll();

                model.CurrentUser = currentUser;

                model.Categories =
                    model.Ticket.DepartmentId > 0
                        ? await CategoryService.GetByDepartment(
                            model.Ticket.DepartmentId
                        )
                        : new List<CategoryViewModel>();

                return View(model);
            }

            model.Ticket.CreatedBy = currentUser.Id;
            model.Ticket.Status = "Pendiente";

            var created =
                await TicketService.CreateTicket(model.Ticket);

            TempData["SuccessMessage"] =
                "El ticket fue creado correctamente.";

            return RedirectToAction(
                "Detail",
                new { id = created.Id }
            );
        }

        public async Task<IActionResult> Create()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            TicketViewModels model = new TicketViewModels
            {
                Ticket = new Ticket(),
                CurrentUser = currentUser,
                Departments = await DepartmentService.GetAll(),
                Categories = new List<CategoryViewModel>()
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(long id)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            Ticket? ticket = await TicketService.GetByTicketId(id);

            if (ticket == null)
                return NotFound();

            List<Department> departments =
            await DepartmentService.GetAll();

            List<CategoryViewModel> categories =
                await CategoryService.GetByDepartment(ticket.DepartmentId);

            // Resolver nombre de la categoría actual del ticket (sin filtrar por activa)
            if (ticket.CategoryId.HasValue)
            {
                var currentCategory = await CategoryService.GetById(ticket.CategoryId.Value);
                ticket.CategoryName = currentCategory?.Name;
            }

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
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrWhiteSpace(commentText))
            {
                TempData["Error"] =
                    "El comentario no puede estar vacío.";

                return RedirectToAction(
                    "Detail",
                    new { id = ticketId }
                );
            }

            Comment comment = new Comment
            {
                TicketId = Convert.ToInt64(ticketId),
                CommentText = commentText.Trim(),
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
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            Comment? comment =
                await CommentService.GetById(commentId);

            if (comment == null)
                return NotFound();

            // Solo el autor puede eliminar su comentario
            if (comment.CreatedBy != currentUser.Id)
            {
                TempData["Error"] =
                    "No tiene permiso para eliminar este comentario.";

                return RedirectToAction(
                    "Detail",
                    new { id = ticketId }
                );
            }

            await CommentService.Delete(commentId);

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
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            // Solo administrador o técnico pueden cambiar estados
            if (currentUser.RoleId != 2 &&
                currentUser.RoleId != 3)
            {
                TempData["Error"] =
                    "No tiene permiso para cambiar el estado del ticket.";

                return RedirectToAction(
                    "Detail",
                    new { id = ticketId }
                );
            }

            try
            {
                await TicketService.UpdateStatus(
                    ticketId,
                    newStatus
                );

                TempData["Success"] =
                    "El estado del ticket se actualizó correctamente.";
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
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

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
                    dueDate,
                    currentUser
                );

                TempData["Success"] =
                    "El ticket se actualizó correctamente.";

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
        public async Task<IActionResult> GetCategories(
            int departmentId)
        {
            if (!IsLoggedIn())
                return Unauthorized();

            var categories =
                await CategoryService.GetByDepartment(
                    departmentId
                );

            var result = categories.Select(c => new
            {
                id = c.Id,
                name = c.Name
            });

            return Json(result);
        }
    }
}