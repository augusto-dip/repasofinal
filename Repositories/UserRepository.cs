using System;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration; // ¡ESTO FALTABA!
using tl2_recupercionparcial2_2025_augusto_dip.Models;

namespace tl2_recupercionparcial2_2025_augusto_dip.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Usuario GetByUserAndPass(string user, string pass)
        {
            Usuario usuario = null;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                
                var command = new SQLiteCommand("SELECT Id, User, Rol FROM Usuarios WHERE User = @User AND Pass = @Pass;", connection);
                command.Parameters.AddWithValue("@User", user);
                command.Parameters.AddWithValue("@Pass", pass);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            User = reader["User"].ToString(),
                            Rol = reader["Rol"] != DBNull.Value ? reader["Rol"].ToString() : "User"
                        };
                    }
                }
            }
            return usuario;
        }
    }
}