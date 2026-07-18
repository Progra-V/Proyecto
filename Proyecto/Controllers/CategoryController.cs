using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class CategoryController : Controller
    {

        //Obtener Role del usuario actual desde la sesión
        private int GetCurrentRole()
        {
            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return 3;

            var user = JsonConvert.DeserializeObject<User>(userJson);

            return user?.RoleId ?? 3;
        }

        // Muestra la lista de categorías.
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Categories";
            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(GetCurrentRole());

            var categories = await CategoryService.getAll();

            return View(categories);
        }

        // Muestra el formulario para crear una categoría.
        public async Task<IActionResult> Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Nueva categoría";
            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(GetCurrentRole());

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = new SelectList(
                departments.Where(x => x.IsActive),
                "Id",
                "Name");

            return View(new Category());
        }

        // Guarda una categoría.
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = new SelectList(
                departments.Where(x => x.IsActive),
                "Id",
                "Name",
                category.DepartmentId);

            if (!ModelState.IsValid)
                return View(category);

            if (await CategoryService.exists(category.Name, category.DepartmentId))
            {
                ModelState.AddModelError(
                    "Name",
                    "Ya existe una categoría con ese nombre para este departamento.");

                return View(category);
            }

            await CategoryService.create(category);

            return RedirectToAction(nameof(Index));
        }
        // Muestra el formulario para editar una categoría.
        public async Task<IActionResult> Edit(long id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Editar categoría";
            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(GetCurrentRole());

            var category = await CategoryService.getById(id);

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
        public async Task<IActionResult> Edit(Category category)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var departments = await DepartmentService.GetAll();

            ViewBag.Departments = new SelectList(
                departments.Where(x => x.IsActive),
                "Id",
                "Name",
                category.DepartmentId);

            if (!ModelState.IsValid)
                return View(category);

            await CategoryService.update(category);

            return RedirectToAction(nameof(Index));
        }

        // Desactiva una categoría.
        public async Task<IActionResult> Disable(long id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            await CategoryService.disable(id);

            return RedirectToAction(nameof(Index));
        }
    }
}