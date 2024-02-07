public class StudyBlock
{
    public StudyBlock(Task task, TimeOnly startTime, TimeSpan duration)
    {
        this.task = task;
        this.startTime = startTime;
        this.duration = duration;
    }

    public Task task { get; set; }
    public TimeOnly startTime { get; set; }
    public TimeSpan duration { get; set; }
}