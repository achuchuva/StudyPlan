namespace StudyPlanService;

public class Assignment : Task
{
    public Assignment(string name, TimeSpan estimatedTime, Subject subject, DateTime dueDate) : base (name, estimatedTime, subject, dueDate)
    {

    }

    public override double GetPriority(DateTime currentDate)
    {
        if (estimatedTime == TimeSpan.Zero)
        {
            return 0.0;
        }

        TimeSpan remainingTime = dueDate - currentDate;
        return estimatedTime.TotalSeconds / remainingTime.TotalSeconds;
    }
}