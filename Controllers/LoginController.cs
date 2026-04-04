using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 
using tl2_recupercionparcial2_2025_augusto_dip.Repositories;
using tl2_recupercionparcial2_2025_augusto_dip.ViewModels;
using System;

namespace tl2_recupercionparcial2_2025_augusto_dip.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepo; 

        public LoginController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel vm)
        {
            try 
            {
                if (!ModelState.IsValid) return View(vm);

                var usuario = _userRepo.GetByUserAndPass(vm.User, vm.Pass);

                if (usuario != null)
                {
                    HttpContext.Session.SetString("User", usuario.User);
                    HttpContext.Session.SetString("Rol", usuario.Rol ?? "User"); 

                    return RedirectToAction("Index", "Tareas");
                }

                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(vm);
            }
            catch (Exception ex)
            {
                // CORRECCIÓN: Ahora el error de SQLite se mostrará en la interfaz en lugar de romper la app.
                ModelState.AddModelError(string.Empty, $"Error de BD (Revisá los nombres de columnas en el Repositorio): {ex.Message}");
                return View(vm);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login"); 
        }
    }
}