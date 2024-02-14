namespace StudyPlanService.Tests;

public class TaskTest
{
    [Fact]
    public void TestDecreaseEstimatedTime()
    {
        Task task = new Task("Exercise 11F", new TimeSpan(3, 0, 0), new Subject("Maths"), new DateTime());

        task.DecreaseEstimatedTime(new TimeSpan(1, 30, 0));

        Assert.Equal(new TimeSpan(1, 30, 0), task.estimatedTime);
    }
}