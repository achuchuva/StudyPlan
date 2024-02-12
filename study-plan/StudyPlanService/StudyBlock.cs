public class StudyBlock : TimeBlock
{
    public StudyBlock(StudyTask task, TimeOnly startTime, TimeSpan duration) : base (startTime, duration)
    {
        this.task = task;
    }

    public StudyTask task { get; set; }
}