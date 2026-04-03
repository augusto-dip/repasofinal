using System.ComponentModel.DataAnnotations;

namespace tl2_recupercionparcial2_2025_augusto_dip.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string User { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }
    }
}