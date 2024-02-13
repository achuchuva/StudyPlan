using Microsoft.Data.Sqlite;

namespace StudyPlan.Data;

public class StudyTask
{
    public StudyTask(string name, TimeSpan estimatedTime, Subject subject, DateTime dueDate)
    {
        this.name = name;
        this.estimatedTime = estimatedTime;
        this.subject = subject;
        this.dueDate = dueDate;
    }

    public int id { get; set; }
    public string name { get; set; }
    public TimeSpan estimatedTime { get; set; }
    public Subject subject { get; set; }
    public DateTime dueDate { get; set; }

    public static StudyTask Fetch(int id)
    {
        StudyTask task = null;

        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"SELECT tasks.name, tasks.estimatedTime, subjects.id, subjects.name, tasks.dueDate FROM tasks INNER JOIN subjects ON tasks.subjectId = subjects.id WHERE tasks.id = $id";
        command.Parameters.AddWithValue("$id", id);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                task = new StudyTask(
                    reader.GetString(0),
                    TimeSpan.Parse(reader.GetString(1)),
                    new Subject(reader.GetString(3))
                    {
                        id = Int32.Parse(reader.GetString(2))
                    },
                    DateTime.Parse(reader.GetString(4)))
                {
                    id = id
                };
            }
        }

        connection.Close();
        return task;
    }

    public static List<StudyTask> FetchAll()
    {
        List<StudyTask> tasks = new List<StudyTask>();

        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT tasks.id, tasks.name, tasks.estimatedTime, subjects.id, subjects.name, tasks.dueDate FROM tasks INNER JOIN subjects ON tasks.subjectId = subjects.id";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                tasks.Add(new StudyTask(
                    reader.GetString(1),
                    TimeSpan.Parse(reader.GetString(2)),
                    new Subject(reader.GetString(4))
                    {
                        id = Int32.Parse(reader.GetString(3))
                    },
                    DateTime.Parse(reader.GetString(5)))
                {
                    id = Int32.Parse(reader.GetString(0))
                });
            }
        }

        connection.Close();
        return tasks;
    }

    public void Create()
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO tasks (name, subjectId, estimatedTime, dueDate) VALUES ($name, $subjectId, $estimatedTime, $dueDate)";
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$subjectId", subject.id);
        command.Parameters.AddWithValue("$estimatedTime", estimatedTime.ToString());
        command.Parameters.AddWithValue("$dueDate", dueDate.ToString());

        id = Convert.ToInt32(command.ExecuteScalar());

        connection.Close();
    }

    public void Update()
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"UPDATE tasks SET name = $name, subjectId = $subjectId, estimatedTime = $estimatedTime, dueDate = $dueDate WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$subjectId", subject.id);
        command.Parameters.AddWithValue("$estimatedTime", estimatedTime.ToString());
        command.Parameters.AddWithValue("$dueDate", dueDate.ToString());

        command.ExecuteNonQuery();

        connection.Close();
    }

    public void Delete()
    {
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "study-plan", "StudyPlan.db");
        SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM tasks WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
        connection.Close();
    }

    public void DecreaseEstimatedTime(TimeSpan time)
    {
        estimatedTime = estimatedTime.Subtract(time);

        if (estimatedTime < TimeSpan.Zero)
        {
            estimatedTime = TimeSpan.Zero;
        }
    }
}