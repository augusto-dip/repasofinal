using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_recupercionparcial2_2025_augusto_dip.Models;
using tl2_recupercionparcial2_2025_augusto_dip.Repositories;
using tl2_recupercionparcial2_2025_augusto_dip.Services;
using tl2_recupercionparcial2_2025_augusto_dip.ViewModels;

namespace tl2_recupercionparcial2_2025_augusto_dip.Controllers
{
    public class TareasController : Controller
    {
        private readonly ITareaRepository _repo;
        private readonly IAuthenticationService _auth;

        public TareasController(ITareaRepository repo, IAuthenticationService auth)
        {
            _repo = repo;
            _auth = auth;
        }

        private SelectList ObtenerEstados()
        {
            var items = Enum.GetValues(typeof(Estado)).Cast<Estado>()
                .Select(c => new SelectListItem { Value = c.ToString(), Text = c.ToString() }).ToList();
            return new SelectList(items, "Value", "Text");
        }

        public IActionResult Index()
        {
            try
            {
                if (!_auth.IsAuthenticated()) return RedirectToAction("Index", "Login");

                var tareas = _repo.GetAll();
                var viewModels = tareas.Select(t => new TareaIndexViewModel
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descripcion = t.Descripcion, 
                    Complejidad = t.Complejidad,
                    Estado = t.Estado.ToString()
                }).ToList();

                return View(viewModels);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Alta() // Renombrado de Create a Alta
        {
            try
            {
                if (!_auth.HasAccessLevel("Admin")) return RedirectToAction("Index");

                var vm = new TareaCreateViewModel { ListaEstados = ObtenerEstados() };
                return View(vm);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult Alta(TareaCreateViewModel vm) // Renombrado
        {
            try
            {
                if (!_auth.HasAccessLevel("Admin")) return RedirectToAction("Index");

                if (ModelState.IsValid)
                {
                    int sumaActual = _repo.GetAll().Sum(t => t.Complejidad);
                    
                    if (sumaActual + vm.Complejidad > 50)
                    {
                        ModelState.AddModelError(string.Empty, "La suma total de complejidad no puede superar 50.");
                        vm.ListaEstados = ObtenerEstados();
                        return View(vm);
                    }

                    _repo.Add(new Tarea { 
                        Titulo = vm.Titulo, 
                        Descripcion = vm.Descripcion, 
                        Complejidad = vm.Complejidad, 
                        Estado = vm.Estado 
                    });
                    
                    return RedirectToAction("Index");
                }
                
                vm.ListaEstados = ObtenerEstados();
                return View(vm);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Modificar(int id) // Renombrado de Edit a Modificar
        {
            try
            {
                if (!_auth.HasAccessLevel("Admin")) return RedirectToAction("Index");

                var t = _repo.GetById(id);
                if (t == null) return NotFound();

                var vm = new TareaUpdateViewModel
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descripcion = t.Descripcion,
                    Complejidad = t.Complejidad,
                    Estado = t.Estado,
                    ListaEstados = ObtenerEstados() 
                };

                return View(vm);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult Modificar(TareaUpdateViewModel vm) // Renombrado
        {
            try
            {
                if (!_auth.HasAccessLevel("Admin")) return RedirectToAction("Index");

                if (ModelState.IsValid)
                {
                    var tareaOriginal = _repo.GetById(vm.Id);
                    int sumaActual = _repo.GetAll().Sum(t => t.Complejidad);
                    
                    if ((sumaActual - tareaOriginal.Complejidad + vm.Complejidad) > 50)
                    {
                        ModelState.AddModelError(string.Empty, "La suma total de complejidad no puede superar 50.");
                        vm.ListaEstados = ObtenerEstados();
                        return View(vm);
                    }

                    var tarea = new Tarea
                    {
                        Id = vm.Id,
                        Titulo = vm.Titulo,
                        Descripcion = vm.Descripcion,
                        Complejidad = vm.Complejidad,
                        Estado = vm.Estado
                    };

                    _repo.Update(tarea);
                    return RedirectToAction("Index");
                }

                vm.ListaEstados = ObtenerEstados();
                return View(vm);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Borrar(int id)
        {
            try
            {
                if (!_auth.HasAccessLevel("Admin")) return RedirectToAction("Index");
                
                var tarea = _repo.GetById(id);
                if (tarea == null) return NotFound();

                // Le pasamos la tarea a la vista para confirmar
                return View(tarea); 
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost, ActionName("Borrar")]
        public IActionResult BorrarConfirmado(int id)
        {
            try
            {
                if (!_auth.HasAccessLevel("Admin")) return RedirectToAction("Index");
                
                _repo.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}