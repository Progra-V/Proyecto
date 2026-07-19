using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class CategoryController : BaseController
    {

        //Obtener Role del usuario actual desde la sesión
        private int GetCurrentRole()
        {
            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return 1;

            var user = JsonConvert.DeserializeObject<User>(userJson);

            return user?.RoleId ?? 1;
        }

        // Muestra la lista de categorías.
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Categories";

            var categories = await CategoryService.GetAll();

            return View(categories);
        }

        // Muestra el formulario para crear una categoría.
        public async Task<IActionResult> Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Nueva categoría";

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = new SelectList(
                departments.Where(x => x.IsActive),
                "Id",
                "Name");

            return View(new Category());
        }

        // Guarda una categoría.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Nueva categoría";

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = new SelectList(
                departments.Where(x => x.IsActive),
                "Id",
                "Name",
                category.DepartmentId);

            if (!ModelState.IsValid)
                return View(category);

            if (await CategoryService.Exists(category.Name, category.DepartmentId))
            {
                ModelState.AddModelError(
                    "Name",
                    "Ya existe una categoría con ese nombre para este departamento.");

                return View(category);
            }

            await CategoryService.Create(category);

            TempData["SuccessMessage"] = "La categoría fue creada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // Muestra el formulario para editar una categoría.
        public async Task<IActionResult> Edit(long id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Editar categoría";

            var category = await CategoryService.GetById(id);

            if (category == null)
                return NotFound();

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = new SelectList(
                departments.Where(x => x.IsActive),
                "Id",
                "Name",
                category.DepartmentId);

            return View(category);
        }

        // Actualiza una categoría.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Editar categoría";

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = new SelectList(
                departments.Where(x => x.IsActive),
                "Id",
                "Name",
                category.DepartmentId);

            if (!ModelState.IsValid)
                return View(category);

            await CategoryService.Update(category);

            TempData["SuccessMessage"] = "La categoría fue actualizada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // Desactiva una categoría.
        // Activa o desactiva una categoría.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(long id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await CategoryService.ChangeStatus(id);

            return RedirectToAction(nameof(Index));
        }
    }
}