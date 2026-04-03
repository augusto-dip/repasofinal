using System.Data.SQLite;
using System.Runtime.InteropServices;
using tl2_recupercionparcial2_2025_augusto_dip.Models; // <-- Con esto alcanza para Models y el Enum Categoria

namespace tl2_recupercionparcial2_2025_augusto_dip.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly string _connectionString;

        public TareaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Tarea> GetAll()
        {
            var list = new List<Tarea>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT Id, Titulo, Descripcion, Complejidad, Estado FROM Tareas ORDER BY Titulo ASC;", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Tarea
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Titulo = reader["Titulo"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Complejidad = Convert.ToInt32(reader["Complejidad"]),
                            // Enum.Parse funciona perfecto porque la clase y el Enum comparten namespace
                            Estado = Enum.Parse<Estado>(reader["Categoria"].ToString())
                        });
                    }
                }
            }
            return list;
        }

        // ... (Los métodos Add, Update, Delete y GetById quedan exactamente iguales al código anterior)

        public void Add(Tarea tarea)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                // SQL del PDF 
                var command = new SQLiteCommand("INSERT INTO Tareas (Titulo, Descripcion, Complejidad, Estado) VALUES (@Titulo, @Descripcion, @Complejidad, @Estado);", connection);
                command.Parameters.AddWithValue("@Titulo", tarea.Titulo);
                command.Parameters.AddWithValue("@Descripcion", tarea.Descripcion);
                command.Parameters.AddWithValue("@Anio", tarea.Complejidad);
                command.Parameters.AddWithValue("@Categoria", tarea.Estado.ToString()); // Guardamos como texto
                command.ExecuteNonQuery();
            }
        }

        public void Update(Tarea tarea)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                // SQL del PDF 
                var command = new SQLiteCommand("UPDATE Tareas SET Titulo = @Titulo, Descripcion = @Descripcion, Complejidad = @Complejidad, Estado = @Estado WHERE Id = @Id;", connection);
                command.Parameters.AddWithValue("@Titulo", tarea.Titulo);
                command.Parameters.AddWithValue("@Descripcion", tarea.Descripcion);
                command.Parameters.AddWithValue("@Complejidad", tarea.Complejidad);
                command.Parameters.AddWithValue("@Estado", tarea.Estado.ToString());
                command.Parameters.AddWithValue("@Id", tarea.Id);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                // SQL del PDF
                var command = new SQLiteCommand("DELETE FROM Tareas WHERE Id = @Id;", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public Tarea GetById(int id)
        {
            Tarea tarea = null;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                // SQL del PDF 
                var command = new SQLiteCommand("SELECT Id, Titulo, Descripcion, Complejidad, Estado FROM Tareas WHERE Id = @Id;", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tarea = new Tarea
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Titulo = reader["Titulo"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Complejidad = Convert.ToInt32(reader["Complejidad"]),
                            Estado = Enum.Parse<Estado>(reader["Estado"].ToString())
                        };
                    }
                }
            }
            return tarea;
        }
    }
}