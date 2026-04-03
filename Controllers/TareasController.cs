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
                if (!_auth.IsAuthenticated()) return RedirectToAction("Login", "Acceso");

                var tareas = _repo.GetAll();
                var viewModels = tareas.Select(t => new TareaIndexViewModel
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descripcion = t.Descripcion, // Corregido: antes decía t.Titulo
                    Complejidad = t.Complejidad,
                    Estado = t.Estado.ToString()
                }).ToList();

                return View(viewModels);
            }
            catch (Exception)
            {
                // El parcial pide redirigir a una vista de error sin detalles técnicos
                return View("Error");
            }
        }

        public IActionResult Create()
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
        public IActionResult Create(TareaCreateViewModel vm)
        {
            try
            {
                if (!_auth.HasAccessLevel("Admin")) return RedirectToAction("Index");

                if (ModelState.IsValid)
                {
                    // REGLA DE NEGOCIO: La suma total no puede superar 50
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

        public IActionResult Edit(int id)
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
        public IActionResult Edit(TareaUpdateViewModel vm)
        {
            try
            {
                if (!_auth.HasAccessLevel("Admin")) return RedirectToAction("Index");

                if (ModelState.IsValid)
                {
                    // REGLA DE NEGOCIO AL MODIFICAR
                    var tareaOriginal = _repo.GetById(vm.Id);
                    int sumaActual = _repo.GetAll().Sum(t => t.Complejidad);
                    
                    // Lógica clave: Restamos la complejidad que tenía esta tarea y sumamos la nueva ingresada
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

        public IActionResult Delete(int id)
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