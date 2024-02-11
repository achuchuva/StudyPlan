namespace StudyPlan.Data;

public class StudyBlock
{
    public StudyBlock(StudyTask task, TimeOnly startTime, TimeSpan duration)
    {
        this.task = task;
        this.startTime = startTime;
        this.duration = duration;
    }

    public StudyTask task { get; set; }
    public TimeOnly startTime { get; set; }
    public TimeSpan duration { get; set; }
}