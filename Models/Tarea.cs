namespace tl2_recupercionparcial2_2025_augusto_dip.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Complejidad { get; set; }
        public Estado Estado { get; set; } 
    }
}