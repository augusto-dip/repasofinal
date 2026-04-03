using System.Collections.Generic;
using tl2_recupercionparcial2_2025_augusto_dip.Models;

namespace tl2_recupercionparcial2_2025_augusto_dip.Repositories
{
    public interface ITareaRepository
    {
        List<Tarea> GetAll();
        Tarea GetById(int id);
        void Add(Tarea tarea);
        void Update(Tarea tarea);
        void Delete(int id);
    }
}