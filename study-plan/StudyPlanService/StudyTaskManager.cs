using Microsoft.Data.Sqlite;

public class StudyTaskManager
{
    private SqliteConnection GetDbConnection()
    {
        return new SqliteConnection("Data Source=../StudyPlan.db");
    }

    public StudyTask GetTask(int id)
    {
        SqliteConnection connection = GetDbConnection();
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT tasks.name, tasks.estimatedTime, subjects.name, tasks.dueDate FROM tasks INNER JOIN subjects ON tasks.subjectId = subjects.id WHERE tasks.id = $id";
        command.Parameters.AddWithValue("$id", id);

        StudyTask task = null;

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                task = new StudyTask(
                    reader.GetString(0),
                    TimeSpan.Parse(reader.GetString(1)),
                    new Subject(reader.GetString(2)),
                    DateTime.Parse(reader.GetString(3)));
            }
        }

        connection.Close();
        return task;
    }

    private int GetSubjectIdByName(string name)
    {
        SqliteConnection connection = GetDbConnection();
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT id FROM subjects WHERE name = $name";
        command.Parameters.AddWithValue("$name", name);

        int subjectId = 0;

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                subjectId = Int32.Parse(reader.GetString(0));
            }
        }

        connection.Close();
        return subjectId;
    }

    public void CreateTask(StudyTask task)
    {
        CreateSubject(task.subject);

        SqliteConnection connection = GetDbConnection();
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO tasks (name, subjectId, estimatedTime, dueDate) VALUES ($name, $subjectId, $estimatedTime, $dueDate)";
        command.Parameters.AddWithValue("$name", task.name);
        command.Parameters.AddWithValue("$subjectId", GetSubjectIdByName(task.subject.name));
        command.Parameters.AddWithValue("$estimatedTime", task.estimatedTime.ToString());
        command.Parameters.AddWithValue("$dueDate", task.dueDate.ToString());

        command.ExecuteNonQuery();
        connection.Close();
    }

    private void CreateSubject(Subject subject)
    {
        SqliteConnection connection = GetDbConnection();
        connection.Open();

        var commandCheck = connection.CreateCommand();
        commandCheck.CommandText = @"SELECT COUNT(*) FROM subjects WHERE name = $name";
        commandCheck.Parameters.AddWithValue("$name", subject.name);

        long count = (long) commandCheck.ExecuteScalar();

        if (count == 0)
        {
            var commandInsert = connection.CreateCommand();
            commandInsert.CommandText = @"INSERT INTO subjects (name) VALUES ($name)";
            commandInsert.Parameters.AddWithValue("$name", subject.name);

            commandInsert.ExecuteNonQuery();
        }

        connection.Close();
    }

    public void UpdateTask(int id, StudyTask task)
    {
        SqliteConnection connection = GetDbConnection();
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"UPDATE tasks SET name = $name, subjectId = $subjectId, estimatedTime = $estimatedTime, dueDate = $dueDate WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$name", task.name);
        command.Parameters.AddWithValue("$subjectId", GetSubjectIdByName(task.subject.name));
        command.Parameters.AddWithValue("$estimatedTime", task.estimatedTime.ToString());
        command.Parameters.AddWithValue("$dueDate", task.dueDate.ToString());

        command.ExecuteNonQuery();
        connection.Close();
    }

    public void DeleteTask(int id)
    {
        SqliteConnection connection = GetDbConnection();
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM tasks WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
        connection.Close();
    }
}
