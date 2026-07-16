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
                // Login mediante Supabase Authentication
                var session = await SupabaseAuthentication.SignIn(
                    user.Email,
                    user.Pwd
                );


                if (session == null)
                {
                    ViewBag.LoginMessage = "Correo o contraseña incorrectos.";
                    return View("Index", user);
                }


                // Buscar usuario dentro de la tabla users
                var currentUser = await UserService.GetByEmail(user.Email);


                if (currentUser == null)
                {
                    ViewBag.LoginMessage = "Usuario no registrado en el sistema.";
                    return View("Index", user);
                }


                if (!currentUser.IsActive)
                {
                    ViewBag.LoginMessage = "El usuario se encuentra desactivado.";
                    return View("Index", user);
                }


                // Guardar sesión de Supabase (autenticación)
                HttpContext.Session.SetString(
                    "session",
                    JsonConvert.SerializeObject(session)
                );


                // Guardar usuario interno de la aplicación
                HttpContext.Session.SetString(
                    "user",
                    JsonConvert.SerializeObject(currentUser)
                );


                return RedirectToAction(
                    "Index",
                    "Ticket"
                );
            }
            catch
            {
                ViewBag.LoginMessage =
                    "No fue posible comunicarse con el servidor.";

                return View("Index", user);
            }
        }
    }
}