using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class CategoryController : Controller
    {
        // Muestra la lista de categorías.
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Categories";
            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(2);

            var categories = await CategoryService.getAll();

            return View(categories);
        }

        // Muestra el formulario para crear una categoría.
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "New Category";
            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(2);

            return View();
        }

        // Guarda una categoría.
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
                return View(category);

            if (await CategoryService.exists(category.Name, category.DepartmentId))
            {
                ModelState.AddModelError("Name", "Ya existe una categoría con ese nombre para este departamento.");
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

            ViewData["Title"] = "Edit Category";
            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(2);

            var category = await CategoryService.getById(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // Actualiza una categoría.
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

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