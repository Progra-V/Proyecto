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

            return currentUser != null && currentUser.RoleId == 1;
        }



        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                if (string.IsNullOrEmpty(
                    HttpContext.Session.GetString("user")))
                {
                    return RedirectToAction("Index", "Login");
                }

                return RedirectToAction("Index", "Ticket");
            }


            var departments =
                await DepartmentService.GetAll();


            return View(departments);
        }




        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");


            var department = new Department
            {
                IsActive = true
            };


            return View(department);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Department department)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");


            department.Name =
                department.Name?.Trim() ?? string.Empty;


            department.Code =
                department.Code?.Trim().ToUpper() ?? string.Empty;


            department.Description =
                department.Description?.Trim();



            if (!ModelState.IsValid)
                return View(department);




            var existingDepartment =
                await DepartmentService.GetByName(
                    department.Name);



            if (existingDepartment != null)
            {
                ModelState.AddModelError(
                    nameof(department.Name),
                    "Ya existe un departamento con ese nombre."
                );

                return View(department);
            }



            var existingCode =
                await DepartmentService.GetByCode(
                    department.Code);



            if (existingCode != null)
            {
                ModelState.AddModelError(
                    nameof(department.Code),
                    "Ya existe un departamento con ese código."
                );

                return View(department);
            }




            await DepartmentService.Create(
                department);



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
            Department department)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");



            department.Name =
                department.Name?.Trim() ?? string.Empty;


            department.Code =
                department.Code?.Trim().ToUpper() ?? string.Empty;


            department.Description =
                department.Description?.Trim();




            if (!ModelState.IsValid)
                return View(department);




            var currentDepartment =
                await DepartmentService.GetById(
                    department.Id);



            if (currentDepartment == null)
                return NotFound();





            var existingDepartment =
                await DepartmentService.GetByName(
                    department.Name);



            if (existingDepartment != null &&
                existingDepartment.Id != department.Id)
            {
                ModelState.AddModelError(
                    nameof(department.Name),
                    "Ya existe otro departamento con ese nombre."
                );

                return View(department);
            }





            var existingCode =
                await DepartmentService.GetByCode(
                    department.Code);



            if (existingCode != null &&
                existingCode.Id != department.Id)
            {
                ModelState.AddModelError(
                    nameof(department.Code),
                    "Ya existe otro departamento con ese código."
                );

                return View(department);
            }






            currentDepartment.Name =
                department.Name;


            currentDepartment.Code =
                department.Code;


            currentDepartment.Description =
                department.Description;


            currentDepartment.IsActive =
                department.IsActive;





            await DepartmentService.Edit(
                currentDepartment);



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
                department);




            TempData["SuccessMessage"] =
                department.IsActive
                    ? "El departamento fue activado correctamente."
                    : "El departamento fue desactivado correctamente.";



            return RedirectToAction(nameof(Index));
        }
    }
}