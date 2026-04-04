using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Clave para SetString
using tl2_recupercionparcial2_2025_augusto_dip.Repositories;
using tl2_recupercionparcial2_2025_augusto_dip.ViewModels;
using System;

namespace tl2_recupercionparcial2_2025_augusto_dip.Controllers
{
    public class LoginController : Controller
    {
        // NOTA: Asegurate de usar el nombre de interfaz correcto que tengas. 
        // Si la tuya se llama IUserRepository, cambialo acá.
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
                    // Si la BD devuelve un Rol nulo, le clavamos "User" por defecto para que no crashee
                    HttpContext.Session.SetString("Rol", usuario.Rol ?? "User"); 

                    return RedirectToAction("Index", "Tareas");
                }

                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(vm);
            }
            catch (Exception ex)
            {
                // Tira la excepción a la cara del desarrollador para ver si falló el SQL
                throw new Exception("ERROR CRÍTICO AL LOGUEAR: " + ex.Message);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); // Te devuelve al inicio
        }
    }
}