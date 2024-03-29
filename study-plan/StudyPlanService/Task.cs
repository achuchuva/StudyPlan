using Microsoft.Data.Sqlite;

namespace StudyPlanService;

public class Task
{
    public Task(string name, TimeSpan estimatedTime, Subject subject, DateTime dueDate)
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

    public static Task Fetch(int id)
    {
        Task task;

        SqliteConnection connection = new SqliteConnection("Data Source=./study-plan/StudyPlan.db");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT tasks.id, tasks.name, tasks.estimatedTime, subjects.id, subjects.name, tasks.dueDate, tasks.type FROM tasks INNER JOIN subjects ON tasks.subjectId = subjects.id WHERE tasks.id = $id";
        command.Parameters.AddWithValue("$id", id);

        using (var reader = command.ExecuteReader())
        {
            reader.Read();
            task = TaskFactory.CreateTask(reader);
        }

        connection.Close();
        return task;
    }

    public static List<Task> FetchAll()
    {
        List<Task> tasks = new List<Task>();

        SqliteConnection connection = new SqliteConnection("Data Source=./study-plan/StudyPlan.db");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT tasks.id, tasks.name, tasks.estimatedTime, subjects.id, subjects.name, tasks.dueDate, tasks.type FROM tasks INNER JOIN subjects ON tasks.subjectId = subjects.id";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                tasks.Add(TaskFactory.CreateTask(reader));
            }
        }

        connection.Close();
        return tasks;
    }

    public void Create()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=./study-plan/StudyPlan.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO tasks (name, subjectId, estimatedTime, dueDate, type) VALUES ($name, $subjectId, $estimatedTime, $dueDate, $type)";
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$subjectId", subject.id);
        command.Parameters.AddWithValue("$estimatedTime", estimatedTime.ToString());
        command.Parameters.AddWithValue("$dueDate", dueDate.ToString());
        command.Parameters.AddWithValue("$type", "Assignment");

        id = Convert.ToInt32(command.ExecuteScalar());

        connection.Close();
    }

    public void Update()
    {
        SqliteConnection connection = new SqliteConnection("Data Source=./study-plan/StudyPlan.db");
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
        SqliteConnection connection = new SqliteConnection("Data Source=./study-plan/StudyPlan.db");
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

    public virtual double GetPriority(DateTime currentDate)
    {
        return 0.0;
    }
}
