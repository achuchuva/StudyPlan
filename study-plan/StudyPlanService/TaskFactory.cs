using Microsoft.Data.Sqlite;

namespace StudyPlanService;

public static class TaskFactory
{
    public static Task CreateTask(SqliteDataReader reader)
    {
        string type = reader.GetString(6);

        switch (type)
        {
            case "Assignment":
                return new Assignment(
                    reader.GetString(1),
                    TimeSpan.Parse(reader.GetString(2)),
                    new Subject(reader.GetString(4))
                    {
                        id = reader.GetInt32(3)
                    },
                    DateTime.Parse(reader.GetString(5)))
                {
                    id = reader.GetInt32(0)
                };
            case "ExamStudy":
                return new ExamStudy(
                    reader.GetString(1),
                    TimeSpan.Parse(reader.GetString(2)),
                    new Subject(reader.GetString(4))
                    {
                        id = reader.GetInt32(3)
                    },
                    DateTime.Parse(reader.GetString(5)))
                {
                    id = reader.GetInt32(0)
                };
            default:
                throw new Exception("Unknown task type");
        }
    }
}