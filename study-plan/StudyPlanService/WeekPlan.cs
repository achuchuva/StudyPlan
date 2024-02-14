namespace StudyPlanService;

public class WeekPlan
{
    private DayPlan[] days = new DayPlan[7];
    public DayPlan[] Days
    {
        get { return days; }
    }

    public WeekPlan(DateOnly date)
    {
        for (int i = 0; i < days.Length; i++)
        {
            days[i] = new DayPlan(date);
            date = date.AddDays(1);
        }
    }
}