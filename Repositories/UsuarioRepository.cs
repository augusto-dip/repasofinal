using System.Data.SQLite;
using tl2_recupercionparcial2_2025_augusto_dip.Models;

namespace tl2_recupercionparcial2_2025_augusto_dip.Repositories
{
    public interface IUserRepository { Usuario GetByCredentials(string user, string pass); }

    public class UserRepository : IUserRepository
    {
        private string _conn;
        public UserRepository(IConfiguration conf) { _conn = conf.GetConnectionString("DefaultConnection"); }

        public Usuario GetByCredentials(string user, string pass)
        {
            Usuario u = null;
            using (var conn = new SQLiteConnection(_conn)) {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT Id, User, Rol FROM Usuarios WHERE User=@u AND Pass=@p;", conn);
                cmd.Parameters.AddWithValue("@u", user);
                cmd.Parameters.AddWithValue("@p", pass);
                using (var reader = cmd.ExecuteReader()) {
                    if (reader.Read()) {
                        u = new Usuario { Id = Convert.ToInt32(reader["Id"]), Nombre = reader["Nombre"].ToString(), User = reader["User"].ToString(), Rol = reader["Rol"].ToString() };
                    }
                }
            }
            return u;
        }
    }
}