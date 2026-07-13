using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class DepartmentController : Controller
    {
        private bool IsAdmin()
        {
            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return false;

            User? currentUser =
                JsonConvert.DeserializeObject<User>(userJson);

            return currentUser != null && currentUser.Rol == 1;
        }


        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                if (string.IsNullOrEmpty(
                    HttpContext.Session.GetString("user")
                ))
                {
                    return RedirectToAction("Index", "Login");
                }

                return RedirectToAction("Index", "Ticket");
            }

            var departments = await DepartmentService.GetAll();

            return View(departments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");

            var department = new Department
            {
                Activo = true
            };

            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Department department
        )
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");

            department.Nombre =
                department.Nombre?.Trim() ?? string.Empty;

            department.Descripcion =
                department.Descripcion?.Trim();

            if (!ModelState.IsValid)
                return View(department);

            var existingDepartment =
                await DepartmentService.GetByName(
                    department.Nombre
                );

            if (existingDepartment != null)
            {
                ModelState.AddModelError(
                    nameof(department.Nombre),
                    "Ya existe un departamento con ese nombre."
                );

                return View(department);
            }

            await DepartmentService.Create(department);

            TempData["SuccessMessage"] =
                "El departamento fue creado correctamente.";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");

            var department =
                await DepartmentService.GetById(id);

            if (department == null)
                return NotFound();

            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Department department
        )
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");

            department.Nombre =
                department.Nombre?.Trim() ?? string.Empty;

            department.Descripcion =
                department.Descripcion?.Trim();

            if (!ModelState.IsValid)
                return View(department);

            var currentDepartment =
                await DepartmentService.GetById(
                    department.Id
                );

            if (currentDepartment == null)
                return NotFound();

            var existingDepartment =
                await DepartmentService.GetByName(
                    department.Nombre
                );

            if (existingDepartment != null &&
                existingDepartment.Id != department.Id)
            {
                ModelState.AddModelError(
                    nameof(department.Nombre),
                    "Ya existe otro departamento con ese nombre."
                );

                return View(department);
            }

            currentDepartment.Nombre =
                department.Nombre;

            currentDepartment.Descripcion =
                department.Descripcion;

            currentDepartment.Activo =
                department.Activo;

            await DepartmentService.Edit(
                currentDepartment
            );

            TempData["SuccessMessage"] =
                "El departamento fue actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");

            var department =
                await DepartmentService.GetById(id);

            if (department == null)
                return NotFound();

            await DepartmentService.ChangeStatus(
                department
            );

            TempData["SuccessMessage"] =
                department.Activo
                    ? "El departamento fue activado correctamente."
                    : "El departamento fue desactivado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}
