public class TimeBlock
{
    public TimeOnly startTime { get; set; }
    public TimeSpan duration { get; set; }

    public TimeBlock(TimeOnly startTime, TimeSpan duration)
    {
        this.startTime = startTime;
        this.duration = duration;
    }

    public void UpdateTimeBlock(TimeSpan timeChange)
    {
        startTime = startTime.Add(timeChange);
        duration = duration.Subtract(timeChange);
        if (duration < TimeSpan.Zero)
        {
            duration = TimeSpan.Zero;
        }
    }
}
