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

        StudyTaskManager studyTaskManager = new StudyTaskManager();

        studyTaskManager.CreateTask(tasks[1]);
        StudyTask task = studyTaskManager.GetTask(2);
        Console.WriteLine(task.name);
        Console.WriteLine(task.subject.name);
        Console.WriteLine(task.estimatedTime);
        Console.WriteLine(task.dueDate);
    }
}