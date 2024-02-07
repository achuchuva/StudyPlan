namespace StudyPlanService.Tests;

public class SchedulerTest
{
    private List<Task> tasks = new List<Task>
        {
            new Task("Exercise 11F", new TimeSpan(3, 0, 0), new Subject("Maths"), new DateTime(2024, 2, 8)),
            new Task("Read 12 Angry Men", new TimeSpan(2, 0, 0), new Subject("English"), new DateTime(2024, 2, 10)),
            new Task("Complete Analysis Report", new TimeSpan(5, 0, 0), new Subject("Physics"), new DateTime(2024, 2, 8)),
            new Task("Prepare for exam", new TimeSpan(4, 0, 0), new Subject("Economics"), new DateTime()),
            new Task("Write practice essay", new TimeSpan(2, 0, 0), new Subject("English"), new DateTime()),
            new Task("Review the Battle of Shiv", new TimeSpan(2, 0, 0), new Subject("History"), new DateTime()),
            new Task("Exercise 12E", new TimeSpan(1, 0, 0), new Subject("Maths"), new DateTime()),
        };

    [Fact]
    public void TestPlannerAlgorithm()
    {
        WeekPlan weeklyPlan = new Scheduler(tasks).CreatePlan();

        foreach (DayPlan day in weeklyPlan.Days)
        {
            foreach (StudyBlock studyBlock in day.StudyBlocks)
            {
                Assert.Equal(new StudyBlock(tasks[0], new TimeOnly(9, 0, 0), new TimeSpan(1, 30, 0)), studyBlock);
            }
        }
    }

    [Fact]
    public void TestGetTimeAvailableInTimeBlock()
    {
        TimeSpan studyTime = new TimeSpan(1, 0, 0);
        TimeSpan result = new Scheduler(tasks).GetTimeAvailableInTimeBlock(studyTime);

        Assert.Equal(studyTime, result);

        studyTime = new TimeSpan(3, 0, 0);
        result = new Scheduler(tasks).GetTimeAvailableInTimeBlock(studyTime);

        Assert.Equal(new TimeSpan(1, 30, 0), result);
    }

    [Fact]
    public void TestSelectTask()
    {
        Scheduler algorithm = new Scheduler(tasks);

        Task selectedTask = algorithm.SelectTask(new DateTime(2024, 2, 6));

        Assert.Equal(tasks[2], selectedTask);
    }

    [Fact]
    public void TestGetTimeRemainingInTask()
    {
        TimeSpan studyTime = new TimeSpan(1, 30, 0);
        TimeSpan result = new Scheduler(tasks).GetTimeRemainingInTask(studyTime, tasks[0]);

        Assert.Equal(studyTime, result);
    }
}