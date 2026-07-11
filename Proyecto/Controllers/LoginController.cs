using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Auth;
using Proyecto.Models;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ValidateLogin(UserModel user)
        {
            try
            {
                // Validar credenciales con Supabase Authentication
                var session = await SupabaseAuthentication.SignIn(user.Email, user.Pwd);

                if (session == null)
                {
                    ViewBag.LoginMessage = "Correo o contraseña incorrectos.";
                    return View("Index", user);
                }

                // Validar información adicional del usuario en la tabla Usuarios
                var testUser = await UserService.GetByEmail(user.Email);

                if (testUser == null)
                {
                    ViewBag.LoginMessage = "Usuario no encontrado.";
                    return View("Index", user);
                }

                if (!testUser.Activo)
                {
                    ViewBag.LoginMessage = "El usuario no se encuentra activo.";
                    return View("Index", user);
                }

                // Guardar sesión del usuario autenticado
                HttpContext.Session.SetString(
                    "session",
                    JsonConvert.SerializeObject(session)
                );
                HttpContext.Session.SetString(
    "user",
    JsonConvert.SerializeObject(testUser)
);

                return RedirectToAction("Index", "Ticket");
            }
            catch
            {
                ViewBag.LoginMessage = "No fue posible comunicarse con el servidor.";
                return View("Index", user);
            }
        }
    }
}