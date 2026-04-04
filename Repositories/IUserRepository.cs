using tl2_recupercionparcial2_2025_augusto_dip.Models;

namespace tl2_recupercionparcial2_2025_augusto_dip.Repositories
{
    public interface IUserRepository
    {
        Usuario GetByUserAndPass(string user, string pass);
    }
}