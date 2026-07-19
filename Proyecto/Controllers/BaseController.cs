using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public abstract class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var session = HttpContext.Session.GetString("session");

            if (string.IsNullOrEmpty(session))
                return;

            ViewData["CustomNavMenu"] = NavigationService.GetMenuPages(GetCurrentRole());
        }

        protected int GetCurrentRole()
        {
            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return 3;

            var user = JsonConvert.DeserializeObject<User>(userJson);

            return user?.RoleId ?? 3;
        }

        protected User? GetCurrentUser()
        {
            var userJson = HttpContext.Session.GetString("user");

            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonConvert.DeserializeObject<User>(userJson);
        }

        protected bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("session"));
        }
    }
}