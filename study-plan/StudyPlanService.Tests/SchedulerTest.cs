namespace StudyPlanService.Tests;

public class SchedulerTest
{
    private List<StudyTask> tasks = new List<StudyTask>
        {
            new StudyTask("Exercise 11F", new TimeSpan(3, 0, 0), new Subject("Maths"), DateTime.Today.AddDays(2)),
            new StudyTask("Read 12 Angry Men", new TimeSpan(2, 0, 0), new Subject("English"), DateTime.Today.AddDays(4)),
        };

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

        StudyTask selectedTask = algorithm.SelectTask(DateTime.Today);

        Assert.Equal(tasks[0], selectedTask);
    }

    [Fact]
    public void TestGetTimeRemainingInTask()
    {
        TimeSpan studyTime = new TimeSpan(1, 30, 0);
        TimeSpan result = new Scheduler(tasks).GetTimeRemainingInTask(studyTime, tasks[0]);

        Assert.Equal(studyTime, result);
    }
}