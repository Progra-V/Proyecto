using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Services;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class DashboardController : BaseController
    {

        private User? GetCurrentUser()
        {
            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonConvert.DeserializeObject<User>(userJson);
        }


        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
                return RedirectToAction("Index", "Login");

            var currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            ViewData["Title"] = "Dashboard";

            ViewData["UserName"] =
                $"{currentUser.FirstName} {currentUser.LastName}";


            var dashboard = await DashboardService.GetDashboardAsync();

            return View(dashboard);
        }
    }
}