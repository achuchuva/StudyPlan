using Microsoft.Data.Sqlite;

namespace StudyPlan.Data;

public class Subject
{
    public Subject(string name)
    {
        this.name = name;
    }

    public int id { get; set; }
    public string name { get; set; }

    public static List<Subject> FetchAll()
    {
        List<Subject> subjects = new List<Subject>();

        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM subjects";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                subjects.Add(new Subject(reader.GetString(1))
                {
                    id = Int32.Parse(reader.GetString(0))
                });
            }
        }

        connection.Close();
        return subjects;
    }

    public static Subject Fetch(int id)
    {
        Subject subject = null;

        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT subjects.name FROM subjects WHERE subjects.id = $id";
        command.Parameters.AddWithValue("$id", id);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                subject = new Subject(reader.GetString(0))
                {
                    id = id
                };
            }
        }

        connection.Close();
        return subject;
    }

    public void Create()
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO subjects (name) VALUES ($name)";
        command.Parameters.AddWithValue("$name", name);

        id = Convert.ToInt32(command.ExecuteScalar());

        connection.Close();
    }

    public void Update()
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"UPDATE subjects SET name = $name WHERE id = $id";
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();

        connection.Close();
    }

    public void Delete()
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM subjects WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
        connection.Close();
    }
}