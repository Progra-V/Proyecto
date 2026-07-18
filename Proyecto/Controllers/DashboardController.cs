using Microsoft.AspNetCore.Mvc;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {
            // Verifica si el usuario ha iniciado sesión
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Dashboard";

            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(2);

            // Obtiene toda la información del Dashboard desde el Service
            var dashboard = await DashboardService.GetDashboardAsync();

            return View(dashboard);
        }
    }
}