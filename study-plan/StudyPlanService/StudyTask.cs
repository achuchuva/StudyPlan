public class StudyTask
{
    // Should possibly be a subclass of an activity. Another activity could be an event subclass
    public StudyTask(string name, TimeSpan estimatedTime, Subject subject, DateTime dueDate)
    {
        this.name = name;
        this.estimatedTime = estimatedTime;
        this.subject = subject;
        this.dueDate = dueDate;
    }

    public string name { get; set; }
    public TimeSpan estimatedTime { get; set; }
    public Subject subject { get; set; }
    public DateTime dueDate { get; set; }

    public void DecreaseEstimatedTime(TimeSpan time)
    {
        estimatedTime = estimatedTime.Subtract(time);

        if (estimatedTime < TimeSpan.Zero)
        {
            estimatedTime = TimeSpan.Zero;
        }
    }
}