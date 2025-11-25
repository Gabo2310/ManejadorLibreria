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

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
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

            // Login exitoso
            TempData["Success"] = $"¡Bienvenido {usuario.NombreUsuario}!";
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string nombreUsuario, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Todos los campos son requeridos";
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
    }
}