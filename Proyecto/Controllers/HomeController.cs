using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using System.Diagnostics;

namespace Proyecto.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Ticket()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Titulo"] = "Mi proyecto MVC";
            ViewData["Año"] = 2026;
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
