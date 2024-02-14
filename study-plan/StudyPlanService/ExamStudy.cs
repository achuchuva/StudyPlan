namespace StudyPlanService;

public class ExamStudy : Task
{
    public ExamStudy(string name, TimeSpan estimatedTime, Subject subject, DateTime dueDate) : base(name, estimatedTime, subject, dueDate)
    {

    }

    public override double GetPriority(DateTime currentDate)
    {
        int daysRemaining = (int)(dueDate - currentDate).TotalDays;

        int optimalStudyDays = 14;

        double priority = 0.0;

        if (daysRemaining >= 0 && daysRemaining <= optimalStudyDays)
        {
            priority = 0.75;
        }
        else if (daysRemaining > optimalStudyDays)
        {
            priority = 0.1;
        }

        return priority;
    }
}