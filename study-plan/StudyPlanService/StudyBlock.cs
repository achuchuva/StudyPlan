namespace StudyPlanService;

public class StudyBlock : TimeBlock
{
    public StudyBlock(Task task, TimeOnly startTime, TimeSpan duration) : base (startTime, duration)
    {
        this.task = task;
    }

    public Task task { get; set; }
}