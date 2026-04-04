using System.Data.SQLite;
using tl2_recupercionparcial2_2025_augusto_dip.Models;


namespace tl2_recupercionparcial2_2025_augusto_dip.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly string _connectionString;

        public TareaRepository(IConfiguration configuration)
        {
            // Ignoramos el archivo de configuración y forzamos la ruta acá mismo
            _connectionString = "Data Source=tareas.db";
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
                            // CORREGIDO: Leemos la columna "Estado" (no "Categoria")
                            Estado = Enum.Parse<Estado>(reader["Estado"].ToString())
                        });
                    }
                }
            }
            return list;
        }

        public void Add(Tarea tarea)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Tareas (Titulo, Descripcion, Complejidad, Estado) VALUES (@Titulo, @Descripcion, @Complejidad, @Estado);", connection);
                command.Parameters.AddWithValue("@Titulo", tarea.Titulo);
                command.Parameters.AddWithValue("@Descripcion", tarea.Descripcion);
                // CORREGIDO: Los parámetros son @Complejidad y @Estado (no @Anio y @Categoria)
                command.Parameters.AddWithValue("@Complejidad", tarea.Complejidad);
                command.Parameters.AddWithValue("@Estado", tarea.Estado.ToString()); 
                command.ExecuteNonQuery();
            }
        }

        public void Update(Tarea tarea)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
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