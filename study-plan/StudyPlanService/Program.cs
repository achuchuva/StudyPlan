using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        List<StudyTask> tasks = new List<StudyTask>
        {
            new StudyTask("Exercise 11F", new TimeSpan(3, 0, 0), new Subject("Maths"), new DateTime(2024, 2, 11)),
            new StudyTask("Read 12 Angry Men", new TimeSpan(2, 0, 0), new Subject("English"), new DateTime(2024, 2, 12)),
            new StudyTask("Complete Analysis Report", new TimeSpan(5, 0, 0), new Subject("Physics"), new DateTime(2024, 2, 10)),
        };

        WeekPlan weekPlan = new Scheduler(tasks).CreatePlan();

        // Convert object to JSON string
        string jsonString = JsonSerializer.Serialize(weekPlan);

        // Write JSON string to file
        File.WriteAllText("output.json", jsonString);

        Console.WriteLine("JSON file created successfully.");
    }
}