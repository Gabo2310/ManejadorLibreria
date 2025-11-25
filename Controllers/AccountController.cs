using Microsoft.AspNetCore.Mvc;
using ManejadorLibreria.Services;
using ManejadorLibreria.Models;

namespace ManejadorLibreria.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
            {
                return RedirectToAction("Index", "Libros");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email y contraseña son requeridos";
                return View();
            }

            var usuario = await _authService.ValidarUsuario(email, password);

            if (usuario == null)
            {
                ViewBag.Error = "Email o contraseña incorrectos";
                return View();
            }

            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNombre", usuario.NombreUsuario);
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email);

            TempData["Success"] = $"¡Bienvenido {usuario.NombreUsuario}!";
            return RedirectToAction("Index", "Libros");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string nombreUsuario, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Todos los campos son requeridos";
                return View();
            }

            if (password.Length < 6)
            {
                ViewBag.Error = "La contraseña debe tener al menos 6 caracteres";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            var usuario = await _authService.RegistrarUsuario(nombreUsuario, email, password);

            if (usuario == null)
            {
                ViewBag.Error = "El email ya está registrado";
                return View();
            }

            TempData["Success"] = "¡Registro exitoso! Ahora puedes iniciar sesión";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Has cerrado sesión correctamente";
            return RedirectToAction("Index", "Home");
        }
    }
}