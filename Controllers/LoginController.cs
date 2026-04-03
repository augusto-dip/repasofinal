using Microsoft.AspNetCore.Mvc;
using tl2_recupercionparcial2_2025_augusto_dip.Repositories;

namespace tl2_recupercionparcial2_2025_augusto_dip.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUserRepository _repo;
        public AccesoController(IUserRepository repo) { _repo = repo; }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string user, string pass)
        {
            var u = _repo.GetByCredentials(user, pass);
            if (u != null) {
                HttpContext.Session.SetString("User", u.User);
                HttpContext.Session.SetString("Rol", u.Rol);
                return RedirectToAction("Index", "Tareas");
            }
            ViewBag.Error = "Datos incorrectos";
            return View();
        }
    }
}
