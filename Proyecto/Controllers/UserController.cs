using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class UserController : Controller
    {
        private bool IsAdmin()
        {
            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return false;

            User currentUser = JsonConvert.DeserializeObject<User>(userJson);

            return currentUser.Rol == 1;
        }


        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
                    return RedirectToAction("Index", "Login");

                return RedirectToAction("Index", "Ticket");
            }

            var users = await UserService.GetAll();

            return View(users);
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");

            if (!ModelState.IsValid)
                return View(user);

            await UserService.Create(user);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");


            var user = await UserService.GetById(id);

            if (user == null)
                return NotFound();

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");


            if (!ModelState.IsValid)
                return View(user);


            await UserService.Edit(user);

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> ChangeStatus(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Ticket");


            var user = await UserService.GetById(id);

            if (user == null)
                return NotFound();

            await UserService.Edit(user);

            return RedirectToAction("Index");
        }
    }
}