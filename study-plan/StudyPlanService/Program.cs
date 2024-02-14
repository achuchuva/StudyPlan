using System.Text.Json;

namespace StudyPlanService;

public class Program
{
    public static void Main(string[] args)
    {
        Task task = Task.Fetch(1);
    }
}