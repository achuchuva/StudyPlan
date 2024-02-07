using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        List<Task> tasks = new List<Task>
        {
            new Task("Exercise 11F", new TimeSpan(3, 0, 0), new Subject("Maths"), new DateTime(2024, 2, 11)),
            new Task("Read 12 Angry Men", new TimeSpan(2, 0, 0), new Subject("English"), new DateTime(2024, 2, 12)),
            new Task("Complete Analysis Report", new TimeSpan(5, 0, 0), new Subject("Physics"), new DateTime(2024, 2, 10)),
        };

        WeekPlan weekPlan = new Scheduler(tasks).CreatePlan();

        // Convert object to JSON string
        string jsonString = JsonSerializer.Serialize(weekPlan);

        // Write JSON string to file
        File.WriteAllText("output.json", jsonString);

        Console.WriteLine("JSON file created successfully.");
    }
}