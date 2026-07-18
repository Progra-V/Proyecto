using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Services;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class DashboardController : Controller
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
        public async Task<IActionResult> Index()
        {
            // Verifica si el usuario ha iniciado sesión
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Dashboard";

            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(GetCurrentRole());

            // Obtiene toda la información del Dashboard desde el Service
            var dashboard = await DashboardService.GetDashboardAsync();

            return View(dashboard);
        }
    }
}