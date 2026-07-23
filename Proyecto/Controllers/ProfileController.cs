using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Auth;
using Proyecto.Models;
using Proyecto.Models.ViewModels;
using Proyecto.Services;

namespace Proyecto.Controllers
{
    public class ProfileController : BaseController
    {
        public IActionResult Index()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            ProfileViewModel model = new ProfileViewModel
            {
                CurrentUser = currentUser,
                Phone = currentUser.Phone
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhone(string? phone)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            User? currentUser = GetCurrentUser();

            if (currentUser == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrWhiteSpace(phone))
            {
                TempData["Error"] = "El teléfono no puede quedar vacío.";
                return RedirectToAction(nameof(Index));
            }

            currentUser.Phone = phone.Trim();

            await UserService.Edit(currentUser);

            // Actualizar la sesión para reflejar el cambio sin re-login
            HttpContext.Session.SetString(
                "user",
                JsonConvert.SerializeObject(currentUser)
            );

            TempData["SuccessMessage"] = "Tu número de teléfono fue actualizado.";

            return RedirectToAction(nameof(Index));
        }

    

    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string newPassword, string confirmPassword)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                TempData["Error"] = "La contraseña debe tener al menos 6 caracteres.";
                return RedirectToAction(nameof(Index));
            }

            if (newPassword != confirmPassword)
            {
                TempData["Error"] = "Las contraseñas no coinciden.";
                return RedirectToAction(nameof(Index));
            }

            var sessionJson = HttpContext.Session.GetString("session");

            if (string.IsNullOrEmpty(sessionJson))
                return RedirectToAction("Index", "Login");

            var session = JsonConvert.DeserializeObject<Supabase.Gotrue.Session>(sessionJson);

            if (session?.AccessToken == null || session.RefreshToken == null)
                return RedirectToAction("Index", "Login");

            bool success = await SupabaseAuthentication.ChangePassword(
                session.AccessToken,
                session.RefreshToken,
                newPassword
            );

            if (!success)
            {
                TempData["Error"] = "No fue posible cambiar la contraseña. Intenta de nuevo.";
                return RedirectToAction(nameof(Index));
            }

            // Logout automático
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "Contraseña actualizada. Por favor inicia sesión de nuevo.";

            return RedirectToAction("Index", "Login");
        }
    }
}