using Microsoft.AspNetCore.Mvc.Rendering;
using tl2_recupercionparcial2_2025_augusto_dip.Models; // <-- ¡Cambiado! Antes decía Enums
using System.ComponentModel.DataAnnotations;

namespace tl2_recupercionparcial2_2025_augusto_dip.ViewModels
{
    public class TareaIndexViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Complejidad { get; set; }
        public string Estado { get; set; }
    }

    public class TareaCreateViewModel
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Debe tener entre 5 y 500 caracteres")]
        public string Descripcion { get; set; }

        [Range(1, 10, ErrorMessage = "Complejidad invalida, debe estar en este rango (1-10)")]
        public int Complejidad { get; set; }

        [Required(ErrorMessage = "Seleccione un estado")]
        public Estado Estado { get; set; }

        public SelectList? ListaEstados { get; set; }
    }

    public class TareaUpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Descripcion { get; set; }

        [Required]
        [Range(1, 10)]
        public int Complejidad { get; set; }

        [Required]
        public Estado Estado { get; set; }

        public SelectList? ListaEstados { get; set; }
    }
}